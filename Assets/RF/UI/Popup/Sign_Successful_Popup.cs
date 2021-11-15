using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign_Successful_Popup : Popup_Base
{
    [SerializeField] private Text title_Text;
    [SerializeField] private Text main_Text;

    private Sign_Popup signPopup;
    
    public void SetTitle(string str)
    {
        title_Text.text = str;
    }

    public void SetText(string str)
    {
        main_Text.text = str;
    }

    public void SetPopup(Sign_Popup popup)
    {
        signPopup = popup;
    }

    public void onClose()
    {
        this.Remove();
        if (signPopup != null)
        {
            UI_Manager.Instance.RemovePopup(signPopup);
        }
    }
}
