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

        InitHandler();
    }

    private void InitHandler()
    {
        var connect = UI_Manager.Instance.CreateUI<Connecting_UI>();
        socket.Socket.On("connect", () =>
        {
            UI_Manager.Instance.RemoveUI(connect);
            UI_Manager.Instance.CreateUI<Login_UI>();
        }); 
        
        socket.Socket.On<bool, int>("sign", (can,status ) =>
        {
            Debug.Log(can);
            Debug.Log(status);
            signAction.Invoke(can, status);
        });
        
        socket.Socket.On<bool, int>("login", (can,status ) =>
        {
            Debug.Log(can);
            Debug.Log(status);
            loginAction.Invoke(can, status);
        });
        
        socket.Socket.On<ChatData>("chat", (data) =>
        {
            
        });
        
        socket.Socket.On<string>("getInventory", (json) =>
        {
            var data = json;
            data = data.Replace("[", "");
            data = data.Replace("]", "");
            getInventoryAction.Invoke(JsonConvert.DeserializeObject<Dictionary<string,object>>(data));
        });
    }

    #region Chat
    public void SendChat(ChatData data)
    {
        socket.Socket.Emit("chat", data);
    }
    #endregion
    
    #region Inventory

    private Action<Dictionary<string, object>> getInventoryAction;
    public void GetInventory(Action<Dictionary<string, object>> action)
    {
        socket.Socket.Emit("getInventory", SteamManager.Instance.steamID.ToString());
        getInventoryAction = action;
    }
    #endregion
    
    #region Account
    private Action<bool, int> loginAction;
    public void Login(string pwd)
    {
        socket.Socket.Emit("login", SteamManager.Instance.steamID.ToString(), pwd);
        loginAction = (can, code) =>
        {
            if (can)
            {
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
        socket.Socket.Emit("sign", SteamManager.Instance.steamID.ToString(), nickName, pwd, email);
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
    #endregion
}
