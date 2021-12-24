using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RF.UI.First
{
    public class First_UI : UI_Base
    {
        #region 기본 내장 함수
        private void Start()
        {
            UI_Manager.Instance.AddUIView("First_UI", this);
            UI_Manager.Instance.SetActiveUI("First_UI", false);
            Debug.Log("등록");
        }
        #endregion
        
        #region  프레젠터
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private Slider progressBar;
        
        public void OnLoadThink(string loadText, float progress)
        {
            loadingText.text = loadText;
        
            progressText.text = Mathf.Ceil(progress * 100) + "%";
            progressBar.value = progress;

            if (progress >= 1F)
            {
                UI_Manager.Instance.SetActiveUI("First_UI", false);
                
                UI_Manager.Instance.SetActiveUI("Login_UI", true);
            }
        }
        #endregion

        #region 오버라이드
        public override void On_Open()
        {
            base.On_Open();
        }

        public override void On_Close()
        {
            base.On_Close();
        }

        public override void On_Refresh()
        {
            base.On_Refresh();
        }
        #endregion
    }
}
