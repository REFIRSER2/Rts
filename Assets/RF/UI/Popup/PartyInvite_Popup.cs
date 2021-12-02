using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.UI;

public class PartyInvite_Popup : Popup_Base
{
    [SerializeField] private Text title_Text;
    [SerializeField] private Text main_Text;

    private Lobby lobby;
    
    public void SetTitle(string str)
    {
        title_Text.text = str;
    }

    public void SetText(string str)
    {
        main_Text.text = str;
    }

    public void SetLobby(Lobby lb)
    {
        lobby = lb;
    }

    public void onAccept()
    {
        SteamManager.Instance.JoinLobby(lobby.Id);
    }

    public void onCancel()
    {
        UI_Manager.Instance.RemovePopup(this);
    }
}
