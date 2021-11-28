using System.Collections.Generic;
using UnityEngine;

namespace RF.Debug
{
    public class DebugManager : MonoBehaviour
    {
        #region 싱글톤
        public static DebugManager Instance;
        #endregion
    
        #region 디버그
        [SerializeField] private bool isDebug = true;
        [SerializeField] private Dictionary<string, bool> debugTable = new Dictionary<string, bool>();

        public void Debug(string key, string str)
        {
            if (!CanDebug(key))
            {
                return;
            }
        
            UnityEngine.Debug.Log("KEY : " + key + " VALUE : " + str);
        }
    
        private bool CanDebug(string str)
        {
            return debugTable.ContainsKey(str) && debugTable[str];
        }
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
    }
}
