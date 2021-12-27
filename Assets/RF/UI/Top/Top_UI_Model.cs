using System;
using RF.Lobby;
using TMPro;
using UnityEngine;

namespace RF.UI.Top
{
    public class Top_UI_Model : MonoBehaviour
    {
        #region 프레젠터
        private Top_UI presenter;

        [SerializeField] private TMP_Dropdown modeSelector;

        public void OnRefreshProfile()
        {
            presenter.OnRefreshProfile();    
        }

        public void OnRefreshGoods()
        {
            presenter.OnRefreshGoods();
        }
        
        public void OnTryPlay()
        {
            presenter.OnTryPlay();
        }

        public void OnTryLeave()
        {
            presenter.OnTryLeave();
        }

        public void OnSelectMode(int num)
        {
            presenter.OnSelectMode(num);
        }

        public void OnStartFindQuickMatch()
        {
            presenter.OnStartFindQuickMatch();
        }
        
        public void OnLeaveFindQuickMatch()
        {
            presenter.OnLeaveFindQuickMatch();
        }
        #endregion
        
        #region 기본 내장 함수
        private void Awake()
        {
            if (presenter == null)
            {
                presenter = this.GetComponent<Top_UI>();
            }
            
            modeSelector.onValueChanged.AddListener((mode) =>
            {
                OnSelectMode(mode);
            });
        }

        private void Start()
        {
            
        }

        #endregion
    }
}
