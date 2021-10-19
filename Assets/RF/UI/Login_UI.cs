using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_UI : UI_Base
{
    [SerializeField] private GameObject loginGroup;

    [SerializeField] private InputField pwdInput;
    
    public void onSign_Click()
    {
        UI_Manager.Instance.CreatePopup(EnumData.PopupType.Sign);
        //DatabaseManager.Instance.Sign();
    }

    public void onLogin_Click()
    {
        if (pwdInput.text == "")
        {
            
            return;
        }
        DatabaseManager.Instance.Login(pwdInput.text);
    }

    public void onFind_Click()
    {
        UI_Manager.Instance.CreatePopup(EnumData.PopupType.FindAccount);
    }
}
