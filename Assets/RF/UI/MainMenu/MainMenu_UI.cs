using System;
using RF.Account;
using RF.Steam;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace RF.UI.MainMenu
{
    public class MainMenu_UI : UI_Base
    {
        #region 유니티 기본 내장 함수
        private void Start()
        {
            model = this.GetComponent<MainMenu_UI_Model>();
            
            UI_Manager.Instance.AddUIView("MainMenu_UI", this);


        }
        #endregion
        
        #region 모델

        private MainMenu_UI_Model model;

        public MainMenu_UI_Model GetModel()
        {
            return model;
        }
        #endregion
        
        #region 프레젠터

        public void OnTryPlay()
        {
            
        }

        public void OnTryLeave()
        {
            
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
