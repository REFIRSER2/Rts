using System;
using UnityEngine;

namespace RF.UI.MainMenu
{
    public class MainMenu_UI_Model : MonoBehaviour
    {
        #region 프레젠터
        private MainMenu_UI presenter;
        

        #endregion
        
        #region 기본 내장 함수

        private void Awake()
        {
            if (presenter == null)
            {
                presenter = this.GetComponent<MainMenu_UI>();
            }
        }

        private void Start()
        {
            
        }

        #endregion
    }
}
