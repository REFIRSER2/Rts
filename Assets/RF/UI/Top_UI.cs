using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Top_UI : UI_Base
{
    [SerializeField] private Text nickName_Text;
    [SerializeField] private Text ship_Text;

    [SerializeField] private Text level_Text;
    [SerializeField] private Text exp_Text;

    [SerializeField] private Text gold_Text;
    [SerializeField] private Text cash_Text;

    private void Awake()
    {
        SetNickname(BackendManager.Instance.GetLocalNickname());
        SetGold(BackendManager.Instance.GetGold());
        SetCash(BackendManager.Instance.GetCash());
        SetLevel(BackendManager.Instance.GetLevel());
        SetEXP(BackendManager.Instance.GetEXP());
    }

    private void SetNickname(string nick)
    {
        nickName_Text.text = nick;
    }

    private void SetShip(string ship)
    {
        ship_Text.text = ship;
    }

    private void SetLevel(int level)
    {
        level_Text.text = level.ToString() + "/" + "";
    }

    private void SetEXP(int exp)
    {
        exp_Text.text = exp.ToString() + "/" + "";
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
