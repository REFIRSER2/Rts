using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

public class Shop_UI : UI_Base
{
    [SerializeField] private GameObject auctionScroll;
    [SerializeField] private GameObject shopScroll;

    [SerializeField] private Transform auctionTransform;
    [SerializeField] private Transform shopTransform;

    public void AddItem(string id, string name, string itemType, string buyType, string price)
    {
        var item = UI_Manager.Instance.CreateUIItem<Shop_Item>();
        item.transform.SetParent(shopTransform, false);
        item.SetName(name);
        item.SetPrice(price);
    }
    
    private void Awake()
    {
        auctionScroll.SetActive(false);
        shopScroll.SetActive(true);
    }
}
