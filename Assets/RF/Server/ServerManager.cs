using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SocketIO3;
using Sirenix.Serialization;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;
    private SocketManager socket;

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

        socket = new SocketManager(new Uri("http://127.0.0.1:27000"));
        socket.Open();

        Debug.Log(socket.Socket.IsOpen);

        InitHandler();
 
    }

    private void InitHandler()
    {
        socket.Socket.On("connect", () =>
        {
            UI_Manager.Instance.CreateUI<First_UI>(); 
        }); 
        
        socket.Socket.On<bool, int>("login", (canLogin,status ) =>
        {
            Debug.Log(canLogin);
            Debug.Log(status);
        });
    }

    public void Login(string pwd)
    {
        socket.Socket.Emit("login", SteamManager.Instance.steamID.ToString(), pwd);
    }
}
