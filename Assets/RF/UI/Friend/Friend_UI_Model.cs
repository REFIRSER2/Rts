using System;
using RF.Lobby;
using UnityEngine;

namespace RF.UI.Friend
{
    public class Friend_UI_Model : MonoBehaviour
    {
        #region 프레젠터
        private Friend_UI presenter;

        public void OnPartyRefresh(Party party)
        {
            presenter.OnPartyRefresh(party);
        }
        #endregion
        
        #region 기본 내장 함수

        private void Awake()
        {
            if (presenter == null)
            {
                presenter = this.GetComponent<Friend_UI>();
            }
        }

        private void Start()
        {
            
        }

        #endregion
    }
}
