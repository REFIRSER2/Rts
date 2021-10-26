using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_Popup : Popup_Base
{
    public void onExit()
    {
        Application.Quit();
    }
    
    public void onClose()
    {
        UI_Manager.Instance.RemovePopup(this);
    }
}
