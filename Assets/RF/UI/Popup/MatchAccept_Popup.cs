using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class MatchAccept_Popup : Popup_Base
{
    [SerializeField] private GameObject acceptBtn;
    [SerializeField] private GameObject cancelBtn;
    [SerializeField] private GameObject acceptBtn_Disable;
    [SerializeField] private GameObject cancelBtn_Disable;
    
    [SerializeField] private Image progress;
    [SerializeField] private Image progress2;
    
    private float timer = 0F;
    
    private Lobby lobby;
    
    private bool isReady = false;
    
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
        LobbyManager.Instance.CancelQuickMatch();
        UI_Manager.Instance.RemovePopup(this);  
    }

    public void SetLobby(Lobby lb)
    {
        lobby = lb;
        StartCoroutine(AutoCancel());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < 10F)
        {
            progress.fillAmount = 1 - (((timer)/10F));
            progress2.fillAmount = 1 - (((timer)/10F));
        }
    }

    IEnumerator AutoCancel()
    {
        yield return new WaitForSeconds(10F);
        if (!isReady)
        {
            LobbyManager.Instance.CancelQuickMatch();
        }
        UI_Manager.Instance.RemovePopup(this);   
    }
}
