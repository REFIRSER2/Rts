using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO3;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    #region 싱글톤
    public static ChatManager Instance;
    #endregion

    #region 공유
    private Chat_UI chatUI;

    private bool isIngame = false;

    private void Setup()
    {
        chatUI = FindObjectOfType<Chat_UI>();
    }
    public Chat_UI GetChatUI()
    {
        return chatUI;
    }
    #endregion
    
    #region Lobby Chat
    private SocketManager lobby_ChatServer;
    
    private void Setup_Lobby()
    {
        lobby_ChatServer = SocketConnectManager.Instance.GetChatServer();
    }
    #endregion
    
    #region InGame Chat
    private SocketManager ingame_ChatServer;
    
    private void Setup_InGame()
    {
        
        ingame_ChatServer = SocketConnectManager.Instance.GetChatServer();
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
        Setup();
        Setup_Lobby();
        Setup_InGame();
    }


    private void Update()
    {
        
    }
    #endregion

    private void HandlerInit()
    {

    }

    public void NotifyMessage(string msg)
    {
        chatUI.OnNotifyMessage("공지",msg);
    }

    public void SendSystemMessage(string msg)
    {
        chatUI.SendSystemMessage(msg);
    }
    
    public void SendChat(string channel, string msg, bool isTeam)
    {
        ChatData data = new ChatData();
        data.nickName = MainManager.Instance.userProfile.nickName;
        data.channel = channel;
        data.message = msg;
        data.isTeam = isTeam;
        data.isInGame = false; 

        lobby_ChatServer.Socket.Emit("chat", data);
    }
    
    public void SendChat(string msg, bool isTeam)
    {
        ChatData data = new ChatData();
        data.nickName = MainManager.Instance.userProfile.nickName;
        data.message = msg;
        data.lobbyID = SteamManager.Instance.currentLobby.Id.ToString();
        data.isTeam = isTeam;
        data.isInGame = true; 

        lobby_ChatServer.Socket.Emit("chat", data);
    }
}
