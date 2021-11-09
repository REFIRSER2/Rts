using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

public class LobbyManager : MonoBehaviour
{
    #region 싱글톤
    public static LobbyManager Instance;
    #endregion

    #region 파티 시스템
    private SocketManager lobbyServer;
    
    private List<string> partyMembers = new List<string>();
    private int partyID = -1;

    private void SetupParty()
    {
        lobbyServer = SocketConnectManager.Instance.GetLobbyServer();

        lobbyServer.Socket.On("connect", () =>
        {
            
        });
        
        lobbyServer.Socket.On<int, List<string>>("create party", (id, members) =>
        {
            Debug.Log("Create party");
            onCreateParty(id, members);
        });

        lobbyServer.Socket.On("leave party", () =>
        {
            Debug.Log("Leave party");
            onLeaveParty();   
        });

        lobbyServer.Socket.On<string>("leave party member", (id) =>
        {
            onLeavePartyMember(id);
        });

        lobbyServer.Socket.On<int, string>("invite party", (code, id) =>
        {
            onInvite(code, id);
        });

        lobbyServer.Socket.On<int, string, int>("invited party", (code, id, party) =>
        {
            onInvited(code, id, party);
        });
        
        lobbyServer.Socket.On<int, List<string>>("join party", (party, members) =>
        {
            onJoinParty(party, members);
        });
        
        lobbyServer.Socket.On<string>("joined party member", (id) =>
        {
            onJoinPartyMember(id);
        });
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
        lobbyServer.Socket.Emit("create party", id.ToString());
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
        lobbyServer.Socket.Emit("join party", SteamManager.Instance.steamID.ToString(),oldID, newID);
    }

    public void LeaveParty(string id)
    {
        lobbyServer.Socket.Emit("leave party", id,GetPartyID());
    }

    private void RefreshParty()
    {
        if (FindObjectOfType<MainMenu_UI>() != null)
        {
            FindObjectOfType<MainMenu_UI>().RefreshParty();   
        }    
    }
    
    private void onCreateParty(int id, List<string> members)
    {
        partyID = id;
        partyMembers = members;

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

    private void onJoinParty(int id, List<string> members)
    {
        partyID = id;
        partyMembers = members;

        Debug.Log(partyMembers.Count);
        
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
    }*/
    #endregion
    
    #region 매치 시스템

    private void SetupMatch()
    {
       lobbyServer.Socket.On("find quick match", () =>
       {
           onFindQuickMatch();
       });
    }

    private Action findQuickMatchAction;
    public void AddQuickMatchAction(Action action)
    {
        findQuickMatchAction = action;
    }
    public void FindQuickMatch(int gamemode, Action action)
    {
        lobbyServer.Socket.Emit("find quick match", gamemode, GetPartyMembers());
        
    }
    
    private void onFindQuickMatch()
    {
        findQuickMatchAction.Invoke(); 
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
        SetupMatch();
    }

    #endregion
}
