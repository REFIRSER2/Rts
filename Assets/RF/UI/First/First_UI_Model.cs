using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RF.UI.First
{
    public class First_UI_Model:MonoBehaviour
    {
        #region 기본 내장 함수
        private void Awake()
        {
            if (presenter == null)
            {
                presenter = this.GetComponent<First_UI>();
            }
        }

        private void OnEnable()
        {
            StartCoroutine("Loading");
        }

        #endregion
        
        #region 프레젠터
        private First_UI presenter;
        #endregion
        
        #region 로딩
        IEnumerator Loading()
        {
            yield return new WaitForSeconds(1F);
            var ao = SceneManager.LoadSceneAsync("Lobby");
            ao.allowSceneActivation = false;
        
            string dotStr = "";
            string loadStr = "Loading";
            
            float progress = 0f;
            while (!ao.isDone)
            {
                yield return new WaitForSeconds(0.3F);
            
                if (ao.progress < 0.9F)
                {
                    progress = Mathf.Clamp(progress+Time.deltaTime*4F, 0F, ao.progress);
                }
                else
                {
                    progress = Mathf.Clamp(progress+Time.deltaTime*4F, 0F, 1F);
                }

                if (dotStr.Length >= 3)
                {
                    dotStr = "";
                }
                dotStr = dotStr + ".";

                loadStr = "Loading";
                loadStr = loadStr + " " + dotStr;

                presenter.OnLoadThink(loadStr, progress);
            }
        }
        #endregion
    }
}
