using System;
using UnityEngine;

namespace RF.UI
{
    public class UI_Base : MonoBehaviour
    {
        #region 유니티 기본 내장 함수
        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            On_Open();
        }

        private void OnDisable()
        {
            On_Close();
        }
        #endregion

        #region 가상 함수
        public virtual void On_Open()
        {
            
        }

        public virtual void On_Close()
        {
            
        }

        public virtual void On_Refresh()
        {
            
        }
        #endregion
    }
}
