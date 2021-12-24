using System;
using UnityEngine;

namespace RF.UI.Login
{
    public class Login_UI_Model : MonoBehaviour
    {
        #region 프레젠터
        private Login_UI presenter;
        #endregion
        
        #region 이벤트
        public void OnTryLogin()
        {
            presenter.OnTryLogin();
        }

        public void OnLogin(int code)
        {
            presenter.OnLogin(code);
        }

        public void OnTrySign()
        {
            presenter.OnTrySign();
        }

        public void OnSign(int code)
        {
            presenter.OnSign(code);
        }

        public void OnTryFind()
        {
            
        }
        #endregion
        
        #region 유니티 기본 내장 함수

        private void Awake()
        {
            if (presenter == null)
            {
                presenter = this.GetComponent<Login_UI>();
            }
        }

        private void Start()
        {
            
        }

        #endregion
    }
}
