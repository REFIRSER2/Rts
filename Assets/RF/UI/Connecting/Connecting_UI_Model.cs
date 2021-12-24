using System.Collections;
using RF.UI.Popup;
using UnityEngine;

namespace RF.UI.Connecting
{
    public class Connecting_UI_Model:MonoBehaviour
    {
        #region 기본 내장 함수

        private void Awake()
        {
            if (presenter == null)
            {
                presenter = this.GetComponent<Connecting_UI>();
            }
        }

        private void OnEnable()
        {
            StartCoroutine("TimeOut");
        }
        #endregion
        
        #region 프레젠터
        private Connecting_UI presenter;
        
        public void OnConnected()
        {
            presenter.OnConnected();
        }
        #endregion
        
        #region 타임아웃
        IEnumerator TimeOut()
        {
            yield return new WaitForSeconds(30F);
            
            presenter.OnTimeOut();
        }
        #endregion
    }
}
