using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Singletone
    public static ShopManager Instance;
    #endregion

    #region Shop UI

    [SerializeField] private Shop_UI shopUI;
    
    private void Setup()
    {
        var find = Backend.Chart.GetChartList();
        if (find.IsSuccess())
        {
            JsonData chartRows = find.GetReturnValuetoJSON()["rows"];
            for (int i = 0; i < chartRows.Count; i++)
            {
                JsonData data = chartRows[i];
                var getItems = Backend.Chart.GetChartContents((string) data["selectedChartFileId"][0]);
                if (getItems.IsSuccess())
                {
                    JsonData itemRows = getItems.Rows();

                    for (int i2 = 0; i2 < itemRows.Count; i2++)
                    {
                        string id = itemRows[i2]["itemID"][0].ToString();
                        string name = itemRows[i2]["itemName"][0].ToString();
                        string itemType = itemRows[i2]["ItemType"][0].ToString();
                        string buyType = itemRows[i2]["BuyType"][0].ToString();
                        string price = itemRows[i2]["Price"][0].ToString();

                        
                    }
                }
            }
        }
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
