using System;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SocketIO3;
using Newtonsoft.Json;
using RF.Steam;
using RF.UI;
using RF.UI.Connecting;
using RF.UI.Login;
using RF.UI.MainMenu;
using RF.UI.Top;
using UnityEngine;

namespace RF.Account
{
    public class User
    {
        public string nickName = "";
        public int rank = 0;
        public int rankPoint = 0;
        public int mmr = 0;

        public int gold = 0;
        public int cash = 0;
    }
    
    public class AccountManager : MonoBehaviour
    {
        #region 싱글톤
        public static AccountManager Instance;
        #endregion
        
        #region 서버
        private SocketManager server;

        private void Setup()
        {
            server = new SocketManager(new Uri("http://54.180.191.145:27000"));
            server.Open();
            
            //var connectingUI = UI_Manager.Instance.CreateUI<Connecting_UI>();
            server.Socket.On("connect", () =>
            {
                Debug.Log("Connected");
                var ui = UI_Manager.Instance.GetUIView("Connecting_UI") as Connecting_UI;
                ui.GetModel().OnConnected();
                //connectingUI.OnConnected();
            });

            
            // UI_Manager.Instance.CreateUI<>();
        }
        #endregion
        
        #region 회원가입
        public void Setup_Sign()
        {
            server.Socket.On<int>("sign", (code) =>
            {
                OnSign(code);
            });    
        }

        public void Sign(string nickname, string email, string pwd)
        {
            server.Socket.Emit("sign", SteamManager.Instance.GetAccount().steamID.ToString(), nickname, pwd, email);
        }
        
        public void OnSign(int code)
        {
            var ui = UI_Manager.Instance.GetUIView("Login_UI") as Login_UI;
            ui.GetComponent<Login_UI_Model>().OnSign(code);
            //var ui = UI_Manager.Instance.GetUI("Login_UI") as Login_UI;
            //Debug.Log(ui);
            //ui.OnSign(code);    
        }
        #endregion

        #region 로그인
        private void Setup_Login()
        {
            server.Socket.On<int>("login", (code) =>
            {
                OnLogin(code); 
            });        
        }  
        
        public void Login(string pwd)
        {
            server.Socket.Emit("login", SteamManager.Instance.GetAccount().steamID.ToString(), pwd);
        }
        
        public void OnLogin(int code)
        {
            var ui = UI_Manager.Instance.GetUIView("Login_UI") as Login_UI;
            ui.GetComponent<Login_UI_Model>().OnLogin(code);
            //var ui = UI_Manager.Instance.GetUI("Login_UI") as Login_UI;
            //Debug.Log(ui);
            //ui.OnLogin(code);
        }
        #endregion
        
        #region 유저 데이터
        public User user = new User();
        
        #endregion
        
        #region 프로필
        private void Setup_Profile()
        {
            server.Socket.On<int, string>("profile", (code, json) =>
            {
                OnGetProfile(code, json); 
            });        
        }  
        
        public void GetProfile()
        {
            server.Socket.Emit("profile", SteamManager.Instance.GetAccount().steamID.ToString());
        }
        
        public void OnGetProfile(int code, string json)
        {
            var dataStr = "";
            Dictionary<string, object> data = null;

            switch (code)
            {
                case 0:
                    dataStr = json;
                    dataStr = dataStr.Replace("[", "");
                    dataStr = dataStr.Replace("]", "");

                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);

                    user.nickName = data["nickname"].ToString();
                    user.rank = Convert.ToInt32(data["rank"]);
                    user.rankPoint = Convert.ToInt32(data["rankpoint"]);
                    user.mmr = Convert.ToInt32(data["mmr"]);

                    var ui = UI_Manager.Instance.GetUIView("Top_UI") as Top_UI;
                    if (ui != null)
                    {
                        ui.GetModel().OnRefreshGoods();  
                    }
                    break;
            }
        }
        #endregion
        
        #region 인벤토리
        private void Setup_Inventory()
        {
            server.Socket.On<int, string>("inventory", (code, json) =>
            {
                OnGetInventory(code, json); 
            });        
        }

        public void GetInventory()
        {
            server.Socket.Emit("inventory", SteamManager.Instance.GetAccount().steamID.ToString());
        }
        
        public void OnGetInventory(int code, string json)
        {
            switch (code)
            {
                case 0:
                    var rep = json;
                    rep = rep.Replace("[", "");
                    rep = rep.Replace("]", "");

                    Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rep);

                    user.gold = Convert.ToInt32(data["gold"]);
                    user.cash = Convert.ToInt32(data["cash"]);
                    
                    var ui = UI_Manager.Instance.GetUIView("Top_UI") as Top_UI;
                    if (ui != null)
                    {
                        ui.GetModel().OnRefreshGoods();  
                    }
                    //callback_getInventory.Invoke(data);
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
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
            

        }

        private void Start()
        {
            Setup();
            Setup_Sign();
            Setup_Login();
            Setup_Profile();
            Setup_Inventory();
        }

        #endregion
    }
}
