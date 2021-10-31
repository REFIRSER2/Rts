using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;

public class PartyInvite_Popup : Popup_Base
{
    private Lobby lobby;
    
    public void SetLobby(Lobby lb)
    {
        lobby = lb;
    }
    
    public void onAccept()
    {
        SteamManager.Instance.JoinLobby(lobby);
        UI_Manager.Instance.RemovePopup(this);
    }
    
    public void onCancel()
    {
        UI_Manager.Instance.RemovePopup(this);
    }
}
