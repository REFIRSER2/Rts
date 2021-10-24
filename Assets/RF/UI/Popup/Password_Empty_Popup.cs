using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Password_Empty_Popup : Popup_Base
{
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
}
