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

    [SerializeField] private Transform ui_Layer;
    [SerializeField] private Transform popup_Layer;

    private GameObject uiPool_Object;
    
    private List<UI_Base>uiPool_List = new List<UI_Base>();
    private List<Popup_Base>popupPool_List = new List<Popup_Base>();

    private int uiIndex = -1;
    private int popupIndex = -1;
    
    private void Setup()
    {
        if (uiPool_Object != null)
        {
            return;
        }
        uiPool_Object = new GameObject();
        uiPool_Object.name = "UI_Pool_Object";
        DontDestroyOnLoad(uiPool_Object);
    }

    public void CreateUI(EnumData.UIType uiType)
    {
        if (uiIndex >= 0)
        {
            uiPool_List[uiIndex].gameObject.SetActive(false);
            uiPool_List[uiIndex].transform.SetParent(uiPool_Object.transform, false);
        }

        var ui = Instantiate(Resources.Load("Prefabs/UI/" + uiType.ToString() + "_UI") as GameObject, ui_Layer);
        ui.transform.SetParent(ui_Layer, false);
        ui.gameObject.name = uiType.ToString() + "_UI";

        uiPool_List.Add(ui.GetComponent<UI_Base>());
        
        uiIndex = uiPool_List.Count - 1;
    }
    
    public void CreatePopup(EnumData.PopupType popupType)
    {
        if (popupIndex >= 0)
        {
            //popupPool_List[popupIndex].gameObject.SetActive(false);
            //popupPool_List[popupIndex].transform.SetParent(uiPool_Object.transform, false);
        }

        var ui = Instantiate(Resources.Load("Prefabs/UI/Popup/" + popupType.ToString() + "_Popup") as GameObject, popup_Layer);
        ui.transform.SetParent(popup_Layer, false);
        ui.gameObject.name = popupType.ToString() + "_Popup";

        popupPool_List.Add(ui.GetComponent<Popup_Base>());
        
        popupIndex = popupPool_List.Count - 1;
    }

    public void RemoveUI(EnumData.UIType uiType)
    {
        Destroy(GameObject.Find(uiType.ToString() + "_UI"));
        //Destroy();
    }

    public void RemoveAllUI()
    {
        foreach (var ui in uiPool_List)
        {
            Destroy(ui.gameObject);
        }
    }

    public void RemovePopup(EnumData.PopupType popupType)
    {
        GameObject find = GameObject.Find(popupType.ToString() + "_Popup");
        Popup_Base type = find.GetComponent<Popup_Base>();

        int index = 0;
        bool check = false;
        foreach (var popup in popupPool_List)
        {
            if (popup.GetType() == type.GetType())
            {
                check = true;
                break;
            }

            index++;
        }

        if (check)
        {
            popupPool_List.RemoveAt(index);
        }
        
        Destroy(GameObject.Find(popupType.ToString() + "_Popup"));
        popupIndex = popupPool_List.Count-1;
    }

    public void BackUI()
    {
        uiPool_List.RemoveAt(uiIndex);

        uiIndex = uiPool_List.Count - 1;
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

        Setup();
    }
    #endregion
    
    #region Inspector
    #endregion
}
