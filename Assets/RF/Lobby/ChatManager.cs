using System;
using RF.UI;
using RF.UI.Chat;
using UnityEngine;
using SocketManager = BestHTTP.SocketIO3.SocketManager;

namespace RF.Lobby
{
    public enum ChatChannel
    {
        Lobby = 0,
        Public = 1,
        Private = 2,
    }
    
    public class ChatData
    {
        public string id;
        public string nickName;
        public string text;

        public string roomName = "Lobby";

        public int channel = (int)ChatChannel.Lobby;
    }
    
    public class ChatManager : MonoBehaviour
    {
        #region 싱글톤
        public static ChatManager Instance;
        #endregion
        
        #region 서버
        private SocketManager server;

        private void Setup()
        {
            server = new SocketManager(new Uri("http://54.180.191.145:27002"));
            
            server.Socket.On<ChatData>("chat", (data) =>
            {
                OnChatReceive(data);
            });
        }
        #endregion
        
        #region 채팅

        public void ChatSend(ChatData data)
        {
            server.Socket.Emit("chat", data);
        }
        
        private void OnChatReceive(ChatData data)
        {
            var ui = UI_Manager.Instance.GetUIView("Chat_UI") as Chat_UI;
            ui.GetModel().OnChatReceive(data.id, data.nickName, data.text, data.channel);
        }
        #endregion
        
        #region 기본 내장 함수
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            Setup();
        }
        
        private void Start(){
        
        }
        #endregion
    }
}
