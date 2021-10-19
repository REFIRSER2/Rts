using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetPassword_Popup : Popup_Base
{
    [SerializeField] private InputField newPwdInput;
    [SerializeField] private InputField oldPwdInput;
    
    public override void On_Open()
    {
        base.On_Open();
    }

    public override void On_Close()
    {
        base.On_Close();
    }
    
    public void onClose()
    {
        UI_Manager.Instance.RemovePopup(EnumData.PopupType.ResetPassword);
    }

    public void onReset()
    {
        if (oldPwdInput.text == "" || newPwdInput.text == "")
        {
            
            return;
        }
        DatabaseManager.Instance.ResetPassword(oldPwdInput.text, newPwdInput.text);
    }
}
