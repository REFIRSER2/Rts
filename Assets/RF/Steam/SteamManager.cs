using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamManager : MonoBehaviour
{
    #region Singletone
    public static SteamManager Instance;
    #endregion
    
    #region Steam
    private uint appID = 480;

    public SteamId steamID;
    
    private void Setup()
    {
        try
        {
            SteamClient.Init(appID);
        }
        catch (SystemException e)
        {
            
        }

        if (SteamClient.IsValid)
        {
            steamID = SteamClient.SteamId;

        }

    }
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
    }

    #endregion
}
