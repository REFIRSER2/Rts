using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;

public class MatchAccept_Popup : Popup_Base
{
    [SerializeField] private GameObject acceptBtn;
    [SerializeField] private GameObject cancelBtn;

    private Lobby lobby;
    
    public void onAccept()
    {
        acceptBtn.SetActive(false);
        cancelBtn.SetActive(false);

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
    }
}
