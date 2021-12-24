using System;
using System.Collections.Generic;
using Photon.Pun;
using RF.Steam;
using RF.UI;
using RF.UI.Connecting;
using RF.UI.MainMenu;
using RF.UI.Popup;
using Steamworks;
using UnityEngine;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

namespace RF.Lobby
{
    public class Party
    {
        public int id = -1;
        public List<MemberData> memberList = new List<MemberData>();
    }
    
    public class MemberData
    {
        public string id = "";
        public string steamID = "";
    }
    
    public class LobbyManager : MonoBehaviour
    {
        #region 싱글톤
        public static LobbyManager Instnace;
        #endregion

        #region 서버
        private SocketManager server;

        private void Setup()
        {
            server = new SocketManager(new Uri("http://54.180.191.145:27001"));
            server.Open();
            
            //var connectingUI = UI_Manager.Instance.CreateUI<Connecting_UI>();
            server.Socket.On("connect", () =>
            {
                Debug.Log("Connected");
                //connectingUI.OnConnected();
            });

            
            // UI_Manager.Instance.CreateUI<>();
        }
        #endregion
        
        #region 파티
        public Party party;
        
        private void Setup_Party()
        {
            server.Socket.On<int, List<MemberData>>("create party", (id, datas) =>
            {
                Debug.Log("Create party");
                OnCreateParty(id, datas);
            });
            
            server.Socket.On<string>("leave party member", (id) => { OnLeavePartyMember(id); });

            server.Socket.On<int, string>("invite party", (code, id) => { OnInvite(code, id); });

            server.Socket.On<int, string, int>("invited party", (code, id, party) => { OnInvited(code, id, party); });

            server.Socket.On<int, List<MemberData>>("join party",
                (party, datas) => { OnJoinParty(party, datas); });

            server.Socket.On<List<MemberData>>("joined party member", (datas) => { OnJoinPartyMember(datas); });
        }
        
        public void CreateParty(ulong id)
        {
            party = new Party();
 
            MemberData memberData = new MemberData();
            memberData.id = PhotonNetwork.LocalPlayer.UserId;
            memberData.steamID = id.ToString();
        
            server.Socket.Emit("create party", id.ToString(), memberData);
        }

        public void OnCreateParty(int id, List<MemberData> memberDatas)
        {
            party.id = id;
            party.memberList = memberDatas;

            Refresh();
        }
        
        public void LeaveParty(ulong id)
        {
            if (party.id == -1)
            {
                return;
            }

            server.Socket.Emit("leave party", id);
        }
        
        private void OnLeaveParty()
        {
            party.id = -1;
            party.memberList.Clear();

            CreateParty(SteamManager.Instance.GetAccount().steamID);

            Refresh();
        }
        
        private void OnLeavePartyMember(string id)
        {
            for (int i = 0; i < party.memberList.Count; i++)
            {
                if (party.memberList[i].steamID == id)
                {
                    party.memberList.RemoveAt(i);
                }
            }
            Refresh();
        }
        
        public void InviteParty(string id)
        {
            server.Socket.Emit("invite party", SteamManager.Instance.GetAccount().steamID.ToString(), id, party.id);
        }
        
        private void OnInvite(int code, string receiver)
        {
            switch (code)
            {
                case 0:
                    PartyInvite_Popup popup = UI_Manager.Instance.CreatePopupView("PartyInvite_Popup") as PartyInvite_Popup;

                    ulong id = Convert.ToUInt64(receiver);
                    SteamId steam = (SteamId) id;

                    Friend friend = new Friend(steam);

                    Debug.Log(friend.Name);

                    popup.SetTitle("알림");
                    popup.SetMain(friend.Name + " 님을 초대하였습니다");
                    break;
            }

        }
        
        private void OnInvited(int code, string sender, int id)
        {
            /*
            switch (code)
            {
                case 0:
                    if (FindObjectOfType<PartyInvited_Popup>() != null)
                    {
                        PartyInvited_Popup q_popup = UI_Manager.Instance.CreatePopup<PartyInvited_Popup>();

                        ulong q_id = Convert.ToUInt64(sender);
                        SteamId q_steam = (SteamId) q_id;

                        Friend q_friend = new Friend(q_steam);

                        q_popup.SetPartyID(party);
                        q_popup.SetTitle("알림");
                        q_popup.SetText(q_friend.Name + " 님이 파티 초대를 하였습니다");

                        q_popup.gameObject.SetActive(false);

                        party_Popups.Enqueue(q_popup);
                        return;
                    }

                    PartyInvited_Popup popup = UI_Manager.Instance.CreatePopup<PartyInvited_Popup>();

                    ulong id = Convert.ToUInt64(sender);
                    SteamId steam = (SteamId) id;

                    Friend friend = new Friend(steam);

                    popup.SetPartyID(party);
                    popup.SetTitle("알림");
                    popup.SetText(friend.Name + " 님이 파티 초대를 하였습니다");
                    break;
            }*/
        }
        
        public void JoinParty(int oldID, int newID)
        {
            MemberData data = new MemberData();
            data.id = PhotonNetwork.LocalPlayer.UserId;
            data.steamID = SteamManager.Instance.GetAccount().steamID.Value.ToString();
            server.Socket.Emit("join party", data, oldID, newID);
        }
        
        private void OnJoinParty(int id, List<MemberData> memberDatas)
        {
            party.id = id;
            party.memberList = memberDatas;

            Refresh();
        }

        private void OnJoinPartyMember(List<MemberData> memberDatas)
        {
            party.memberList = memberDatas;
            //Debug.Log(id);

            Refresh();
        }

        private void Refresh()
        {
            var mainMenu = UI_Manager.Instance.GetUIView("MainMenu_UI") as MainMenu_UI;
            
            if (mainMenu != null)
            {
                var model = mainMenu.GetModel();
                
                model.OnRefreshParty();
            }
        }
        #endregion
        
        #region 유니티 기본  내장 함수
        private void Awake()
        {
            if (Instnace == null)
            {
                Instnace = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

            Setup();
            Setup_Party();
        }
        #endregion
    }
}
