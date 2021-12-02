using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UI_Manager : SerializedMonoBehaviour
{
    #region Singletone
    public static UI_Manager Instance;
    #endregion
    
    #region UI Pool

    [SerializeField] private Canvas ui_Canvas;
    [SerializeField] private Transform ui_Layer;
    [SerializeField] private Transform popup_Layer;

    private List<UI_Base> uiPool_List = new List<UI_Base>();
    private List<Popup_Base> popupPool_List = new List<Popup_Base>();
    private List<UIItem_Base> uiItemPool_List = new List<UIItem_Base>();
    
    private GameObject ui_Pool_OBJ;

    private void SetUp()
    {
        ui_Pool_OBJ = new GameObject();
        ui_Pool_OBJ.name = "UI_Pool";
        
        DontDestroyOnLoad(ui_Pool_OBJ);
        DontDestroyOnLoad(ui_Canvas.gameObject);
    }

    public T CreateUI<T>(string name = "") where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject obj = Instantiate(Resources.Load($"Prefabs/UI/{name}")) as GameObject;
        obj.transform.SetParent(ui_Layer, false);
        T ui = obj.GetComponent<T>();
        
        uiPool_List.Add(ui);

        return ui;
    }

    public void RemoveUI(UI_Base ui)
    {
        if (uiPool_List.Contains(ui))
        {
            uiPool_List.Remove(ui);
        }
        ui.gameObject.SetActive(false);
        //Destroy();
    }

    public void CleanUI()
    {
        int max = uiPool_List.Count;
        for (int i=0;i<max;i++)
        {
            RemoveUI(uiPool_List[i]);
        }
    }

    public T CreatePopup<T>(string name = "") where T : Popup_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject obj = Instantiate(Resources.Load($"Prefabs/UI/Popup/{name}")) as GameObject;
        obj.transform.SetParent(popup_Layer, false);
        T popup = obj.GetComponent<T>();
        
        popupPool_List.Add(popup);

        return popup;    
    }

    public void RemovePopup(Popup_Base popup)
    {
        Destroy(popup.gameObject);
    }

    public void CleanPopup()
    {
        int index = 0;
        while (popupPool_List.Count == 0)
        {
            var obj = popupPool_List[index].gameObject;
            popupPool_List.RemoveAt(index);
            
            Destroy(obj);
        }
    }

    public T CreateUIItem<T>(string name = "") where T : UIItem_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        GameObject obj = Instantiate(Resources.Load($"Prefabs/UI/Item/{name}")) as GameObject;
        T uiItem = obj.GetComponent<T>();

        if (!uiItemPool_List.Contains(uiItem))
        {
            uiItemPool_List.Add(uiItem);
        }
        uiItemPool_List.Add(uiItem);

        return uiItem;
    }

    public void RemoveUIItem(UIItem_Base uiItem)
    {
        Destroy(uiItem.gameObject);
    }

    public void CleanUIItem(UIItem_Base uiItem)
    {
        if (!uiItemPool_List.Contains(uiItem))
        {
            return;
        }

        int index = 0;
        while (uiItemPool_List.Count == 0)
        {
            var obj = uiItemPool_List[index].gameObject;
            uiItemPool_List.RemoveAt(index);
            
            Destroy(obj);
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

        SetUp();
    }
    #endregion
    
    #region Inspector
    #endregion
}
