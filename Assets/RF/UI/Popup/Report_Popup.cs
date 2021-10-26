using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Report_Popup : Popup_Base
{
    [SerializeField] private TMP_InputField nickInput;
    
    [SerializeField] private List<Toggle> toggleList;
    [SerializeField] private List<Text> reasonList; 
    [SerializeField] private TMP_InputField detailInput;

    public void onClose()
    {
        UI_Manager.Instance.RemovePopup(this);
    }
    
    public void onReport()
    {
        string reason = "";
        int index = 0;
        
        foreach (var check in toggleList)
        {
            if (check.isOn)
            {
                reason += reasonList[index].text + " | ";
            }
            index++;
        }

        var report = Backend.Chat.ReportUser(nickInput.text, reason, detailInput.text);
        {
            UI_Manager.Instance.RemovePopup(this);
            if (report.IsSuccess())
            {
                
            }
            else
            {
                onReportError(report.GetStatusCode());
            }
            
            
        }
    }

    public void onReportError(string code)
    {
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        
        int num = Convert.ToInt32(code);
        switch (num)
        {
            case 404:
                error.SetTitle("오류");
                error.SetText("신고할 수 없는 상태입니다.\n해당 사유 : 해당 닉네임 확인이 불가능합니다.");
                break;
            case 400:
                error.SetTitle("오류");
                error.SetText("신고할 수 없는 상태입니다.\n해당 사유 : 세부 사항이 너무 길거나 너무 적습니다.");
                break;
        }
    }
}
