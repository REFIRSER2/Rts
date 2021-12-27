using System;
using System.Collections.Generic;
using UnityEngine;

namespace RF.UI
{
    public class UI_Manager : MonoBehaviour
    {
        #region 싱글톤
        public static UI_Manager Instance;
        #endregion
        
        #region 유니티 기본 내장 함수
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
        }
        #endregion

        #region UI
        private Dictionary<string, UI_Base> uiViewList = new Dictionary<string, UI_Base>();
        
        public void AddUIView(string uiName, UI_Base ui)
        {
            if (uiViewList.ContainsKey(uiName))
            {
                return;
            }
            
            uiViewList.Add(uiName, ui);
        }

        public UI_Base GetUIView(string uiName)
        {
            if (!uiViewList.ContainsKey(uiName))
            {
                return null;
            }

            return uiViewList[uiName];
        }

        public void SetActiveUI(string uiName, bool isActive)
        {
            if (!uiViewList.ContainsKey(uiName))
            {
                return;
            }

            if (uiViewList[uiName] == null)
            {
                return;
            }
             
            uiViewList[uiName].gameObject.SetActive(isActive);
        }
        #endregion
        
        #region 팝업 뷰
        private Dictionary<string, List<UI_Popup_Base>> popupViewList = new Dictionary<string, List<UI_Popup_Base>>();

        public UI_Popup_Base CreatePopupView(string name)
        {
            Transform layer = GameObject.Find("Popup_Layer").transform;
            GameObject popup = Instantiate(Resources.Load("Prefabs/UI/Popup/" + string.Format("{0}", name)), layer) as GameObject;
            if (!popupViewList.ContainsKey(name))
            {
                popupViewList.Add(name, new List<UI_Popup_Base>());
            }
            popupViewList[name].Add(popup.GetComponent<UI_Popup_Base>());
            
            
            return popup.GetComponent<UI_Popup_Base>();
        }

        public void RemovePopup(string name, int index)
        {
            if (!popupViewList.ContainsKey(name))
            {
                return;
            }

            if (popupViewList[name].Count < index)
            {
                return;
            }
            
            Destroy(popupViewList[name][index].gameObject);
        }
        
        public void RemovePopup(UI_Popup_Base popup)
        {
            Destroy(popup.gameObject);
        }
        #endregion
    }
}
