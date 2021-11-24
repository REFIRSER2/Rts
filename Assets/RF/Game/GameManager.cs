using System;
using System.Collections;
using System.Collections.Generic;
using RF.Player;
using RF.Team;
using Sirenix.Utilities;
using Steamworks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager Instance;
    #endregion

    #region 유니티 기본 함수
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Team.SetUp((int)TeamEnum.RED, "레드팀");
        Team.SetUp((int)TeamEnum.BLUE, "블루팀");
        
        SetupNetwork();
    }

    private void Update()
    {
        UpdateGame();
    }
    #endregion
    
    #region 네트워크

    private void SetupNetwork()
    {
        SteamNetworking.OnP2PSessionRequest = (steamID) =>
        {
            if (SteamManager.Instance.GetLobbyMemberIds().Contains(steamID.ToString()))
            {
                SteamNetworking.AcceptP2PSessionWithUser(steamID);
            }
        };
        
    }

    private void HandleMessage(SteamId id, byte[] data)
    {
        
    }
    #endregion
    
    #region 플레이어 관리
    private int playerIndex = 0;
    private int aliveIndex = 0;
    private int deathIndex = 0;
    
    private Player[] players = new Player[8];
    private Player[] alives = new Player[8];
    private Player[] deaths = new Player[8];
    
    public Player[] GetPlayers()
    {
        return players;
    }

    public Player[] AlivePlayers()
    {
        return alives;
    }

    public Player[] DeathPlayers()
    {
        return deaths;
    }
    
    public void SpawnPlayer()
    {
        
        
        //OnPlayerSpawn();
    }
    
    public virtual void OnPlayerSpawn(Player ply)
    {
        players[playerIndex + 1] = ply;

        playerIndex++;
    }
    #endregion
    
    #region 팀

    #endregion
    
    #region 참가
    
    #endregion
    
    #region 게임 체크
    private bool isEnd;
    private void UpdateGame()
    {
        if (SteamManager.Instance.GetLobbyMemberIds().Count <= 1)
        {
            if (!isEnd)
            {
                EndGame();    
            }
        }
    }
    #endregion
    
    #region 게임 종료

    private void EndGame()
    {
        isEnd = true;
    }
    #endregion
}