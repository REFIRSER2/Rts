using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.UI;

public class PartyInvite_Popup : Popup_Base
{
    #region 변수
    [SerializeField] private Text title_Text;
    [SerializeField] private Text main_Text;
    #endregion
    
   #region UI 정보
    public void SetTitle(string str)
    {
        title_Text.text = str;
    }

    public void SetText(string str)
    {
        main_Text.text = str;
    }
    #endregion

    #region 클릭 이벤트
    public void onCancel()
    {
        UI_Manager.Instance.RemovePopup(this);
    }
    #endregion
}
