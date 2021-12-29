using System;
using System.Collections;
using RF.Lobby;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace RF.UI.Popup
{
    public class MatchAccept_Popup : UI_Popup_Base
    {
        #region 기본 내장 함수
        private void Start()
        {
            StartCoroutine("AutoCancelTimer");
        }

        private void Update()
        {
            if (waitTimer < waitTime)
            {
                waitTimer += Time.deltaTime;

                waitTimer = Mathf.Min(waitTimer, waitTime);
                
                progress.fillAmount = (waitTime - waitTimer)/waitTime;
            }
        }
        #endregion
        
        #region 엘리먼트
        [SerializeField] private GameObject acceptDisableButton;
        [SerializeField] private GameObject declineDisableButton;

        [SerializeField] private Image progress;
        #endregion
        
        #region 대기 시간
        private float waitTime = 10F;
        private float waitTimer = 0F;
        #endregion
        
        #region 로비
        private string lobbyID;

        public void SetLobby(string id)
        {
            lobbyID = id;
        }
        #endregion

        #region 타이머
        IEnumerator AutoCancelTimer()
        {
            yield return new WaitForSeconds(waitTime);
            UI_Manager.Instance.RemovePopup(this);
            LobbyManager.Instance.CancelQuickMatch();
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
            //base.OnAccept();
            
            LobbyManager.Instance.AcceptQuickMatch(lobbyID);
            
            Debug.Log("on accept");
            
            GetAcceptButton().gameObject.SetActive(false);
            acceptDisableButton.SetActive(true);
        }

        public override void OnDecline()
        {
            //base.OnDecline();

            GetDeclineButton().gameObject.SetActive(false);
            declineDisableButton.SetActive(true);
            
            LobbyManager.Instance.CancelQuickMatch();
        }

        #endregion
    }
}
