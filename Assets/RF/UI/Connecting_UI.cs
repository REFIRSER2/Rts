using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connecting_UI : UI_Base
{
    private void Awake()
    {
        StartCoroutine(CheckConnecting());
    }

    public override void On_Open()
    {
        base.On_Open();
    }

    public override void On_Close()
    {
        base.On_Close();
    }

    IEnumerator CheckConnecting()
    {
        yield return new WaitForSeconds(15F);

        Connect_Error_Popup error = UI_Manager.Instance.CreatePopup<Connect_Error_Popup>();
        error.SetTitle("연결 오류");
        error.SetText("서버와 연결할 수 없습니다\n아래의 닫기버튼을 누르면 게임이 종료됩니다");
    }
}
