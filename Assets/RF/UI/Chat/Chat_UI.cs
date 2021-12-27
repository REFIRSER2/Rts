using System;
using Photon.Pun;
using RF.Account;
using RF.Lobby;
using RF.Steam;
using TMPro;
using UnityEngine;

namespace RF.UI.Chat
{
    public class Chat_UI : UI_Base
    {
        #region 기본 내장 함수
        private void Start()
        {
            UI_Manager.Instance.AddUIView("Chat_UI", this);

            if (model == null)
            {
                model = this.GetComponent<Chat_UI_Model>();    
            }
        }
        #endregion

        #region 모델
        private Chat_UI_Model model;

        public Chat_UI_Model GetModel()
        {
            return model;
        }
        #endregion
        
        #region 프레젠터
        [SerializeField] private ChatChannel channel = ChatChannel.Lobby;
        [SerializeField] private TMP_InputField chatInput;

        public void OnChatSend(string text)
        {
            ChatData data = new ChatData();
            data.channel = (int)channel;
            data.text = text;
            data.nickName = AccountManager.Instance.user.nickName;
            data.id = SteamManager.Instance.GetAccount().steamID.ToString();

            if (channel != ChatChannel.Lobby)
            {
                data.roomName = PhotonNetwork.CurrentRoom.Name;
            }
            
            ChatManager.Instance.ChatSend(data);
            
            chatInput.text = "";
        }
        
        public void OnChatReceive(string id, string nick, string msg, int channel)
        {
            Debug.Log("sender id : " + id + " nick : " + nick + " text : " + msg + " channel : " + channel);
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
