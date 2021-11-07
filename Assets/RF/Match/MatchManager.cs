using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    #region 싱글톤
    public static MatchManager Instance;
    #endregion
    
    #region 매치
    public void FindQuickMatch()
    {
        
    }
    #endregion

    #region 유니티 기본 함수
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion
}
