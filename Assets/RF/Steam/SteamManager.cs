using System;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using Color = UnityEngine.Color;

namespace RF.Steam
{
    public class SteamAccount
    {
        public SteamId steamID;
    }
    
    public class SteamManager : MonoBehaviour
    {
        #region 싱글톤
        public static SteamManager Instance;
        #endregion

        #region 유니티 기본 내장 함수
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

            SetupClient();
        }
        #endregion
        
        #region 스팀 초기화

        private void SetupClient()
        {
            try
            {
                SteamClient.Init(480);
            }
            catch (SystemException e)
            {
                Application.Quit();
            }
            
            SetupAccount();
        }
        #endregion
        
        #region 스팀 계정
        private SteamAccount account = new SteamAccount();

        private void SetupAccount()
        {
            account.steamID = SteamClient.SteamId;
        }

        public SteamAccount GetAccount()
        {
            return account;
        }
        #endregion
        
        #region 프로필
        public Texture2D GetProfileIcon(Image image)
        {
            var texture = new Texture2D((int)image.Width, (int)image.Height);

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var p = image.GetPixel(x, y);
                    texture.SetPixel(x, (int)image.Height - y, new Color(p.r/255.0F, p.g/255.0F, p.b/255.0F, p.a/255.0F));
                }
            }
            texture.Apply();

            return texture;
        }
        #endregion
    }
}
