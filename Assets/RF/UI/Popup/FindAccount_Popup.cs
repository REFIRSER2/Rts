using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindAccount_Popup : Popup_Base
{
    [SerializeField] private InputField emailInput;
    
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
        UI_Manager.Instance.RemovePopup(EnumData.PopupType.FindAccount);
    }

    public void onFind()
    {
        if (emailInput.text == "")
        {
            return;
        }
        DatabaseManager.Instance.FindAccount(emailInput.text);
    }
}
