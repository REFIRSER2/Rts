using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using LitJson;
using UnityEngine.SceneManagement;

public class BackendManager : MonoBehaviour
{
    #region Singletone
    public static BackendManager Instance;
    #endregion
    
    #region Database

    private void BackendInit()
    {
        UI_Manager.Instance.CreateUI<First_UI>();
        
        var backend = Backend.Initialize(true);
        if (backend.IsSuccess())
        {
            Debug.Log("초기화 성공");
        }
        else
        {
            Application.Quit();
        }
        
        Handler();
    }

    public void Sign(Sign_Popup popup, string nickname, string email, string pwd)
    {
        string id = SteamManager.Instance.steamID.ToString();

        var sign = Backend.BMember.CustomSignUp(id, pwd);
        
        if (sign.IsSuccess())
        {
            var emailUpdate= Backend.BMember.UpdateCustomEmail(email);
            var nickCreate = Backend.BMember.CreateNickname(nickname);
            
            if (!emailUpdate.IsSuccess())
            {
                //onStatusCode(emailUpdate.GetStatusCode());
                return;
            }

            if (!nickCreate.IsSuccess())
            {
                onCreateNickError(nickCreate.GetStatusCode());
                return;
                //onStatusCode(nickUpdate.GetStatusCode());
            }

            var successful = UI_Manager.Instance.CreatePopup<Sign_Successful_Popup>();
            successful.SetTitle("알림");
            successful.SetText("성공적으로 회원가입이 완료되었습니다.");
            
            popup.Remove();
        }
        else
        {
            onRegisterError(sign.GetStatusCode());
        }
    }

    public void Login(string pwd)
    {
        string id = SteamManager.Instance.steamID.ToString();
        var login = Backend.BMember.CustomLogin(id, pwd);

        if (!login.IsSuccess())
        {
            onLoginError(login.GetStatusCode());
        }
        else
        {
            UI_Manager.Instance.CleanUI();
            SceneManager.LoadScene("Lobby");
            UI_Manager.Instance.CreateUI<MainMenu_UI>();
            
            if (GetUserData() == null)
            {
                int level = 1;
                int exp = 0;
                int cash = 0;
                int gold = 0;

                Param param = new Param();
                param.Add("cash", cash);
                param.Add("gold", gold);
                param.Add("level", level);
                param.Add("exp", exp);

                var insert = Backend.GameData.Insert("user", param);
            }
            
            var getChannel = Backend.Chat.GetGroupChannelList("일반채널");
            if (getChannel.IsSuccess())
            {
                var channels = getChannel.Rows();
                for (int i = 0; i < channels.Count; i++)
                {
                    var count = Convert.ToInt32(channels[i]["joinedUserCount"].ToString());
                    if (count >= 200)
                    {
                        continue;
                    }
                    else
                    {
                        var serverAddress = channels[i]["serverAddress"].ToString();
                        var serverPort = ushort.Parse(channels[i]["serverPort"].ToString());
                        var inDate = channels[i]["inDate"].ToString();
                        ErrorInfo error;
                        Backend.Chat.JoinChannel(ChannelType.Public, serverAddress, serverPort, "일반채널", inDate, out error);
                        Debug.Log(error);
                    }
                }
            }
            else
            {
                Debug.Log(getChannel.GetMessage());
                Debug.Log("cant connect chat ");
            }
            
            
        }
    }

    public void FindAccount(FindAccount_Popup popup, string email)
    {
        var tempPwd = Backend.BMember.ResetPassword(SteamManager.Instance.steamID.ToString(), email);

        if (!tempPwd.IsSuccess())
        {
            onResetPwdError(tempPwd.GetStatusCode());
            //onStatusCode(tempPwd.GetStatusCode());
        }
        else
        {
            popup.Remove();
            UI_Manager.Instance.CreatePopup<ResetPassword_Popup>();
        }
    }

    public void ResetPassword(string oldPwd, string newPwd)
    {
        var resetPwd = Backend.BMember.UpdatePassword(oldPwd, newPwd);

        if (!resetPwd.IsSuccess())
        {
            onUpdatePwdError(resetPwd.GetStatusCode());
            //onResetPwdError(resetPwd.GetStatusCode());
            //onStatusCode(resetPwd.GetStatusCode());
        }
        else
        {
            
        }
    }

    public void onRegisterError(string code)
    {
        int num = Convert.ToInt32(code);
        
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        
        switch (num)
        {
            case 400:
                error.SetTitle("오류");
                error.SetText("비밀번호를 찾을 수 업습니다.\n해당 사유 : 이메일 불일치");
                break;
            case 404:
                error.SetTitle("오류");
                error.SetText("비밀번호를 찾을 수 업습니다.\n해당 사유 : 계정 혹은 이메일을 찾을 수 없음");
                break;
            case 429:
                error.SetTitle("오류");
                error.SetText("비밀번호를 찾을 수 업습니다.\n해당 사유 : 너무 많은 시도");
                break;
        }    
    }

    public void onLoginError(string code)
    {
        int num = Convert.ToInt32(code);
        
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        
        switch (num)
        {
            case 401:
                error.SetTitle("오류");
                error.SetText("현재 서버에 접속할 수 없습니다.\n해당 사유 : 차단, 과부하");
                break;
            case 403:
                error.SetTitle("오류");
                error.SetText("현재 서버에 접속할 수 없습니다.\n해당 사유 : 잘못된 ID, 비밀번호, 점검");
                break;
            
        }
    }

    public void onResetPwdError(string code)
    {
        int num = Convert.ToInt32(code);
        
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        
        switch (num)
        {
            case 401:
                error.SetTitle("오류");
                error.SetText("현재 서버에 접속할 수 없습니다.\n해당 사유 : 차단, 과부하");
                break;
            case 403:
                error.SetTitle("오류");
                error.SetText("현재 서버에 접속할 수 없습니다.\n해당 사유 : 잘못된 ID, 비밀번호, 점검");
                break;
            
        }   
    }

    public void onUpdatePwdError(string code)
    {
        int num = Convert.ToInt32(code);
        
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        
        switch (num)
        {
            case 400:
                error.SetTitle("오류");
                error.SetText("비밀번호를 변경할 수 없습니다.\n해당 사유 : 잘못된 예전 비밀번호");
                break;
        }     
    }

    public void onCreateNickError(string code)
    {
        int num = Convert.ToInt32(code);
        
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        
        switch (num)
        {
            case 400:
                error.SetTitle("오류");
                error.SetText("닉네임을 설정 할 수 없습니다.\n해당 사유 : 닉네임 확인 불가, 너무 긴 닉네임, 공백 존재");
                break;
            case 409:
                error.SetTitle("오류");
                error.SetText("닉네임을 설정 할 수 없습니다.\n해당 사유 : 닉네임 중복");
                break;            
        }     
    } 
    #endregion
    
    #region User Info
    public string GetLocalNickname()
    {
        string nick = "";
        nick = Backend.UserNickName;
        
        return nick;
    }

    private JsonData userData;
    private JsonData GetUserData()
    {
        var get = Backend.GameData.GetMyData("user", new Where(), 10); 
                
        if (get.IsSuccess() == false)
        {
            return null;
        }

        if (get.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            return null;
        }
        else
        {
            userData = get.Rows();

            return get.Rows();
        }

        return null;
    }
    
    private JsonData GetUserData(string key)
    {
        var get = Backend.GameData.GetMyData("user", new Where(), 10); 
                
        Debug.Log(get.GetMessage());
        
        if (get.IsSuccess() == false)
        {
            return null;
        }


        var rows = get.Rows()[0];

        Debug.Log("test");
            
        return rows[key][0];
    }
    
    public int GetGold()
    {
        int gold = 0;
        gold = Convert.ToInt32(GetUserData("gold").ToString());
 
        return gold;
    }
    
    public int GetCash()
    {
        int cash = 0;
        cash = Convert.ToInt32(GetUserData("cash").ToString());
 
        return cash;
    }
    
    public int GetLevel()
    {
        int level = 0;
        level = Convert.ToInt32(GetUserData("level").ToString());
 
        return level;
    }
    
    public int GetEXP()
    {
        int exp = 0;
        exp = Convert.ToInt32(GetUserData("exp").ToString());
 
        return exp;
    }
    #endregion
    
    #region Report

    #endregion
    
    #region Handler

    private void Handler()
    {

    }
    #endregion
    
    #region Unity General Funcs
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
    }

    private void Start()
    {
        BackendInit();
    }

    private void Update()
    {
        
        Backend.AsyncPoll();
    }
    #endregion
}
