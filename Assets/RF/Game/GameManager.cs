using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RF.Player;
using RF.Team;
using Sirenix.Utilities;
using Steamworks;
using UnityEngine;


public enum GameStatus
{
    Start,
    Active,
    End,
}
public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager Instance;
    #endregion
    
    #region 스폰포인트
    [SerializeField] private List<Transform> red_SpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> blue_SpawnPoints = new List<Transform>();
    #endregion
    
    #region 이니셜라이즈
    public virtual void Initialize()
    {
        CreateTeams();

        var local = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        localPly = local.GetComponent<Player>();
    }
    #endregion
    
    #region 플레이어
    private List<Player> players = new List<Player>();
    private Player localPly;

    public Player LocalPlayer()
    {
        return localPly;
    }
    
    public void AddPlayer(Player ply)
    {
        if (players.Contains(ply))
        {
            return;
        }
        
        players.Add(ply);
    }

    public void RemovePlayer(Player ply)
    {
        if (!players.Contains(ply))
        {
            return;
        }
        
        players.Remove(ply);
    }
    
    public int GetPlayerCount()
    {
        return players.Count;
    }
    #endregion
    
    #region 게임 진행
    private GameStatus gameStatus = GameStatus.Start;
    private int winnerTeam = -1;

    private void SetGameStatus(GameStatus status)
    {
        gameStatus = status;

        switch (status)
        {
            case GameStatus.Start:
                OnGameStart();
                break;
            case GameStatus.Active:
                OnGameActive();
                break;
            case GameStatus.End:
                OnGameEnd();
                break;
        }
    }
    
    private void OnGameStart()
    {
        Debug.Log("On Game Start");
    }
    
    private void OnGameStartThink()
    {
        Debug.Log("On Game Start");
    }

    private void OnGameActive()
    {
        Debug.Log("On Game Active");
    }
    
    private void OnGameActiveThink()
    {
        if (teams.GetTeams()[0].members.Count > 0 && teams.GetTeams()[1].members.Count <= 0)
        {
            winnerTeam = 0;
        }
        else if (teams.GetTeams()[1].members.Count > 0 && teams.GetTeams()[0].members.Count <= 0)
        {
            winnerTeam = 1;
        }
        else
        {
            winnerTeam = -1;
        }
    }

    private void OnGameEnd()
    {
        Debug.Log("On Game End");
    }
    
    private void OnGameEndThink()
    {
        Debug.Log("On Game End Think"); 
    }
    #endregion

    #region 팀
    private Team teams = new Team();

    public virtual void CreateTeams()
    {
        teams.SetUp((int)TeamEnum.RED, "레드팀", red_SpawnPoints);
        teams.SetUp((int) TeamEnum.BLUE, "블루팀", blue_SpawnPoints); 
    }

    public Team Team()
    {
        return teams;
    }
    #endregion

    #region 유니티 기본 함수
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Initialize();
    }

    private void Start()
    {

    }

    private void Update()
    {
        switch (gameStatus)
        {
            case GameStatus.Start:
                OnGameStartThink();
                break;
            case GameStatus.Active:
                OnGameActiveThink();
                break;
            case GameStatus.End:
                OnGameEndThink();
                break;
        }
    }
    #endregion
}