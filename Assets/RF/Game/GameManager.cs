using System;
using System.Collections;
using System.Collections.Generic;
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
        SetupTeam();
    }

    private void Update()
    {
        UpdateGame();
    }
    #endregion
    
    #region 팀
    private SteamId[,] teamMembers = new SteamId[2, 4];

    private void SetupTeam()
    {
        var index = 0;
        foreach (var idStr in SteamManager.Instance.GetLobbyMemberIds())
        {
            ulong id = Convert.ToUInt64(idStr);
            if (index < 4)
            {
                teamMembers[0, index] = id;
            }
            else
            {
                teamMembers[1, ] = id;
            }
            
            index++;
        }
    }
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
