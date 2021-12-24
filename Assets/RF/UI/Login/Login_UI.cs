using RF.Account;
using RF.UI.Popup;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RF.UI.Login
{
    public class Login_UI : UI_Base
    {
        #region 기본 내장 함수

        private void Awake()
        {
            if (model == null)
            {
                model = this.GetComponent<Login_UI_Model>();
            }
        }
        
        private void Start()
        {
            UI_Manager.Instance.AddUIView("Login_UI", this);
            UI_Manager.Instance.SetActiveUI("Login_UI", false);
        }
        #endregion
        
        #region 프레젠터
        private Login_UI_Model model;
        
        [SerializeField] private InputField pwdInput;

        public Login_UI_Model GetModel()
        {
            return model;
        }

        public void OnTryLogin()
        {
            AccountManager.Instance.Login(pwdInput.text);
        }

        public void OnLogin(int code)
        {
            Error_Popup error;
            switch (code)
            {
                default:
                    
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
                    error.SetTitle("오류");
                    error.SetMain("알 수 없는 오류로 로그인 할 수 없습니다");
                    break;
                case 0:
                    

                
                    //
                    //UI_Manager.Instance.CreateUI<MainMenu_UI>();
                    SceneManager.LoadScene("Lobby");
                    
                    AccountManager.Instance.GetProfile();
                    AccountManager.Instance.GetInventory();

                    //LobbyManager.Instance.CreateParty(SteamManager.Instance.steamID);
                    //MainManager.Instance.GetProfile();
                    break;
                case 1:
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
                    error.SetTitle("오류");
                    error.SetMain("DB에서 계정을 찾을 수 없습니다");
                    break;
                case 2:
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;;
                    error.SetTitle("오류");
                    error.SetMain("비밀번호가 일치하지 않습니다");
                    break;
                case 3:
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
                    error.SetTitle("오류");
                    error.SetMain("DB 서버에 접근 할 수 없습니다.");
                    break;
            }
            
        }

        public void OnTrySign()
        {
            var popup = UI_Manager.Instance.CreatePopupView("Sign_Popup") as Sign_Popup;
        }

        public void OnSign(int code)
        {
            Error_Popup error;
            switch (code)
            {
                default:
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
                    error.SetTitle("오류");
                    error.SetMain("알 수 없는 오류로 회원가입 할 수 없습니다");
                    break;
                case 0:
                    Sign_Successful_Popup successful = UI_Manager.Instance.CreatePopupView("Sign_Successful_Popup") as Sign_Successful_Popup;
                    successful.SetTitle("알림");
                    successful.SetMain("회원가입을 성공적으로 마쳤습니다");
 
                    SceneManager.LoadScene("Lobby");
                    
                    AccountManager.Instance.GetProfile();
                    AccountManager.Instance.GetInventory();
                    break;
                case 1:
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
                    error.SetTitle("오류");
                    error.SetMain("이메일을 형식에 맞게 입력해주세요");
                    break;
                case 2:
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
                    error.SetTitle("오류");
                    error.SetMain("이미 계정이 존재합니다");
                    break;
                case 3:
                    error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
                    error.SetTitle("오류");
                    error.SetMain("DB 서버에 접근 할 수 없습니다.");
                    break;
            }                
        }
        #endregion
        
        #region 오버라이드

        public override void On_Open()
        {
            base.On_Open();
        }

        public override void On_Close()
        {
            base.On_Close();
        }

        public override void On_Refresh()
        {
            base.On_Refresh();
        }

        #endregion
    }
}
