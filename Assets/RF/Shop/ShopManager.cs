using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Singletone
    public static ShopManager Instance;
    #endregion

    #region Shop UI
    private void Setup()
    {
        var data = Backend.Chart.GetChartContents("29978");
        Debug.Log(data);

    }
    #endregion

    #region Unity General Funcs
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Setup();
    }
    #endregion
}
