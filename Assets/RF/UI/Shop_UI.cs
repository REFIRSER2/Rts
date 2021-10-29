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
    [SerializeField] private List<GameObject> pageButton_List = new List<GameObject>();

    private int cat = 0;
    private int subCat = 0;

    private int itemCount = 0;
    
    private int page = 1;
    private int slot = 1;
    
    public void AddItem(string id, string name, string itemType, string buyType, string price)
    {
        var item = UI_Manager.Instance.CreateUIItem<Shop_Item>();
        item.transform.SetParent(shop_Content, false);
        item.SetName(name);
        item.SetPrice(string.Format("{0} {1}", price.ToString(), (EnumData.Currently)(Convert.ToInt32(buyType))).ToString());
    }

    private void RefreshItems()
    {
        var data = BackendManager.Instance.GetShop_Items(cat);

        if (data == null)
        {
            return;
        }

        itemCount = data.Count;

        for (int i = 0; i < shop_Content.childCount; i++)
        {
            Destroy(shop_Content.GetChild(i).gameObject);
        }

        for (int i = 0; i < data.Count; i++)
        {
            Debug.Log(data.ToJson());
            
            int min = (slot - 1) * 8;
            int max = min + 8;
            if (i >= min && i < max)
            {
                AddItem(data[i]["ItemID"]["S"].ToString(), data[i]["ItemName"]["S"].ToString(), data[i]["ItemType"]["S"].ToString(), data[i]["BuyType"]["S"].ToString(), data[i]["Price"]["S"].ToString());
            }
        }
        //Debug.Log();
        /*foreach (var VARIABLE in COLLECTION)
        {
            
        } */
    }

    private void RefreshPageBtns()
    {
        for (int i = 0; i < pageButton_List.Count; i++)
        {
            pageButton_List[i].SetActive(false);
        }
        
        for (int i = 0; i < pageButton_List.Count; i++)
        {
            //0 + (slot*8) < itemcount
            if (itemCount >= 0 + (((page-1)*4 + i+1) - 1) * 8)
            {
                pageButton_List[i].SetActive(true);  
            }
            
        }
    }

    public void onCat(int index)
    {
        Debug.Log("index :" + index);
        
        cat = index;
        page = 1;
        slot = 1;
        
        foreach (var obj in subCat_List)
        {
            obj.SetActive(false);
        }
        subCat_List[cat].SetActive(true);
        
        RefreshItems();
        RefreshPageBtns();
    }
    
    public void onNext()
    {
        if (itemCount < page * 4 * 8)
        {
            page += 1;  
            RefreshItems();
        }
        RefreshPageBtns();
    }

    public void onPrev()
    {
        if (page > 1)
        {
            page -= 1;
            RefreshItems();
        }

        RefreshPageBtns();
    }

    public void onSlot(int index)
    {
        slot = index + ((page-1)*4);
        RefreshItems();
    }

    private void Awake()
    {
        RefreshItems();
        
        RefreshPageBtns();
    }
}