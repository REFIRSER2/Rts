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
        private Dictionary<string, UI_Popup_Base> popupViewList = new Dictionary<string, UI_Popup_Base>();

        public UI_Popup_Base CreatePopupView(string name)
        {
            Transform layer = GameObject.Find("Popup_Layer").transform;
            GameObject popup = Instantiate(Resources.Load("Prefabs/UI/Popup/" + string.Format("{0}", name)), layer) as GameObject;
            return popup.GetComponent<UI_Popup_Base>();
        }

        public void RemovePopup(UI_Popup_Base popup)
        {
            Debug.Log("remove popup");
            Destroy(popup.gameObject);
        }
        #endregion
    }
}
