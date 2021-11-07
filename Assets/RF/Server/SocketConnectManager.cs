using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SocketIO3;
using Newtonsoft.Json;
using Sirenix.Serialization;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

public class SocketConnectManager : MonoBehaviour
{
    public static SocketConnectManager Instance;
    
    private SocketManager mainServer;
    private SocketManager lobbyServer;
    private SocketManager chatServer;
    private SocketManager inGameServer;
    private SocketManager matchServer;

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

        mainServer = new SocketManager(new Uri("http://54.180.191.145:27000"));
        mainServer.Open();

        //chatServer = new SocketManager(new Uri("http://54.180.191.145:27001"));
        lobbyServer = new SocketManager(new Uri("http://54.180.191.145:27001"));
        lobbyServer.Open();
        
        chatServer = new SocketManager(new Uri("http://54.180.191.145:27002"));
        chatServer.Open();

        matchServer = new SocketManager(new Uri("http://54.180.191.145:27003"));
        matchServer.Open();

        inGameServer = new SocketManager(new Uri("http://54.180.191.145:27004"));
        inGameServer.Open();

        InitMainHandler();
        InitChatHandler();
        InitMatchHandler();
    }

    public SocketManager GetMainServer()
    {
        return mainServer;
    }

    public SocketManager GetLobbyServer()
    {
        return lobbyServer;
    }

    public SocketManager GetChatServer()
    {
        return chatServer;
    }

    public SocketManager GetInGameServer()
    {
        return inGameServer;
    }

    private void OnApplicationQuit()
    {
        if (matchID != 0)
        {
            LeaveQuickMatch();  
        }
    }

    private void InitMainHandler()
    {
        var connect = UI_Manager.Instance.CreateUI<Connecting_UI>();
        mainServer.Socket.On("connect", () =>
        {
            UI_Manager.Instance.RemoveUI(connect);
            UI_Manager.Instance.CreateUI<Login_UI>();
        }); 
        
        mainServer.Socket.On("disconnect", () =>
        {
            UI_Manager.Instance.CleanUI();
            connect = UI_Manager.Instance.CreateUI<Connecting_UI>();
        });
        
        mainServer.Socket.On<bool, int>("sign", (can,status ) =>
        {
            signAction.Invoke(can, status);
        });
        
        mainServer.Socket.On<bool, int>("login", (can,status ) =>
        {
            Debug.Log("로그인");
            Debug.Log("로그인 " + status);
            loginAction.Invoke(can, status);
        });

        mainServer.Socket.On<bool, string>("inventory", (can, json) =>
        {
            Debug.Log(json);
            if (can)
            {
                var data = json;
                data = data.Replace("[", "");
                data = data.Replace("]", "");
                getInventoryAction.Invoke(JsonConvert.DeserializeObject<Dictionary<string,object>>(data));
                Debug.Log(JsonConvert.DeserializeObject<Dictionary<string,object>>(data));    
            }
            Debug.Log(json);
        });

        mainServer.Socket.On<bool, string>("profile", (can, json) =>
        {
            if (can)
            {
                var data = json;
                data = data.Replace("[", "");
                data = data.Replace("]", "");
                getProfileAction.Invoke(JsonConvert.DeserializeObject<Dictionary<string,object>>(data));
            }
        });
        
        mainServer.Socket.On<ulong>("create party", (id) =>
        {
            onCreateParty.Invoke(id);
        });
        
        mainServer.Socket.On<List<ulong>>("join party", (members) =>
        {
            PartySystem.Instance.JoinParty(members);
        });
    }

    private void InitChatHandler()
    {
        lobbyServer.Socket.On<ChatData>("chat", (data) =>
        {
            var chatUI = FindObjectOfType<Chat_UI>();
            if (chatUI != null)
            {
                chatUI.OnReceiveMessage(data.nickName, data.message, data.channel);
            }
        });   
        
        /*
        inGameServer.Socket.On<ChatData>("chat", (data) =>
        {
            var chatUI = FindObjectOfType<Chat_UI>();
            if (chatUI != null)
            {
                chatUI.OnReceiveMessage(data.nickName, data.message, data.channel);
            }    
        });*/
    }

    private void InitMatchHandler()
    {
        matchServer.Socket.On<ulong>("find quick match", (id) =>
        {
            findMatchAction.Invoke(id);
        });
        
        matchServer.Socket.On<ulong>("joinRoom", (id) =>
        {
            Debug.Log(id);
            Debug.Log((SteamId)Convert.ToUInt64(id));
        });
        
        matchServer.Socket.On<List<string>>("leave quick match", (members) =>
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i] == SteamManager.Instance.steamID.ToString())
                {
                    matchID = 0;
                    SteamManager.Instance.LeaveLobby();

                    if (i == 0)
                    {
                        
                    }
                }
            }
            if (members.Contains(SteamManager.Instance.steamID.ToString()))
            {

            }
        });
        

        /*matchServer.Socket.On<int, SteamId>("joinRoom", (id, lobby) =>
        {
            matchID = id;
            SteamManager.Instance.JoinLobby(lobby);
        });*/
    }

    #region Chat
    public void SendChat(ChatData data)
    {
        lobbyServer.Socket.Emit("chat", data);
    }
    #endregion
    
    #region Inventory

    private Action<Dictionary<string, object>> getInventoryAction;
    public void GetInventory(Action<Dictionary<string, object>> action)
    {
        mainServer.Socket.Emit("inventory", SteamManager.Instance.steamID.ToString());
        getInventoryAction -= action;
        getInventoryAction += action;
    }
    #endregion
    
    #region Account

    private Action<bool, int> signAction;
    public void Sign(Sign_Popup popup, string nickName, string pwd, string email)
    {
        mainServer.Socket.Emit("sign", SteamManager.Instance.steamID.ToString(), nickName, pwd, email);
        signAction = (can, code) =>
        {
            if (can)
            {
               popup.Remove(); 
            }
            else
            {
                Error_Popup error = UI_Manager.Instance.CreatePopup<Error_Popup>();
                
                switch (code)
                {
                    default:
                        error.SetTitle("오류");
                        error.SetText("알 수 없는 오류로 로그인 할 수 없습니다");
                        break;
                    case 1:
                        error.SetTitle("오류");
                        error.SetText("이메일을 형식에 맞게 입력해주세요");
                        break;
                    case 2:
                        error.SetTitle("오류");
                        error.SetText("이미 계정이 존재합니다");
                        break;
                    case 3:
                        error.SetTitle("오류");
                        error.SetText("DB 서버에 접근 할 수 없습니다.");
                        break;
                }
            }
        };
    }
    
    private Action<Dictionary<string, object>> getProfileAction;
    public UserProfile userProfile;
    public void GetProfile(string id)
    {
        mainServer.Socket.Emit("profile", id);
        getProfileAction = (data) =>
        {
            if (userProfile != null)
            {
                return;
            }
            userProfile = new UserProfile();
            userProfile.nickName = data["nickname"].ToString();
            userProfile.rank = Convert.ToInt32(data["rank"]);
            userProfile.rankPt = Convert.ToInt32(data["rankpoint"]);
            userProfile.mmr = Convert.ToInt32(data["mmr"]);
        };
    }

    #endregion

    #region 파티

    private Action<ulong> onCreateParty;
    private Action<ulong> onInviteParty;
    public void CreateParty()
    {
        mainServer.Socket.Emit("create party", SteamManager.Instance.steamID.ToString());

        onCreateParty = (id) =>
        {
            PartySystem.Instance.CreateParty(id);
        };
    }
    
    public void InviteParty(ulong id)
    {
        mainServer.Socket.Emit("invite party", SteamManager.Instance.steamID.ToString());
    }
    #endregion
    
    #region 매칭
    private Action<ulong> findMatchAction;
    private ulong matchID = 0;
    public void FindQuickMatch(int mode, Action<ulong> action)
    {
        matchServer.Socket.Emit("find quick match", SteamManager.Instance.steamID.ToString(), SteamManager.Instance.currentLobby.Id.Value, mode, SteamManager.Instance.GetPartyMemberIds());
        findMatchAction = action;
    }

    public void LeaveQuickMatch()
    {
        matchServer.Socket.Emit("leave quick match",  matchID, SteamManager.Instance.GetPartyMemberIds());
    }

    private Action<int> joinQuickMatchAction;
    public void JoinQuickMatch()
    {
        
    }

    public void ReJoinQuickMatch()
    {
        matchServer.Socket.Emit("rejoin quick lobby", SteamManager.Instance.currentLobby);
    }

    
    #endregion
}
