using System;
using RF.Lobby;
using TMPro;
using UnityEngine;

namespace RF.UI.Chat
{
    public class Chat_UI_Model : MonoBehaviour
    {
        #region 프레젠터
        private Chat_UI presenter;

        [SerializeField] private TMP_InputField chatInput;

        public void OnChatSend(string text)
        {
            presenter.OnChatSend(text);
        }
        
        public void OnChatReceive(string id, string nick, string msg, int channel)
        {
            presenter.OnChatReceive(id, nick, msg, channel);
        }
        #endregion    
        
        #region 기본 내장 함수
        private void Awake()
        {
            if (presenter == null)
            {
                presenter = this.GetComponent<Chat_UI>();
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (chatInput.text != "")
            {
                if (Input.GetKey(KeyCode.Return))
                {
                    OnChatSend(chatInput.text);
                }
            }
        }

        #endregion
    }
}
