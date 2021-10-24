using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_UI : UI_Base
{
    [SerializeField] private Text notice_Text;
    public void SetNotice(string str)
    {
        notice_Text.text = str;
    }
}
