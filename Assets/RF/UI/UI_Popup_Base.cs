using System;
using TMPro;
using UnityEngine;

namespace RF.UI
{
    public class UI_Popup_Base : MonoBehaviour
    {
        #region 엘리먼트
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text mainText;
        [SerializeField] private CustomButton acceptButton;
        [SerializeField] private CustomButton declineButton;

        public void SetTitle(string text)
        {
            titleText.text = text;
        }

        public void SetMain(string text)
        {
            mainText.text = text;
        }

        public CustomButton GetAcceptButton()
        {
            return acceptButton;
        }

        public CustomButton GetDeclineButton()
        {
            return declineButton;
        }
        
        public virtual void OnAccept(){
            
            UI_Manager.Instance.RemovePopup(this);
        }

        public virtual void OnDecline()
        {
            UI_Manager.Instance.RemovePopup(this);
        }
        #endregion

        #region 유니티 기본 내장 함수

        private void Awake()
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
