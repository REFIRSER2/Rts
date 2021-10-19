using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{
    #region Singletone
    public static DatabaseManager Instance;
    #endregion
    
    #region Database

    private void DataBaseInit()
    {
        UI_Manager.Instance.CreateUI(EnumData.UIType.First);
        
        var backend = Backend.Initialize(true);
        if (backend.IsSuccess())
        {
            Debug.Log("초기화 성공");
        }
        else
        {
            Application.Quit();
        }
    }

    public void Sign(string nickname, string email, string pwd)
    {
        string id = SteamManager.Instance.steamID.ToString();

        var sign = Backend.BMember.CustomSignUp(id, pwd);
        if (sign.IsSuccess())
        {
            Debug.Log("Signup Successful");
            var emailUpdate= Backend.BMember.UpdateCustomEmail(email);
            var nickUpdate = Backend.BMember.UpdateNickname(nickname);
            
            if (!emailUpdate.IsSuccess())
            {
                
            }

            if (!nickUpdate.IsSuccess())
            {
                
            }
        }
        else
        {
            
        }
    }

    public void Login(string pwd)
    {
        string id = SteamManager.Instance.steamID.ToString();
        var login = Backend.BMember.CustomLogin(id, pwd);

        if (!login.IsSuccess())
        {
            
        }
        else
        {
            UI_Manager.Instance.RemoveAllUI();
            SceneManager.LoadScene("Lobby");
        }
    }

    public void FindAccount(string email)
    {
        var tempPwd = Backend.BMember.ResetPassword(SteamManager.Instance.steamID.ToString(), email);

        if (!tempPwd.IsSuccess())
        {
            

        }
        else
        {
            UI_Manager.Instance.RemovePopup(EnumData.PopupType.FindAccount);
        }
    }

    public void ResetPassword(string oldPwd, string newPwd)
    {
        var resetPwd = Backend.BMember.UpdatePassword(oldPwd, newPwd);

        if (!resetPwd.IsSuccess())
        {
            
        }
        else
        {
            
        }
    }

    public bool IsValidAccount()
    {
        var check = Backend.BMember.FindCustomID(SteamManager.Instance.steamID.ToString());
        Debug.Log(check);
        return false;
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
        DataBaseInit();
    }

    private void Update()
    {
        Backend.AsyncPoll();
    }
    #endregion
}
