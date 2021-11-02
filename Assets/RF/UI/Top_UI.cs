using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Top_UI : UI_Base
{
    [SerializeField] private Text gold_Text;
    [SerializeField] private Text cash_Text;

    private void Awake()
    {
        ServerManager.Instance.GetInventory(GetAction);
        Debug.Log("getinventory");
    }

    private void GetAction(Dictionary<string,object> data)
    {
       SetGold(Convert.ToInt32(data["gold"]));
       SetCash(Convert.ToInt32(data["cash"]));
    }

    private void SetGold(int gold)
    {
        gold_Text.text = gold.ToString();
    }

    private void SetCash(int cash)
    {
        cash_Text.text = cash.ToString();
    }

    public void Refresh()
    {
        
    }
}
