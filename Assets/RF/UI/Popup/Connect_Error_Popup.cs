using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect_Error_Popup : Error_Popup
{
    public override void onClose()
    {
        base.onClose();
        Application.Quit();
    }
}
