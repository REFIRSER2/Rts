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



public class MatchManager : MonoBehaviour
{
    #region 싱글톤

    public static MatchManager Instance;

    #endregion

    #region 로비
    private SocketManager lobbyServer;

    private void SetupLobby()
    {
        lobbyServer = SocketConnectManager.Instance.GetLobbyServer();
    }

    #endregion

    #region 매치 시스템
    public Action findQuickMatchAction;
    public Action leaveQuickMatchAction;
    public Action<Queue<string>> createQuickMatchAction;
    public Action acceptQuickMatchAction;
    public Action cancelQuickMatchAction;
    
    private int rank = 0;
    private int mmr = 0;

    private MemberData localData = new MemberData();

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
        
        lobbyServer.Socket.On("join quick match", () =>
        {
            
        });
        
        lobbyServer.Socket.On<string>("invite quick match", (lobby) =>
        {
            MatchAccept_Popup popup = UI_Manager.Instance.CreatePopup<MatchAccept_Popup>();
            popup.SetLobby(Convert.ToUInt64(lobby));
        });

        lobbyServer.Socket.On<List<MemberData>, string>("create quick match lobby", (users,roomName ) =>
        {
            SteamManager.Instance.CreateLobby(users);
        });
        
        lobbyServer.Socket.On<string,List<MemberData>, List<MemberData>>("start quick match", (roomName, team1, team2) =>
        {
            Debug.Log("start quick match");
            Debug.Log(PhotonNetwork.InRoom);
            Debug.Log(roomName);
            
            UI_Manager.Instance.CleanPopup();
            
            if (!PhotonNetwork.InRoom)
            {
                Debug.Log(roomName);
                PhotonNetwork.JoinRoom(roomName);
                //PhotonManager.Instance.redTeams = team1;
               // PhotonManager.Instance.blueTeams = team2;
            }
        });
        
        lobbyServer.Socket.On<string>("create quick match room", (roomName) =>
        {
            PhotonManager.Instance.CreateQuickRoom(roomName);

            Debug.Log("Create Quick Match Room");
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
        localData.id = PhotonNetwork.LocalPlayer.UserId;
        localData.steamID = SteamManager.Instance.steamID.Value.ToString();
        
        Debug.Log(LobbyManager.Instance);
        Debug.Log("find quick match");
        
        LobbyManager.Instance.SetLobbyMembers(LobbyManager.Instance.GetPartyMembers());
        lobbyServer.Socket.Emit("find quick match", gameMode, LobbyManager.Instance.GetLobbyMembers());
        
        //PhotonNetwork.JoinOrCreateRoom();
    }

    public void LeaveQuickMatch()
    {
        lobbyServer.Socket.Emit("leave quick match", gameMode, LobbyManager.Instance.GetLobbyMembers());
        SteamManager.Instance.LeaveLobby();
        //PhotonManager.Instance.LeaveRoom();
    }

    public void AcceptQuickMatch()
    {
        Debug.Log("accept quick match");
        lobbyServer.Socket.Emit("accept quick match", localData);
    }
    
    public void CancelQuickMatch()
    {
        lobbyServer.Socket.Emit("cancel quick match", localData);
    }

    public void CreateQuickMatch(Lobby lobby)
    {
        Debug.Log("Create quick match lobby");
        //lobbyServer.Socket.Emit("create quick match lobby",localData, lobby);
        lobbyServer.Socket.Emit("create quick match lobby", localData, lobby.Id.Value.ToString());
    }

    public void CreateQuickMatchRoom()
    {
        Debug.Log("create quick match room");
        lobbyServer.Socket.Emit("start quick match", name, localData);
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
