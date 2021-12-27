using RF.Lobby;
using UnityEngine;

namespace RF.UI.Popup
{
    public class PartyInvited_Popup : UI_Popup_Base
    {
        #region 파티

        private int partyID = -1;
        
        public void SetParty(int id)
        {
            partyID = id;
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
            base.OnAccept();
            
            LobbyManager.Instance.JoinParty(LobbyManager.Instance.party.id, partyID);
        }

        public override void OnDecline()
        {
            base.OnDecline();

            if (LobbyManager.Instance.invited_Popups.Count > 0)
            {
                PartyInvited_Popup popup = LobbyManager.Instance.invited_Popups.Dequeue();
                popup.gameObject.SetActive(true);
            }
        }

        #endregion
    }
}
