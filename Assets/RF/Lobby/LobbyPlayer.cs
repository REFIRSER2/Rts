using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour, IPunObservable
{
    #region 기본 내장 함수

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #endregion
    
    #region 팀

    private int team = 0;

    public void SetTeam(int num)
    {
        team = num;
    }
    
    public int GetTeam()
    {
        return team;
    }
    #endregion
    
    #region 옵저버
    private bool isReady = false;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isReady);
        }
        else if(stream.IsReading)
        {
            isReady = Convert.ToBoolean(stream.ReceiveNext());
        }
    }
    #endregion
}
