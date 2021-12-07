using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.UI;

public class PartyInvited_Popup : Popup_Base
{
    #region 변수
    [SerializeField] private Text title_Text;
    [SerializeField] private Text main_Text;

    private Lobby lobby;
    #endregion
    
    #region UI 정보 설정
    public void SetLobby(Lobby lb)
    {
        lobby = lb;
    }
    
    private int partyID = -1;
    public void SetPartyID(int id)
    {
        partyID = id;
    }
    
    public void SetTitle(string str)
    {
        title_Text.text = str;
    }

    public void SetText(string str)
    {
        main_Text.text = str;
    }
    #endregion
    
    #region 클릭 이벤트
    public void onAccept()
    {
        UI_Manager.Instance.RemovePopup(this);
        
        SteamManager.Instance.JoinLobby(lobby);
        /* Debug.Log(partyID);
         if (partyID == -1)
         {
             return;
         }
 
         foreach (var item in LobbyManager.Instance.party_Popups)
         {
             UI_Manager.Instance.RemovePopup(item);
         }
         
         LobbyManager.Instance.JoinParty(LobbyManager.Instance.GetPartyID(),partyID);*/
        
    }
    
    public void onCancel()
    {
        UI_Manager.Instance.RemovePopup(this);
        
        /*(LobbyManager.Instance.party_Popups.Count > 0)
        {
            PartyInvited_Popup popup = LobbyManager.Instance.party_Popups.Dequeue();
            popup.gameObject.SetActive(true);
        }*/
    }
    #endregion
}
