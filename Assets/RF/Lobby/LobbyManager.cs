using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using RF.Photon;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

public class PartyMemberData
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

    private SocketManager lobbyServer;

    private void SetupLobby()
    {
        lobbyServer = SocketConnectManager.Instance.GetLobbyServer();
    }

    #endregion


    #region 파티 시스템

    private List<string> partyMembers = new List<string>();
    private List<string> partyMemberIds = new List<string>();
    private int partyID = -1;

    public Queue<PartyInvited_Popup> party_Popups = new Queue<PartyInvited_Popup>();

    private void SetupParty()
    {
        lobbyServer = SocketConnectManager.Instance.GetLobbyServer();

        lobbyServer.Socket.On("connect", () => { });

        lobbyServer.Socket.On<int, List<string>, List<string>>("create party", (id, members, ids) =>
        {
            Debug.Log("Create party");
            onCreateParty(id, members, ids);
        });

        lobbyServer.Socket.On("leave party", () =>
        {
            Debug.Log("Leave party");
            onLeaveParty();
        });

        lobbyServer.Socket.On<string>("leave party member", (id) => { onLeavePartyMember(id); });

        lobbyServer.Socket.On<int, string>("invite party", (code, id) => { onInvite(code, id); });

        lobbyServer.Socket.On<int, string, int>("invited party", (code, id, party) => { onInvited(code, id, party); });

        lobbyServer.Socket.On<int, List<string>, Dictionary<SteamId, SteamLobbyClient>>("join party",
            (party, members, membersData) => { onJoinParty(party, members, membersData); });

        lobbyServer.Socket.On<string>("joined party member", (id) => { onJoinPartyMember(id); });
    }

    public int GetPartyID()
    {
        return partyID;
    }

    public List<string> GetPartyMembers()
    {
        return partyMembers;
    }

    public void CreateParty(ulong id)
    {
        Debug.Log("create party");
        lobbyServer.Socket.Emit("create party", id.ToString(), PhotonNetwork.LocalPlayer.UserId);
    }

    public void LeaveParty(ulong id)
    {
        if (partyID == -1)
        {
            return;
        }

        lobbyServer.Socket.Emit("leave party", id);
    }

    public void InviteParty(string id)
    {
        lobbyServer.Socket.Emit("invite party", SteamManager.Instance.steamID.ToString(), id, partyID);
    }

    public void JoinParty(int oldID, int newID)
    {
        lobbyServer.Socket.Emit("join party", SteamManager.Instance.GetLobbyMembers(),
            SteamManager.Instance.steamID.ToString(), oldID, newID);
    }

    public void LeaveParty(string id)
    {
        lobbyServer.Socket.Emit("leave party", id, GetPartyID());
    }

    private void RefreshParty()
    {
        if (FindObjectOfType<MainMenu_UI>() != null)
        {
            FindObjectOfType<MainMenu_UI>().RefreshParty();
        }
    }

    private void onCreateParty(int id, List<string> members, List<string> ids)
    {
        partyID = id;
        partyMembers = members;
        partyMemberIds = ids;

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
        if (partyMembers.Contains(id))
        {
            partyMembers.Remove(id);
        }

        RefreshParty();
    }

    private void onJoinParty(int id, List<string> members, Dictionary<SteamId, SteamLobbyClient> membersData)
    {
        partyID = id;
        partyMembers = members;

        SteamManager.Instance.SetLobbyMembers(membersData);

        RefreshParty();
    }

    private void onJoinPartyMember(string id)
    {
        partyMembers.Add(id);

        Debug.Log(id);

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
    
        #region 매치 시스템

    public Action findQuickMatchAction;
    public Action leaveQuickMatchAction;
    public Action<Queue<string>> createQuickMatchAction;
    public Action acceptQuickMatchAction;
    public Action cancelQuickMatchAction;

    private int team = -1;
    private int rank = 0;
    private int mmr = 0;

    public void RefreshMatch()
    {
        lobbyServer.Socket.Emit("get rank");

    }
    
    private void SetupMatch()
    {
        lobbyServer.Socket.On<int>("get rank", (num) =>
        {
            rank = num;
        });
        
        lobbyServer.Socket.On("find quick match", () =>
        {
            findQuickMatchAction.Invoke();
        });
        
        lobbyServer.Socket.On("leave quick match", () =>
        {
            leaveQuickMatchAction.Invoke();
            //PhotonManager.Instance.LeaveRoom();
        });
        
        lobbyServer.Socket.On<Lobby>("invite quick match", (lobby) =>
        {
            MatchAccept_Popup popup = UI_Manager.Instance.CreatePopup<MatchAccept_Popup>();
            popup.SetLobby(lobby);
        });

        lobbyServer.Socket.On<List<string>, string>("create quick match", (users,roomName ) =>
        {
            SteamManager.Instance.CreateLobby(users);
            
        });
        
        lobbyServer.Socket.On<string,List<string>, List<string>>("start quick match", (name, team1, team2) =>
        {
            if (!PhotonNetwork.InRoom)
            {
                PhotonNetwork.JoinRoom(name);
            }
        });
        
        lobbyServer.Socket.On<string>("make quick match room", (name) =>
        {
            PhotonManager.Instance.CreateQuickRoom(name);

            lobbyServer.Socket.Emit("start quick match", name, SteamManager.Instance.steamID.Value.ToString());
        });
        
        lobbyServer.Socket.On("accept quick match", () =>
        {
            acceptQuickMatchAction.Invoke();
        });
        
        lobbyServer.Socket.On("cancel quick match", () =>
        {
            SteamManager.Instance.LeaveLobby();
        });
    }

    public void SetGamemode(int mode)
    {
        gameMode = mode;
    }

    public int GetGamemode()
    {
        return gameMode;
    }

    public void FindQuickMatch()
    {
        lobbyServer.Socket.Emit("find quick match", gameMode, GetPartyMembers());
        
        //PhotonNetwork.JoinOrCreateRoom();
    }

    public void LeaveQuickMatch()
    {
        lobbyServer.Socket.Emit("leave quick match", gameMode, GetPartyMembers());
        PhotonManager.Instance.LeaveRoom();
    }

    public void AcceptQuickMatch()
    {
        lobbyServer.Socket.Emit("accept quick match", SteamManager.Instance.steamID.Value.ToString());
    }
    
    public void CancelQuickMatch()
    {
        lobbyServer.Socket.Emit("cancel quick match", SteamManager.Instance.steamID.Value.ToString());
    }

    public void CreateQuickMatch(Lobby lobby, SteamId id)
    {
        lobbyServer.Socket.Emit("create quick match",id.ToString(), lobby);
    }
    #endregion
    
    
    #region 게임모드
    private int gameMode = 0;

    public void SetGameMode(int num)
    {
        gameMode = num;
    }
    
    public int GetGameMode()
    {
        return gameMode;
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
        SetupLobby();
        SetupParty();
        SetupMatch();
        //SetupMatch();
    }

    #endregion
}


    
    /*
    public void CreateParty(ulong id)
    {
        partyMembers.Clear();
        partyMembers.Add(id);

        FindObjectOfType<MainMenu_UI>().RefreshParty();
    }

    public void JoinParty(List<ulong> members)
    {
        partyMembers = members;
    }
    #endregion
    

}*/
