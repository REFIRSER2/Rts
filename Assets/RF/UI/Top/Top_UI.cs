using System;
using RF.Account;
using RF.Lobby;
using RF.Steam;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RF.UI.Top
{
    public class Top_UI : UI_Base
    {
        #region 기본 내장 함수

        private void Start()
        {
            UI_Manager.Instance.AddUIView("Top_UI", this);

            if (model == null)
            {
                model = this.GetComponent<Top_UI_Model>();
            }
            
            GetModel().OnRefreshGoods();
            GetModel().OnRefreshProfile();
        }
        #endregion
        
        #region 모델
        private Top_UI_Model model;

        public Top_UI_Model GetModel()
        {
            return model;
        }
        #endregion
        
        #region 프레젠터
        [SerializeField] private RawImage profileIcon;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text cashText;

        [SerializeField] private CustomButton playButton;
        [SerializeField] private CustomButton leaveButton;
        
        public async void OnRefreshProfile()
        {
            var image = await SteamFriends.GetLargeAvatarAsync(Convert.ToUInt64(SteamManager.Instance.GetAccount().steamID));
            if (image != null)
            {
                var texture = SteamManager.Instance.GetProfileIcon(image.Value);
                profileIcon.texture = texture;
                profileIcon.gameObject.SetActive(true);
            }
        }
        
        public void OnRefreshGoods()
        {
            goldText.text = AccountManager.Instance.user.gold.ToString();
            cashText.text = AccountManager.Instance.user.cash.ToString();
        }
        
        public void OnTryPlay()
        {
            switch (LobbyManager.Instance.GetGamemode())
            {
                case 0:
                    LobbyManager.Instance.FindQuickMatch();
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public void OnTryLeave()
        {
            switch (LobbyManager.Instance.GetGamemode())
            {
                case 0:
                    LobbyManager.Instance.LeaveQuickMatch();
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public void OnSelectMode(int mode)
        {
            LobbyManager.Instance.SetGamemode(mode);
        }

        public void OnStartFindQuickMatch()
        {
            playButton.gameObject.SetActive(false);
            leaveButton.gameObject.SetActive(true);    
        }

        public void OnLeaveFindQuickMatch()
        {
            leaveButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
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
