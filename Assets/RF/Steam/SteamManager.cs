using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using Color = UnityEngine.Color;

public class SteamManager : MonoBehaviour
{
    #region Singletone
    public static SteamManager Instance;
    #endregion
    
    #region Steam Client
    private uint appID = 480;

    public SteamId steamID;
    public string Nickname;

    private void Setup()
    {
        try
        {
            SteamClient.Init(appID);
        }
        catch (SystemException e)
        {
            Debug.Log("Steam Error :" + e.Message);
        }

        if (SteamClient.IsValid)
        {
            steamID = SteamClient.SteamId;
            Nickname = SteamClient.Name;
        }
    }
    
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
    
    #region Steam Friends

    public IEnumerable<Friend> GetFriends()
    {
        return SteamFriends.GetFriends();
    }
    #endregion
    
    #region Steam Lobby
    public Lobby currentLobby;
    public readonly int maxMembers = 4;

    public Action<Queue<string>, Lobby> lobbyCreatedAction;
    
    private Dictionary<SteamId, SteamLobbyClient> lobbyMembers = new Dictionary<SteamId, SteamLobbyClient>();
    private List<string> lobbyMemberIds = new List<string>();
    private Queue<string> inviteQueue = new Queue<string>();
    
    private void SetupLobby()
    {
        SteamMatchmaking.OnLobbyCreated += onLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += onLobbyEntered;
        SteamMatchmaking.OnLobbyInvite += onLobbyInvited;
        SteamMatchmaking.OnLobbyDataChanged += onLobbyDataChanged;
        SteamMatchmaking.OnLobbyMemberJoined += onLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += onLobbyMemberLeave;
        SteamMatchmaking.OnLobbyMemberDataChanged += onLobbyMemberDataChanged;
        SteamMatchmaking.OnLobbyMemberDisconnected += onLobbyMemberDisconnected;
        SteamMatchmaking.OnLobbyMemberKicked += onLobbyMemberKicked;

        CreateLobby();
        /*
        lobbyCreatedAction = (queue, lb) =>
        {
            Debug.Log("lobby create");
            
            MatchAccept_Popup popup = UI_Manager.Instance.CreatePopup<MatchAccept_Popup>();
            popup.SetLobby(lb);

            LobbyManager.Instance.CreateQuickMatch(lb.Id, steamID);
            /*
            int count = queue.Count;
            for (int i=0; i<count;i++)
            {
                string id = queue.Dequeue();
                ulong id64 = Convert.ToUInt64(id);
                SteamId steamID = id64;
                Friend friend = new Friend(steamID);

                Debug.Log("friend : " + id64);
                if (!friend.IsMe)
                {
                    lb.InviteFriend(steamID); 
                }
            }
        };*/
    }
    
    public void CreateLobby()
    {
        /*for (int i = 0; i < users.Count; i++)
        {
            Friend friend = new Friend(Convert.ToUInt64(users[i]));
            if (friend.IsMe)
            {
                continue;
            }
            inviteQueue.Enqueue(users[i]);
        }*/
        
        SteamMatchmaking.CreateLobbyAsync(maxMembers);
    }
    public void CreateLobby(List<string> users)
    {
        /*for (int i = 0; i < users.Count; i++)
        {
            Friend friend = new Friend(Convert.ToUInt64(users[i]));
            if (friend.IsMe)
            {
                continue;
            }
            inviteQueue.Enqueue(users[i]);
        }*/
        
        SteamMatchmaking.CreateLobbyAsync(maxMembers);
    }

    public void LeaveLobby()
    {
        currentLobby.Leave();
    }

    public void JoinLobby(Lobby lb)
    {
        SteamMatchmaking.JoinLobbyAsync(lb.Id);
    }
    
    public void JoinLobby(SteamId id)
    {
        //SteamMatchmaking.JoinLobbyAsync()
        SteamMatchmaking.JoinLobbyAsync(id);
    }

    public Dictionary<SteamId, SteamLobbyClient> GetLobbyMembers()
    {
        return lobbyMembers;
    }

    public void SetLobbyMembers(Dictionary<SteamId, SteamLobbyClient> members)
    {
        lobbyMembers = members;
    }

    public List<string> GetLobbyMemberIds()
    {
        return lobbyMemberIds;
    }

    private void onLobbyCreated(Result result, Lobby lobby)
    {
        currentLobby = lobby;
        
        //RefreshLobby();

        lobbyCreatedAction.Invoke(inviteQueue, currentLobby);
    }

    private void onLobbyEntered(Lobby lobby)
    {
        currentLobby = lobby;
        
        lobbyMembers.Clear();
        foreach (var item in lobby.Members)
        {
            SteamLobbyClient client = new SteamLobbyClient();
            client.photonID = lobby.GetMemberData(item, "photonID");
            client.steamID = item.Id;
            lobbyMembers.Add(item.Id, client);
        }
        
        if (lobby.MemberCount > 8)
        {
            return;
        }
        
        if (lobby.Owner.IsMe)
        {
            return;
        }
        var matching = UI_Manager.Instance.CreatePopup<MatchAccept_Popup>();
        matching.SetLobby(lobby);
        //RefreshLobby();
    }

    private void onLobbyInvited(Friend friend, Lobby lobby)
    {
        Debug.Log("invited");
        //var invite = UI_Manager.Instance.CreatePopup<PartyInvite_Popup>();
        //invite.SetLobby(lobby);
    }

    private void onLobbyDataChanged(Lobby lobby)
    {
        
    }

    private void onLobbyMemberJoined(Lobby lobby, Friend friend)
    {
        Debug.Log("member joined");
        lobbyMembers.Clear();
        foreach (var item in lobby.Members)
        {
            SteamLobbyClient client = new SteamLobbyClient();
            client.photonID = lobby.GetMemberData(item, "photonID");
            client.steamID = item.Id;
            lobbyMembers.Add(item.Id, client);
        }
        //RefreshLobby();
    }

    private void onLobbyMemberLeave(Lobby lobby, Friend friend)
    {
        lobbyMembers.Clear();
        foreach (var item in lobby.Members)
        {
            SteamLobbyClient client = new SteamLobbyClient();
            client.photonID = lobby.GetMemberData(item, "photonID");
            client.steamID = item.Id;
            lobbyMembers.Add(item.Id, client);
        }
        Debug.Log("member leave");
        //RefreshLobby();
    }

    private void onLobbyMemberDataChanged(Lobby lobby, Friend friend)
    {
        
    }

    private void onLobbyMemberDisconnected(Lobby lobby, Friend friend)
    {
        //RefreshLobby();
    }

    private void onLobbyMemberKicked(Lobby lobby, Friend friend, Friend friend2)
    {
        //RefreshLobby();
    }

    public bool IsStartGame()
    {
        return false;
    }
    #endregion
    
    #region Steam Networking

    #endregion

    #region Unity General Funcs
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
        SetupLobby();
    }

    #endregion
}
