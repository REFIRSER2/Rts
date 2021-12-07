using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sign_Popup : Popup_Base
{
    #region 변수
    [SerializeField] private List<InputField> inputField_List;
    
    [SerializeField] private InputField nickInput;
    [SerializeField] private InputField emailInput;
    [SerializeField] private InputField pwdInput;

    private Action<int> signAction;
    #endregion
    
    #region 오픈/클로즈 오버라이드
    public override void On_Open()
    {
        base.On_Open();

        signAction = onSign;
    }

    public override void On_Close()
    {
        base.On_Close();
    }
    #endregion

    #region 클릭 이벤트
    public void onApply()
    {
        if (emailInput.text != "" && pwdInput.text != "")
        {
            MainManager.Instance.Sign(nickInput.text, pwdInput.text, emailInput.text, signAction);  
        }
        else
        {
            
        }
    }
    #endregion

    #region 회원가입 액션
    public void onSign(int code)
    {
        Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
        switch (code)
        {
            default:
                error.SetTitle("오류");
                error.SetText("알 수 없는 오류로 회원가입 할 수 없습니다");
                break;
            case 0:
                UI_Manager.Instance.RemovePopup(error);
                
                Sign_Successful_Popup successful = UI_Manager.Instance.CreatePopup<Sign_Successful_Popup>();
                successful.SetTitle("알림");
                successful.SetText("회원가입을 성공적으로 마쳤습니다");
                
                UI_Manager.Instance.CleanUI();
                UI_Manager.Instance.CreateUI<MainMenu_UI>();
                SceneManager.LoadScene("Lobby");

                LobbyManager.Instance.CreateParty(SteamManager.Instance.steamID);
                MainManager.Instance.GetProfile();
                break;
            case 1:
                error.SetTitle("오류");
                error.SetText("이메일을 형식에 맞게 입력해주세요");
                break;
            case 2:
                error.SetTitle("오류");
                error.SetText("이미 계정이 존재합니다");
                break;
            case 3:
                error.SetTitle("오류");
                error.SetText("DB 서버에 접근 할 수 없습니다.");
                break;
        }
    }
    #endregion

    #region 유니티 기본 내장 함수
    private void Update()
    {
        int index = -1;

        bool check = false;
        for (int i = 0; i < inputField_List.Count; i++)
        {
            if (inputField_List[i].isFocused)
            {
                index = i;
                check = true;
                break;
            }
        }

        if (check && Input.GetKeyDown(KeyCode.Tab))
            {
                if (index < inputField_List.Count - 1)
                {
                    index++;
                    inputField_List[index].Select();
                }
                else
                {
                    index = 0;
                    inputField_List[index].Select();
                }
            }
    }
    #endregion
}
