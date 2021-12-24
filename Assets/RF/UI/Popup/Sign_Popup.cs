using System;
using RF.Account;
using UnityEngine;
using UnityEngine.UI;

namespace RF.UI.Popup
{
    public class Sign_Popup : UI_Popup_Base
    {
        #region 기본 내장 함수

        private void Start()
        {

        }

        #endregion
        
        #region 엘리먼트
        [SerializeField] private InputField nickNameInput;
        [SerializeField] private InputField emailInput;
        [SerializeField] private InputField pwdInput;

        public string GetNickname()
        {
            return nickNameInput.text;
        }

        public string GetEmail()
        {
            return emailInput.text;
        }

        public string GetPwd()
        {
            return pwdInput.text;
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

        public override void OnAccept()
        {
            base.OnAccept();
            
            AccountManager.Instance.Sign(GetNickname(), GetEmail(), GetPwd());
        }

        public override void OnDecline()
        {
            base.OnDecline();
        }

        #endregion
    }
}
