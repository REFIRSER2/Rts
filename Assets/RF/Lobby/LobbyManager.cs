using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Steamworks;
using UnityEngine;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

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

    #region 로비
    private List<MemberData> lobbyMembers = new List<MemberData>();

    public void SetLobbyMembers(List<MemberData> list)
    {
        lobbyMembers = list;
    }
    
    public List<MemberData> GetLobbyMembers()
    {
        return lobbyMembers;
    }
    #endregion
    
    #region 파티 시스템
    private SocketManager partyServer;
    
    private List<MemberData> partyMembers = new List<MemberData>();
    private int partyID = -1;

    public Queue<PartyInvited_Popup> party_Popups = new Queue<PartyInvited_Popup>();

    private void SetupParty()
    {
        partyServer = SocketConnectManager.Instance.GetLobbyServer();

        partyServer.Socket.On("connect", () => { });

        partyServer.Socket.On<int, List<MemberData>>("create party", (id, datas) =>
        {
            Debug.Log("Create party");
            onCreateParty(id, datas);
        });

        partyServer.Socket.On("leave party", () =>
        {
            Debug.Log("Leave party");
            onLeaveParty();
        });

        partyServer.Socket.On<string>("leave party member", (id) => { onLeavePartyMember(id); });

        partyServer.Socket.On<int, string>("invite party", (code, id) => { onInvite(code, id); });

        partyServer.Socket.On<int, string, int>("invited party", (code, id, party) => { onInvited(code, id, party); });

        partyServer.Socket.On<int, List<MemberData>>("join party",
            (party, datas) => { onJoinParty(party, datas); });

        partyServer.Socket.On<List<MemberData>>("joined party member", (datas) => { onJoinPartyMember(datas); });
    }

    public int GetPartyID()
    {
        return partyID;
    }

    public List<MemberData> GetPartyMembers()
    {
        return partyMembers;
    }

    public void CreateParty(ulong id)
    {
        Debug.Log("create party");
        MemberData memberData = new MemberData();
        memberData.id = PhotonNetwork.LocalPlayer.UserId;
        memberData.steamID = id.ToString();
        
        partyServer.Socket.Emit("create party", id.ToString(), memberData);
    }

    public void LeaveParty(ulong id)
    {
        if (partyID == -1)
        {
            return;
        }

        partyServer.Socket.Emit("leave party", id);
    }

    public void InviteParty(string id)
    {
        partyServer.Socket.Emit("invite party", SteamManager.Instance.steamID.ToString(), id, partyID);
    }

    public void JoinParty(int oldID, int newID)
    {
        MemberData data = new MemberData();
        data.id = PhotonNetwork.LocalPlayer.UserId;
        data.steamID = SteamManager.Instance.steamID.Value.ToString();
        partyServer.Socket.Emit("join party", data, oldID, newID);
    }

    public void LeaveParty(string id)
    {
        partyServer.Socket.Emit("leave party", id, GetPartyID());
    }

    private void RefreshParty()
    {
        if (FindObjectOfType<MainMenu_UI>() != null)
        {
            FindObjectOfType<MainMenu_UI>().RefreshParty();
        }
    }

    private void onCreateParty(int id, List<MemberData> memberDatas)
    {
        partyID = id;
        partyMembers = memberDatas;

        RefreshParty();
    }

    private void onLeaveParty()
    {
        partyID = -1;
        partyMembers.Clear();

        CreateParty(SteamManager.Instance.steamID);

        RefreshParty();
    }

    private void onLeavePartyMember(string id)
    {
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (partyMembers[i].steamID == id)
            {
                partyMembers.RemoveAt(i);
            }
        }
        RefreshParty();
    }

    private void onJoinParty(int id, List<MemberData> memberDatas)
    {
        partyID = id;
        partyMembers = memberDatas;

        SteamManager.Instance.SetLobbyMembers(memberDatas);

        RefreshParty();
    }

    private void onJoinPartyMember(List<MemberData> memberDatas)
    {
        partyMembers = memberDatas;
        //Debug.Log(id);

        RefreshParty();
    }

    private void onInvite(int code, string receiver)
    {
        switch (code)
        {
            case 0:
                PartyInvite_Popup popup = UI_Manager.Instance.CreatePopup<PartyInvite_Popup>();

                ulong id = Convert.ToUInt64(receiver);
                SteamId steam = (SteamId) id;

                Friend friend = new Friend(steam);

                Debug.Log(friend.Name);

                popup.SetTitle("알림");
                popup.SetText(friend.Name + " 님을 초대하였습니다");
                break;
        }

    }

    private void onInvited(int code, string sender, int party)
    {
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
        }
    }
    #endregion
    
    #region 유니티 기본 함수
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetupParty();
    }

    #endregion
}
