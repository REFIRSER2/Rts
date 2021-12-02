using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

public class MainManager : MonoBehaviour
{
    #region 싱글톤
    public static MainManager Instance;
    #endregion
    
    #region Main DB
    private SocketManager mainServer;

    public UserInfo userInfo = new UserInfo();
    
    private Connecting_UI connecting_UI;
    
    private Action<Dictionary<string, object>> callback_getInventory;
    private Action<int> callback_sign;
    private Action<Dictionary<string, object>> callback_profile;
    
    private void Setup()
    {
        mainServer = SocketConnectManager.Instance.GetMainServer();

        connecting_UI = UI_Manager.Instance.CreateUI<Connecting_UI>();
        mainServer.Socket.On("connect", () =>
        {
            onConnected();
        });
        
        mainServer.Socket.On("disconnect", () =>
        {
            onDisconnected();
        });
        
        mainServer.Socket.On<int>("login", (code) =>
        {
            onLogin(code);
        });
        
        mainServer.Socket.On<int>("sign", (code) =>
        {
            onSign(code);
        });
        
        mainServer.Socket.On<int, string>("inventory", (code, json) =>
        {
            onGetInventory(code, json);
        });
        
        mainServer.Socket.On<int, string>("profile", (code, json) =>
        {
            onGetProfile(code, json);
        });
    }

    public void Login(string pwd)
    {
        mainServer.Socket.Emit("login", SteamManager.Instance.steamID.ToString(), pwd);
    }

    public void Sign(string nick, string pwd, string email, Action<int> action)
    {
        mainServer.Socket.Emit("sign", SteamManager.Instance.steamID.ToString(), nick, pwd, email);

        callback_sign -= action;
        callback_sign += action;
    }

    public void GetInventory(Action<Dictionary<string, object>> action)
    {
        mainServer.Socket.Emit("inventory", SteamManager.Instance.steamID.ToString());
        callback_getInventory -= action;
        callback_getInventory += action;
    }

    public void GetProfile()
    {
        mainServer.Socket.Emit("profile", SteamManager.Instance.steamID.ToString());
        callback_profile = (data) =>
        {
            userInfo.nickName = data["nickname"].ToString();
            userInfo.rank = Convert.ToInt32(data["rank"]);
            userInfo.rankPt = Convert.ToInt32(data["rankpoint"]);
            userInfo.mmr = Convert.ToInt32(data["mmr"]);
        };
    }

    private void onConnected()
    {
        UI_Manager.Instance.ReleaseUI<Connecting_UI>();
        UI_Manager.Instance.CreateUI<First_UI>();   
    }

    private void onDisconnected()
    {
        SteamManager.Instance.LeaveLobby();
        //LobbyManager.Instance.LeaveParty(SteamManager.Instance.steamID.ToString());
        
        UI_Manager.Instance.CleanUI();
        connecting_UI = UI_Manager.Instance.CreateUI<Connecting_UI>();
    }

    private void onLogin(int code)
    {
        Error_Popup error;
        switch (code)
        {
            default:
                error =  UI_Manager.Instance.CreatePopup<Error_Popup>();
                error.SetTitle("오류");
                error.SetText("알 수 없는 오류로 로그인 할 수 없습니다");
                break;
            case 0:
                UI_Manager.Instance.CleanUI();
                UI_Manager.Instance.CreateUI<MainMenu_UI>();
                SceneManager.LoadScene("Lobby");
                
                LobbyManager.Instance.JoinSocketChannel(SteamManager.Instance.steamID.ToString());
                //LobbyManager.Instance.CreateParty(SteamManager.Instance.steamID);
                GetProfile();
                break;
            case 1:
                error =  UI_Manager.Instance.CreatePopup<Error_Popup>();
                error.SetTitle("오류");
                error.SetText("DB에서 계정을 찾을 수 없습니다");
                break;
            case 2:
                error =  UI_Manager.Instance.CreatePopup<Error_Popup>();
                error.SetTitle("오류");
                error.SetText("비밀번호가 일치하지 않습니다");
                break;
            case 3:
                error =  UI_Manager.Instance.CreatePopup<Error_Popup>();
                error.SetTitle("오류");
                error.SetText("DB 서버에 접근 할 수 없습니다.");
                break;
        }
    }

    private void onSign(int code)
    {
        callback_sign.Invoke(code);
    }

    private void onGetInventory(int code, string json)
    {
        switch (code)
        {
            case 0:
                var rep = json;
                rep = rep.Replace("[", "");
                rep = rep.Replace("]", "");

                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rep);
                
                callback_getInventory.Invoke(data);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    private void onGetProfile(int code, string json)
    {
        var dataStr = "";
        Dictionary<string, object> data = null;
        
        switch (code)
        {
            case 0:
                dataStr = json;
                dataStr = dataStr.Replace("[", "");
                dataStr = dataStr.Replace("]", "");

                data = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                callback_profile.Invoke(data); 
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
        Setup();
    }

    private void OnApplicationQuit()
    {
        //LobbyManager.Instance.LeaveParty(SteamManager.Instance.steamID.ToString());
        SteamManager.Instance.LeaveLobby();
    }

    #endregion
}
