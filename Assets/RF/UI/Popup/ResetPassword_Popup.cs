using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetPassword_Popup : Popup_Base
{
    [SerializeField] private TMP_InputField newPwdInput;
    [SerializeField] private TMP_InputField oldPwdInput;
    
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
        this.Remove();
    }

    public void onReset()
    {
        if (oldPwdInput.text == "" || newPwdInput.text == "")
        {
            
            return;
        }
        BackendManager.Instance.ResetPassword(oldPwdInput.text, newPwdInput.text);
    }
}
