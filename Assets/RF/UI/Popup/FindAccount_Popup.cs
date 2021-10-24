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
        this.Remove();
    }

    public void onFind()
    {
        if (emailInput.text == "")
        {
            return;
        }
        BackendManager.Instance.FindAccount(this, emailInput.text);
    }
}
