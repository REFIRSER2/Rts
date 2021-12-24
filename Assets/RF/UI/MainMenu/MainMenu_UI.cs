using System;
using RF.Account;
using RF.Steam;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace RF.UI.MainMenu
{
    public class MainMenu_UI : UI_Base
    {
        #region 유니티 기본 내장 함수
        private void Start()
        {
            model = this.GetComponent<MainMenu_UI_Model>();
            
            UI_Manager.Instance.AddUIView("MainMenu_UI", this);
            GetModel().OnRefreshProfile();
            GetModel().OnRefreshGoods();

        }
        #endregion
        
        #region 모델

        private MainMenu_UI_Model model;

        public MainMenu_UI_Model GetModel()
        {
            return model;
        }
        #endregion
        
        #region 프레젠터
        [SerializeField] private RawImage profileIcon;
        [SerializeField] private Text goldText;
        [SerializeField] private Text cashText;

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

        public void OnRefreshParty()
        {
            
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
