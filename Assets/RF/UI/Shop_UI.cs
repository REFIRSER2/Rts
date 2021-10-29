using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;

public class Shop_UI : UI_Base
{
    [SerializeField] private Transform shop_Content;

    [SerializeField] private List<GameObject> subCat_List = new List<GameObject>();

    private int subCat = 0;
    
    private int page = 1;
    
    public void AddItem(string id, string name, string itemType, string buyType, string price)
    {
    
    }

    private void RefreshItems()
    {
        BackendManager.Instance.GetShop_Items();
        //Debug.Log();
        /*foreach (var VARIABLE in COLLECTION)
        {
            
        } */
    }
    
    public void onNext()
    {
        
    }

    public void onPrev()
    {
        
    }
    
    private void Awake()
    {
        RefreshItems();
    }
}