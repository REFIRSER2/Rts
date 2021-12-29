using System;
using System.Collections.Generic;
using Photon.Pun;
using RF.Photon;
using RF.Room;
using RF.Steam;
using RF.UI;
using RF.UI.Connecting;
using RF.UI.Friend;
using RF.UI.MainMenu;
using RF.UI.Popup;
using RF.UI.Top;
using Steamworks;
using UnityEngine;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

namespace RF.Lobby
{
    public enum Gamemode
    {
        Normal,
        AI,
        Rank,
    }
    
    public class Party
    {
        public int id = -1;
        public List<MemberData> memberList = new List<MemberData>();
    }

    public class LobbyData
    {
        public List<MemberData> members = new List<MemberData>();
    }
    
    public class MemberData
    {
        public string id = "";
        public string steamID = "";
    }
    
    public class LobbyManager : MonoBehaviour
    {
        #region 싱글톤
        public static LobbyManager Instance;
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
        public MemberData localMember;
        public Queue<PartyInvited_Popup> invited_Popups = new Queue<PartyInvited_Popup>();
        
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
 
            Debug.Log("party id : " + PhotonManager.Instance.GetLocalID());
            
            localMember = new MemberData();
            localMember.id = PhotonManager.Instance.GetLocalID();
            localMember.steamID = id.ToString();
        
            server.Socket.Emit("create party", id.ToString(), localMember);
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
            PartyInvited_Popup popup;
            ulong senderID = 0;
            switch (code)
            {
                case 0:
                    if (FindObjectOfType<PartyInvited_Popup>() != null)
                    {
                        popup = UI_Manager.Instance.CreatePopupView("PartyInvited_Popup") as PartyInvited_Popup;

                        senderID = Convert.ToUInt64(sender);
                        SteamId q_steam = (SteamId) senderID;

                        Friend q_friend = new Friend(q_steam);

                        popup.SetParty(id);
                        popup.SetTitle("알림");
                        popup.SetMain(q_friend.Name + " 님이 파티 초대를 하였습니다");

                        popup.gameObject.SetActive(false);

                        invited_Popups.Enqueue(popup);
                        return;
                    }

                    popup = UI_Manager.Instance.CreatePopupView("PartyInvited_Popup") as PartyInvited_Popup;

                    senderID = Convert.ToUInt64(sender);
                    SteamId steam = (SteamId) senderID;

                    Friend friend = new Friend(steam);

                    popup.SetParty(id);
                    popup.SetTitle("알림");
                    popup.SetMain(friend.Name + " 님이 파티 초대를 하였습니다");
                    break;
            }
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
            var mainMenu = UI_Manager.Instance.GetUIView("Friend_UI") as Friend_UI;
            
            if (mainMenu != null)
            {
                var model = mainMenu.GetModel();
                
                model.OnPartyRefresh(party);
            }
        }
        #endregion
        
        #region 매칭
        private int gamemode = 0;
        private LobbyData lobbyData;
        
        private void Setup_Match()
        {
            server.Socket.On("find quick match", () =>
            {
                var ui = UI_Manager.Instance.GetUIView("Top_UI") as Top_UI;
                ui.GetModel().OnStartFindQuickMatch();
            });
            
            server.Socket.On("leave quick match", () =>
            {
                var ui = UI_Manager.Instance.GetUIView("Top_UI") as Top_UI;
                ui.GetModel().OnLeaveFindQuickMatch();               
            });
            
            server.Socket.On("cancel quick match", () =>
            {
               UI_Manager.Instance.RemovePopup("MatchAccept_Popup", 0); 
            });
            
            server.Socket.On<List<MemberData>>("create quick match lobby", (members) =>
            {
                Debug.Log("create quick match lobby");

                foreach (var data in members)
                {
                    Debug.Log("id : " + data.steamID + "photon : " + data.id);
                }
                
                CreateQuickMatchLobby(members);
            });
            
            server.Socket.On<string>("create quick match room", (roomName) =>
            {
                CreateQuickMatchRoom(roomName);
            });
            
            server.Socket.On<string>("invite quick match", (lobbyID) =>
            {
                Debug.Log("invite lobby");
                InviteQuickMatch(lobbyID);
            });
            
            server.Socket.On<string, List<MemberData>, List<MemberData>>("start quick match", (roomName, team1, team2) =>
            {
                StartQuickMatch(roomName, team1, team2);
            });
            
            server.Socket.On("join quick match", () =>
            {
                
            });
        }
        
        public void SetGamemode(int mode)
        {
            gamemode = mode;
        }

        public int GetGamemode()
        {
            return gamemode;
        }

        public LobbyData GetLobbyData()
        {
            return lobbyData;
        }

        public void FindQuickMatch()
        {
            server.Socket.Emit("find quick match", GetGamemode(), LobbyManager.Instance.party.memberList);
        }

        public void LeaveQuickMatch()
        {
            Debug.Log("leave quick match try");
            server.Socket.Emit("leave quick match", localMember, LobbyManager.Instance.party.memberList);
        }

        public void AcceptQuickMatch(string id)
        {
            SteamManager.Instance.JoinLobby(id);
            
            server.Socket.Emit("accept quick match", localMember);
        }
        
        public void CancelQuickMatch()
        {
            server.Socket.Emit("cancel quick match", localMember);
        }

        private void CreateQuickMatchLobby(List<MemberData> members)
        {
            Debug.Log("매치 로비 생성");
            lobbyData = new LobbyData();
            lobbyData.members = members;
            SteamManager.Instance.CreateLobby();
        }

        public void OnCreateQuickMatchLobby()
        {
            server.Socket.Emit("create quick match lobby", localMember, SteamManager.Instance.GetLobby().Id.ToString());
        }
        
        private void CreateQuickMatchRoom(string roomName)
        {
            Debug.Log("매치 룸 생성");
            RoomManager.Instance.CreateRoom(roomName);
        }

        public void OnCreateQuickMatchRoom()
        {
            Debug.Log("매치 룸 생성 완료");
            //server.Socket.Emit("");
        }

        private void InviteQuickMatch(string lobbyID)
        {
            Debug.Log("매치 초대");
            var popup = UI_Manager.Instance.CreatePopupView("MatchAccept_Popup") as MatchAccept_Popup;
            popup.SetLobby(lobbyID);
        }

        public void OnJoinQuickMatchRoom(int plCount)
        {

        }

        public void StartQuickMatch(string roomName, List<MemberData> team1, List<MemberData> team2)
        {
            Debug.Log("매치 시작");
        }
        #endregion
        
        #region 유니티 기본  내장 함수
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

            Setup();
            Setup_Party();
            Setup_Match();
        }
        #endregion
    }
}
