using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SocketIO3;
using Newtonsoft.Json;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using JsonTextReader = Newtonsoft.Json.JsonTextReader;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance;
    private SocketManager mainServer;
    private SocketManager chatServer;

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
        mainServer = new SocketManager(new Uri("http://127.0.0.1:27000"));
        mainServer.Open();

        //chatServer = new SocketManager(new Uri("http://54.180.191.145:27001"));
        chatServer = new SocketManager(new Uri("http://127.0.0.1:27001"));
        chatServer.Open();

        InitMainHandler();
        InitChatHandler();
    }

    private void InitMainHandler()
    {
        var connect = UI_Manager.Instance.CreateUI<Connecting_UI>();
        mainServer.Socket.On("connect", () =>
        {
            UI_Manager.Instance.RemoveUI(connect);
            UI_Manager.Instance.CreateUI<Login_UI>();
        }); 
        
        mainServer.Socket.On("disconnect", () =>
        {
            UI_Manager.Instance.CleanUI();
            connect = UI_Manager.Instance.CreateUI<Connecting_UI>();
        });
        
        mainServer.Socket.On<bool, int>("sign", (can,status ) =>
        {
            signAction.Invoke(can, status);
        });
        
        mainServer.Socket.On<bool, int>("login", (can,status ) =>
        {
            Debug.Log("로그인");
            Debug.Log("로그인 " + status);
            loginAction.Invoke(can, status);
        });

        mainServer.Socket.On<bool, string>("inventory", (can, json) =>
        {
            if (can)
            {
                var data = json;
                data = data.Replace("[", "");
                data = data.Replace("]", "");
                getInventoryAction.Invoke(JsonConvert.DeserializeObject<Dictionary<string,object>>(data));
                Debug.Log(JsonConvert.DeserializeObject<Dictionary<string,object>>(data));    
            }
            Debug.Log(json);
        });
        
        mainServer.Socket.On<string, int>("get Name", (nick, status) =>
        {
            switch (status)
            {
                case 0:
                    getNameAction.Invoke(nick);
                    break;
            }
        });
    }

    private void InitChatHandler()
    {
        chatServer.Socket.On<ChatData>("chat", (data) =>
        {
            var chatUI = FindObjectOfType<Chat_UI>();
            if (chatUI != null)
            {
                chatUI.OnReceiveMessage(data.nickName, data.message, data.channel);
            }
        });    
    }

    #region Chat
    public void SendChat(ChatData data)
    {
        chatServer.Socket.Emit("chat", data);
    }
    #endregion
    
    #region Inventory

    private Action<Dictionary<string, object>> getInventoryAction;
    public void GetInventory(Action<Dictionary<string, object>> action)
    {
        mainServer.Socket.Emit("inventory", SteamManager.Instance.steamID.ToString());
        getInventoryAction = action;
    }
    #endregion
    
    #region Account
    private Action<bool, int> loginAction;
    public void Login(string pwd)
    {
        mainServer.Socket.Emit("login", SteamManager.Instance.steamID.ToString(), pwd);
        loginAction = (can, code) =>
        {
            Debug.Log(can);
            if (can)
            {
                SetNickname();
                UI_Manager.Instance.CleanUI();
                SceneManager.LoadScene("Lobby");
                UI_Manager.Instance.CreateUI<MainMenu_UI>();
            }
            else
            {
                
            }
        };
    }

    private Action<bool, int> signAction;
    public void Sign(Sign_Popup popup, string nickName, string pwd, string email)
    {
        mainServer.Socket.Emit("sign", SteamManager.Instance.steamID.ToString(), nickName, pwd, email);
        signAction = (can, code) =>
        {
            if (can)
            {
               popup.Remove(); 
            }
            else
            {
                Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
                
                switch (code)
                {
                    default:
                        error.SetTitle("오류");
                        error.SetText("알 수 없는 오류로 로그인 할 수 없습니다");
                        break;
                    case 1:
                        error.SetTitle("오류");
                        error.SetText("이메일을 형식에 맞게 입력해주세요");
                        break;
                    case 2:
                        error.SetTitle("오류");
                        error.SetText("이미 계정이 존재합니다");
                        break;
                    case 3:
                        error.SetTitle("오류");
                        error.SetText("DB 서버에 접근 할 수 없습니다.");
                        break;
                }
            }
        };
    }

    public string nickName = "";
    private Action<string> getNameAction;
    
    private void SetNickname()
    {
        mainServer.Socket.Emit("get name", SteamManager.Instance.steamID.ToString());
        getNameAction = (name) =>
        {
            nickName = name;
            Debug.Log(nickName);
        };
    }

    #endregion
}
