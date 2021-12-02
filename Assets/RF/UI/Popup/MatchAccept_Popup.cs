using System;
using System.Collections;
using System.Collections.Generic;
using RF.Photon;
using Steamworks.Data;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class MatchAccept_Popup : Popup_Base
{
    #region 변수
    [SerializeField] private GameObject acceptBtn;
    [SerializeField] private GameObject cancelBtn;
    [SerializeField] private GameObject acceptBtn_Disable;
    [SerializeField] private GameObject cancelBtn_Disable;
    
    [SerializeField] private Image progress;
    [SerializeField] private Image progress2;
    
    private float timer = 0F;
    
    private Lobby lobby;
    
    private bool isReady = false;
    #endregion
    
    #region 클릭 이벤트
    public void onAccept()
    {
        acceptBtn.SetActive(false);
        cancelBtn.SetActive(false);
        acceptBtn_Disable.SetActive(true);
        cancelBtn_Disable.SetActive(true);
        
        
        LobbyManager.Instance.acceptQuickMatchAction = () =>
        {
            isReady = true;
        };
        
        LobbyManager.Instance.AcceptQuickMatch();
    }

    public void onCancel()
    {
        //LobbyManager.Instance.CancelQuickMatch();
        UI_Manager.Instance.RemovePopup(this);  
        /*if (PhotonManager.Instance.GetRoom() != null)
        {
            PhotonManager.Instance.LeaveRoom();
        }*/
    }
    #endregion

    /*public void SetLobby(Lobby lb)
    {
        lobby = lb;
        StartCoroutine(AutoCancel());
    }*/

    #region 유니티 기본 내장 함수
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < 10F)
        {
            progress.fillAmount = 1 - (((timer)/10F));
            progress2.fillAmount = 1 - (((timer)/10F));
        }
    }
    #endregion

    #region 자동 취소
    IEnumerator AutoCancel()
    {
        yield return new WaitForSeconds(10F);
        if (!isReady)
        {
            /*if (PhotonManager.Instance.GetRoom() != null)
            {
                PhotonManager.Instance.LeaveRoom();
            }*/
            LobbyManager.Instance.CancelQuickMatch();
        }
        UI_Manager.Instance.RemovePopup(this);   
    }
    #endregion
}
