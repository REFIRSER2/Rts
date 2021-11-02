using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    private Chat_UI chatUI;

    public Chat_UI GetChatUI()
    {
        return chatUI;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        chatUI = FindObjectOfType<Chat_UI>();
        HandlerInit();
    }
    

    private void Update()
    {
        //Backend.Chat.Poll();
    }

    private void HandlerInit()
    {
        /*
        Backend.Chat.OnJoinChannel = (args =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                if (!args.Session.IsRemote)
                {
                    chatUI.OnNotifyMessage("알림", "채팅 채널에 접속하였습니다."); 
                }


                var getChannel = Backend.Chat.GetGroupChannelList("길드채널");
                if (getChannel.IsSuccess())
                {
                    var channels = getChannel.Rows();
                    
                }
            }
            else
            {
                if (!args.Session.IsRemote)
                {
                    chatUI.OnNotifyMessage("알림", "채팅 서버에 연결할 수 없습니다. 채팅 서버 연결 재시도 중 ..");

                    StartCoroutine(RetryJoin());

                }
            }
        });
        
        Backend.Chat.OnNotification = (args =>
        {
            if (SceneManager.GetActiveScene().name == "Lobby")
            {
                FindObjectOfType<MainMenu_UI>().SetNotice(args.Message);
                chatUI.OnNotifyMessage(args.Subject, args.Message);
                // FindObjectOfType<Chat_UI>().OnReceiveNotice(args.Subject, args.Message);
            }
        });

        Backend.Chat.OnChat = (args =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                chatUI.OnReceiveMessage(false, !args.From.IsRemote, args.From.NickName,args.Message);
            }
            else if (args.ErrInfo.Category == ErrorCode.BannedChat)
            {
                chatUI.OnBannedMessage();
            }
        });
        
        Backend.Chat.OnGuildChat = (args =>
        {
            
        });

        Backend.Chat.OnGlobalChat = (args =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                chatUI.OnGlobalMessage(args.From.NickName, args.Message);
            }
            else
            {
                chatUI.OnNotifyMessage("알림", "공지를 받을 수 없습니다.");
            }
        });

        Backend.Chat.OnWhisper = (args =>
        {
            if (args.ErrInfo == ErrorInfo.Success)
            {
                Debug.Log(args.From.IsRemote);
                Debug.Log(args.To.IsRemote);
                if (!args.From.IsRemote)
                {
                    chatUI.OnReceiveMessage(true, true, args.To.NickName,args.Message);  
                }
                else
                {
                    chatUI.OnReceiveMessage(true, false, args.From.NickName,args.Message);  
                }
                
            }
            else if(args.ErrInfo.Category == ErrorCode.BannedChat)
            {
                
            }
        });*/
    }

    /*
    IEnumerator RetryJoin()
    {
        yield return new WaitForSeconds(5F);
        BackendManager.Instance.JoinChannel();
    }*/

    public void NotifyMessage(string msg)
    {
        chatUI.OnNotifyMessage("공지",msg);
    }

    public void SendSystemMessage(string msg)
    {
        chatUI.SendSystemMessage(msg);
    }
    
    public void SendMessage(string channel, string msg)
    {
        ChatData data = new ChatData();
        data.nickName = ServerManager.Instance.nickName;
        data.channel = channel;
        data.message = msg;
        data.lobbyID = SteamManager.Instance.currentLobby.Id.ToString();

        ServerManager.Instance.SendChat(data);
        //Backend.Chat.ChatToChannel(channel, msg);
    }
}
