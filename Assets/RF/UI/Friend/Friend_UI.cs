using System;
using System.Collections.Generic;
using RF.Lobby;
using RF.Steam;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace RF.UI.Friend
{
    public class Friend_UI : UI_Base
    {

        #region 기본 내장 함수
        private void Start()
        {
            UI_Manager.Instance.AddUIView("Friend_UI", this);

            if (model == null)
            {
                model = this.GetComponent<Friend_UI_Model>();
            }
        }
        #endregion
        
        #region 모델
        private Friend_UI_Model model;

        public Friend_UI_Model GetModel()
        {
            return model;
        }
        #endregion
        
        #region 프레젠터
        [SerializeField] private List<RawImage> partyIcons = new List<RawImage>();

        public async void OnPartyRefresh(Party party)
        {
            var index = 0;
            foreach (var data in party.memberList)
            {
                if (partyIcons[index] == null)
                {
                    continue;
                }

                Debug.Log(data.steamID);
                var image = await SteamFriends.GetLargeAvatarAsync(Convert.ToUInt64(data.steamID));
                if (image != null)
                {
                    var texture = SteamManager.Instance.GetProfileIcon(image.Value);
                    partyIcons[index].texture = texture;
                    partyIcons[index].gameObject.SetActive(true);
                }
                
                index++;
            }
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

        #endregion
    }
}
