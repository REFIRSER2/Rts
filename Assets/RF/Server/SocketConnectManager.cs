using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SocketIO3;
using Newtonsoft.Json;
using Sirenix.Serialization;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

public class SocketConnectManager : MonoBehaviour
{
    public static SocketConnectManager Instance;
    
    private SocketManager mainServer;
    private SocketManager lobbyServer;
    private SocketManager matchServer;
    private SocketManager lobby_ChatServer;
    private SocketManager ingame_ChatServer;

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

        //mainServer = new SocketManager(new Uri("http://54.180.191.145:27000"));
        mainServer = new SocketManager(new Uri("http://54.180.191.145:27000"));
        mainServer.Open();
        
        //lobbyServer = new SocketManager(new Uri("http://54.180.191.145:27001"));
        lobbyServer = new SocketManager(new Uri("http://54.180.191.145:27001"));
        lobbyServer.Open();
        
        //lobby_ChatServer = new SocketManager(new Uri("http://54.180.191.145:27002"));
        //lobby_ChatServer = new SocketManager(new Uri("http://127.0.0.1:27002"));
        //lobby_ChatServer.Open();

        //matchServer = new SocketManager(new Uri("http://54.180.191.145:27003"));
        //matchServer = new SocketManager(new Uri("http://127.0.0.1:27003"));
        //matchServer.Open();

        // = new SocketManager(new Uri("http://54.180.191.145:27004"));
        //ingame_ChatServer = new SocketManager(new Uri("http://127.0.0.1:27004"));
        //ingame_ChatServer.Open();
    }

    public SocketManager GetMainServer()
    {
        return mainServer;
    }

    public SocketManager GetLobbyServer()
    {
        return lobbyServer;
    }

    public SocketManager GetChatServer()
    {
        return lobby_ChatServer;
    }

    public SocketManager GetInGameServer()
    {
        return ingame_ChatServer;
    }
}
