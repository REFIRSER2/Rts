using System.Collections;
using RF.UI.Popup;
using UnityEngine;

namespace RF.UI.Connecting
{
    public class Connecting_UI : UI_Base
    {
        #region 일반 내장 함수

        private void Awake()
        {
            if (model == null)
            {
                model = this.GetComponent<Connecting_UI_Model>();
            }   
        }
        private void Start()
        {
            UI_Manager.Instance.AddUIView("Connecting_UI", this);
        }
        #endregion
        
        #region 프레젠터
        private Connecting_UI_Model model;

        public Connecting_UI_Model GetModel()
        {
            return model;
        }
        public void OnTimeOut()
        {
            var error = UI_Manager.Instance.CreatePopupView("Error_Popup") as Error_Popup;
            error.SetTitle("오류");
            error.SetMain("서버와 연결을 할 수 없어 게임을 종료합니다");
            
            error.GetAcceptButton().onClick.AddListener(() =>
            {
                UI_Manager.Instance.RemovePopup(error);
                Application.Quit();
            });
            
            error.GetDeclineButton().onClick.AddListener(() =>
            {
                UI_Manager.Instance.RemovePopup(error);
                Application.Quit();
            });
        }

        public void OnConnected()
        {
            UI_Manager.Instance.SetActiveUI("Connecting_UI", false);
            UI_Manager.Instance.SetActiveUI("First_UI", true);
        }
        #endregion
        
        #region 타임아웃

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
