using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO3;
using Newtonsoft.Json;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    #region 싱글톤
    public static MainManager Instance;
    #endregion
    
    #region Main DB
    private SocketManager mainServer;

    private Connecting_UI connecting_UI;

    private Action<Dictionary<string, object>> callback_getInventory;
    private Action<int> callback_sign;
    
    private void Setup()
    {
        mainServer = SocketConnectManager.Instance.GetMainServer();
        
        connecting_UI = UI_Manager.Instance.CreateUI<Connecting_UI>();
        mainServer.Socket.On("connection", () =>
        {
            onConnected(connecting_UI);
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
    }

    public void Login(string pwd)
    {
        mainServer.Socket.Emit("login", SteamManager.Instance.steamID.ToString(), pwd);
    }

    public void Sign(Sign_Popup popup, string nick, string pwd, string email)
    {
        mainServer.Socket.Emit("sign", SteamManager.Instance.steamID.ToString(), nick, pwd, email);
    }

    public void GetInventory(Action<Dictionary<string, object>> action)
    {
        mainServer.Socket.Emit("inventory", SteamManager.Instance.steamID.ToString());
        callback_getInventory -= action;
        callback_getInventory += action;
    }

    private void onConnected(Connecting_UI connectingUI)
    {
        UI_Manager.Instance.RemoveUI(connectingUI);
        UI_Manager.Instance.CreateUI<Login_UI>();   
    }

    private void onDisconnected()
    {
        UI_Manager.Instance.CleanUI();
        connecting_UI = UI_Manager.Instance.CreateUI<Connecting_UI>();
    }

    private void onLogin(int code)
    {
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        switch (code)
        {
            default:
                error.SetTitle("오류");
                error.SetText("알 수 없는 오류로 로그인 할 수 없습니다");
                break;
            case 0:
                error.Remove();
                break;
            case 1:
                error.SetTitle("오류");
                error.SetText("DB에서 계정을 찾을 수 없습니다");
                break;
            case 2:
                error.SetTitle("오류");
                error.SetText("비밀번호가 일치하지 않습니다");
                break;
            case 3:
                error.SetTitle("오류");
                error.SetText("DB 서버에 접근 할 수 없습니다.");
                break;
        }
    }

    private void onSign(int code)
    {
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        switch (code)
        {
            default:
                error.SetTitle("오류");
                error.SetText("알 수 없는 오류로 회원가입 할 수 없습니다");
                break;
            case 0:
                
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
    #endregion
    
    #region 유니티 기본 함수
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        Setup();
    }
    #endregion
}
