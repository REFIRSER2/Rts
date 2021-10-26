using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd.Tcp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat_UI : UI_Base
{
    [SerializeField] private List<TMP_Text> chatLog_List;
    [SerializeField] private List<ScrollRect> scrollRect_List;
    
    [SerializeField] private TMP_InputField chatInput;

    private int channel = 0;
    
    public void OnReceiveMessage(bool isWhisper, bool isLocal, string nick, string message)
    {
        if (isWhisper)
        {
            if (isLocal)
            {
                chatLog_List[(int)EnumData.ChatChannel.Global].text += "<color=lightblue>" + nick + "님에게 보낸 귓속말 : " + message + "</color>\n";
                chatLog_List[(int)EnumData.ChatChannel.Whisper].text += "<color=lightblue>" + nick + "님에게 보낸 귓속말 : " + message + "</color>\n";
            }
            else
            {
                chatLog_List[(int)EnumData.ChatChannel.Global].text += "<color=lightblue>" + nick + "님의 귓속말 : " + message + "</color>\n";
                chatLog_List[(int)EnumData.ChatChannel.Whisper].text += "<color=lightblue>" + nick + "님의 귓속말 : " + message + "</color>\n";
            }
        }
        else
        {
            chatLog_List[(int)EnumData.ChatChannel.Global].text += nick + " : " + message + "\n"; 
            chatLog_List[(int)EnumData.ChatChannel.Local].text += nick + " : " + message + "\n"; 
        }
        

        StartCoroutine("DownScroll");
    }

    public void OnNotifyMessage(string subject, string message)
    {
        chatLog_List[(int)EnumData.ChatChannel.Global].text += "[" + subject + "]" + "<color=yellow>" + message + "</color>\n";
        chatLog_List[(int)EnumData.ChatChannel.Notify].text += "[" + subject + "]" + "<color=yellow>" + message + "</color>\n";
        StartCoroutine("DownScroll");
    }
    
    public void OnGlobalMessage(string nick, string message)
    {
        chatLog_List[(int)EnumData.ChatChannel.Global].text += "[GM]" + nick + " : <color=yellow>" + message + "</color>\n";
        chatLog_List[(int)EnumData.ChatChannel.Notify].text += "[GM]" + nick + " : <color=yellow>" + message + "</color>\n";
        StartCoroutine("DownScroll");
    }

    public void OnBannedMessage()
    {
        chatLog_List[(int)EnumData.ChatChannel.Global].text += "[경고]" + "<color=red>도배로 인해 채팅이 일시적으로 차단됩니다.</color>\n"; 
        chatLog_List[(int)EnumData.ChatChannel.Notify].text += "[경고]" + "<color=red>도배로 인해 채팅이 일시적으로 차단됩니다.</color>\n"; 
        StartCoroutine("DownScroll");
    }

    public void SendSystemMessage(string msg)
    {
        chatLog_List[(int)EnumData.ChatChannel.Global].text += "<color=lightblue>[시스템] : " + msg + "</color>\n";
        chatLog_List[(int)EnumData.ChatChannel.Notify].text += "<color=lightblue>[시스템] : " + msg + "</color>\n";
        StartCoroutine("DownScroll");
    }

    IEnumerator DownScroll()
    {
        yield return new WaitForSeconds(0.1F);
        scrollRect_List[(int) EnumData.ChatChannel.Global].verticalNormalizedPosition = 0.0F;
        scrollRect_List[(int) EnumData.ChatChannel.Local].verticalNormalizedPosition = 0.0F;
        scrollRect_List[(int) EnumData.ChatChannel.Guild].verticalNormalizedPosition = 0.0F;
        scrollRect_List[(int) EnumData.ChatChannel.Notify].verticalNormalizedPosition = 0.0F;
        scrollRect_List[(int) EnumData.ChatChannel.Party].verticalNormalizedPosition = 0.0F;
        scrollRect_List[(int) EnumData.ChatChannel.Whisper].verticalNormalizedPosition = 0.0F;
    }

    public void ChangeChannel()
    {
        if (channel < chatLog_List.Count - 1)
        {
            channel += 1;
        }
        else
        {
            channel = 0;
        }
        
        for (int i=0;i<chatLog_List.Count;i++)
        {
            if (i != channel)
            {
                chatLog_List[i].gameObject.SetActive(false);
                continue;
            }
            chatLog_List[i].gameObject.SetActive(true);
        }
    }

    public void onSelect()
    {
        scrollRect_List[channel].gameObject.SetActive(true);
    }

    public void onEndEdit()
    {

    }

    private void Awake()
    {
        for (int i=0;i<scrollRect_List.Count;i++)
        {
            scrollRect_List[i].gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (chatInput.IsActive())
        {
            if (chatInput.text == "")
            {
                return;
            }
            
            if (Input.GetKey(KeyCode.Return))
            {
                string findCmd = "";
                foreach (var cmd in CommandManager.Instance.cmdList)
                {
                    if (chatInput.text.Length >= cmd.Key.Length && chatInput.text.Substring(0, cmd.Key.Length) == cmd.Key)
                    {
                        findCmd = cmd.Key;
                        break;
                    }
                }

                if (findCmd != "")
                {
                    CommandManager.Instance.cmdList[findCmd].Invoke(chatInput.text.Replace(findCmd + " ", ""));
                }
                else
                {
                    ChatManager.Instance.SendMessage(ChannelType.Public,chatInput.text);
                }
                
                chatInput.text = "";
            }
        }
    }
}
