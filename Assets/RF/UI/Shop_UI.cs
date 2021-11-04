using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class Shop_UI : UI_Base
{
    [SerializeField] private Transform shop_Content;
    [SerializeField] private List<GameObject> subCat_List = new List<GameObject>();
    [SerializeField] private List<GameObject> pageButton_List = new List<GameObject>();

    [SerializeField] private ShopInfo_Item itemInfo; 
    
    private int cat = 0;
    private int subCat = 0;

    private int selItem = -1;
    
    private int itemCount = 0;
    
    private int page = 1;
    private int slot = 1;
    
    public void AddItem(int index, string id, string name, string itemType, string buyType, string price)
    {
        var item = UI_Manager.Instance.CreateUIItem<Shop_Item>();
        item.transform.SetParent(shop_Content, false);
        item.SetName(name);
        item.SetPrice(string.Format("{0} {1}", price.ToString(), (EnumData.Currently)(Convert.ToInt32(buyType))).ToString());
        item.SetModel(ShopViewManager.Instance.GetItemList_RT(index - (index/8)*8 ));

        int copyidx = index;
        item.GetComponent<CustomButton>().onClick.AddListener(()=>onItem(copyidx));
        
        item.GetBuyButton().onClick.AddListener(()=>onBuy(copyidx));
        ShopViewManager.Instance.Create_ItemList_Mdl(index - (index/8)*8, id);
    }

    private void RefreshItems()
    {
        /*
        //var data = BackendManager.Instance.GetShop_Items(cat);
        
        
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
            int min = (slot - 1) * 8;
            int max = min + 8;
            if (i >= min && i < max)
            {
                AddItem(i, data[i]["ItemID"]["S"].ToString(), data[i]["ItemName"]["S"].ToString(), data[i]["ItemType"]["S"].ToString(), data[i]["BuyType"]["S"].ToString(), data[i]["Price"]["S"].ToString());
            }
        }*/
    }

    private void RefreshPageBtns()
    {
        for (int i = 0; i < pageButton_List.Count; i++)
        {
            pageButton_List[i].SetActive(false);
        }
        
        for (int i = 0; i < pageButton_List.Count; i++)
        {
            if (itemCount >= 0 + (((page-1)*4 + i+1) - 1) * 8)
            {
                pageButton_List[i].SetActive(true);  
            }
            
        }
    }

    private void RefreshItemInfo()
    {

            itemInfo.SetTitle("선택되지 않은 아이템");
            itemInfo.SetEnergy(0);
            itemInfo.SetDamage(0);
            itemInfo.SetDps(0);
            itemInfo.SetHealth(0);
            itemInfo.SetReach(0);
            itemInfo.SetSight(0);
            itemInfo.SetSpeed(0);
            itemInfo.SetWeight(0);
    }

    public void onCat(int index)
    {
        cat = index;
        page = 1;
        slot = 1;
        selItem = -1;
        
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

    private void onBuy(int index)
    {
        /*
        var data = BackendManager.Instance.GetShop_Items(cat);

        if (data == null)
        {
            return;
        }

        Param param = new Param();
        param.Add("ItemCat", cat.ToString());
        param.Add("ItemIndex", index.ToString());
        param.Add("ItemID", data[index]["ItemID"].ToString());
        param.Add("UserID", BackendManager.Instance.GetID());

        var ret = Backend.BFunc.InvokeFunction("BuyItem", param);*/
    }
    
    private void onItem(int index)
    {
        /*
        var data = BackendManager.Instance.GetShop_Items(cat);

        if (data == null)
        {
            return;
        }

        for (int i = 0; i < shop_Content.childCount; i++)
        {
            shop_Content.GetChild(i).GetComponent<CustomButton>().UnSelect();
        }
        
        itemInfo.SetTitle(data[index]["ItemName"]["S"].ToString());*/
    }

    private void Awake()
    {
        itemInfo.SetModel(ShopViewManager.Instance.GetItemInfo_RT());
        
        RefreshItemInfo();
        RefreshItems();
        RefreshPageBtns();
    }
}