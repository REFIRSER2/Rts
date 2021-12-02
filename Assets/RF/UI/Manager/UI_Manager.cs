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

    private Dictionary<string, UI_Base> uiPool_List = new Dictionary<string, UI_Base>();
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

        T ui = null;
        
        if (!uiPool_List.ContainsKey(name))
        {
            GameObject obj = Instantiate(Resources.Load($"Prefabs/UI/{name}")) as GameObject;
            obj.transform.SetParent(ui_Layer, false);
            ui = obj.GetComponent<T>();
            
            uiPool_List.Add(name, ui);
        }
        else
        {
            uiPool_List[name].gameObject.SetActive(true);
            uiPool_List[name].On_Open();
        }

        return ui;
    }
    
    public void ReleaseUI<T>() where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        T ui = null;
        
        if (uiPool_List.ContainsKey(name))
        {
            uiPool_List[name].On_Close();
            uiPool_List[name].gameObject.SetActive(false);
            
            uiPool_List[name].transform.SetParent(ui_Pool_OBJ.transform);
        }
    }

    public void CleanUI()
    {
        int max = uiPool_List.Count;
        foreach (var item in uiPool_List)
        {
            Destroy(item.Value);
        }
        
        uiPool_List.Clear();
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
