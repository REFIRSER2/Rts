using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class ShopViewManager : MonoBehaviour
{
    #region Singletone
    public static ShopViewManager Instance;
    #endregion

    #region Shop View
    [SerializeField] private Transform itemInfo_View;
    [SerializeField] private Camera itemInfo_Cam;
    private RenderTexture itemInfo_RT;

    [SerializeField] private List<Transform> itemList_View;
    [SerializeField] private List<Camera> itemList_Cams;
    [SerializeField]private List<RenderTexture> itemList_RT = new List<RenderTexture>();

    private void Setup()
    {
        Setup_ItemInfo();
        Setup_ItemList();
    }

    private void Setup_ItemInfo()
    {
        var rt = new RenderTexture(314,314, 24);
        rt.Create();

        itemInfo_Cam.targetTexture = rt;
        itemInfo_RT = rt;
    }

    public RenderTexture GetItemInfo_RT()
    {
        return itemInfo_RT;
    }

    public void Create_ItemInfo_Mdl(string id)
    {
        GameObject mdl = Instantiate(Resources.Load("Prefabs/Models/Items/" + id) as GameObject);
        mdl.transform.SetParent(itemInfo_View, false);
        mdl.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void Setup_ItemList()
    {
        foreach (var cam in itemList_Cams)
        {
            var rt = new RenderTexture(320, 370, 24);
            rt.Create();

            cam.targetTexture = rt;
            
            itemList_RT.Add(rt);
        }
    }

    public RenderTexture GetItemList_RT(int index)
    {
        Debug.Log("index :" + index);
        Debug.Log("rt :" + itemList_RT[index]);
        return itemList_RT[index];
    }
    
    public void Create_ItemList_Mdl(int index, string id)
    {
        GameObject mdl = Instantiate(Resources.Load("Prefabs/Models/Items/" + id) as GameObject);
        mdl.transform.SetParent(itemList_View[index], false);
        mdl.transform.localPosition = new Vector3(0, 0, 0);    
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
