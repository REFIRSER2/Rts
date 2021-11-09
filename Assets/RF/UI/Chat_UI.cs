using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat_UI : UI_Base
{
    [SerializeField] private TMP_Text chatLog;
    [SerializeField] private ScrollRect scrollRect;
    
    [SerializeField] private TMP_InputField chatInput;

    [SerializeField] private List<string> inChannels;
    [SerializeField] private List<string> lobbyChannels;
    
    private int channel = 0;
    
    public void OnReceiveMessage(string nick, string message, string channel)
    {
        chatLog.text += "[" + channel + "]" + nick + " : " + message + "\n"; 

        StartCoroutine("DownScroll");
    }

    public void OnNotifyMessage(string subject, string message)
    {
        chatLog.text += "[" + subject + "]" + "<color=yellow>" + message + "</color>\n";
        StartCoroutine("DownScroll");
    }
    
    public void OnGlobalMessage(string nick, string message)
    {
        chatLog.text += "[GM]" + nick + " : <color=yellow>" + message + "</color>\n";
        StartCoroutine("DownScroll");
    }

    public void OnBannedMessage()
    {
        chatLog.text += "[경고]" + "<color=red>도배로 인해 채팅이 일시적으로 차단됩니다.</color>\n";
        StartCoroutine("DownScroll");
    }

    public void SendSystemMessage(string msg)
    {
        chatLog.text += "<color=lightblue>[시스템] : " + msg + "</color>\n";
        StartCoroutine("DownScroll");
    }

    IEnumerator DownScroll()
    {
        yield return new WaitForSeconds(0.1F);
        scrollRect.verticalNormalizedPosition = 0.0F;
    }

    public void ChangeChannel()
    {
        if (channel < 3)
        {
            channel += 1;
        }
        else
        {
            channel = 0;
        }
    }

    public void onSelect()
    {
        scrollRect.gameObject.SetActive(true);
    }

    public void onEndEdit()
    {

    }

    private void Awake()
    {
        scrollRect.gameObject.SetActive(false);

        channel = 0;
        /*
        if (SteamManager.Instance.IsStartGame())
        {
            SendSystemMessage(inChannels[channel] + " 채널에 참가하였습니다.");  
        }
        else
        {
            SendSystemMessage(lobbyChannels[channel] + " 채널에 참가하였습니다.");
        }*/
    }
    
    private void Update()
    {
        if (chatInput.IsActive())
        {
            if (chatInput.isFocused && Input.GetKeyDown(KeyCode.Tab))
            {
                /*
                if (SteamManager.Instance.IsStartGame())
                {
                    SendSystemMessage(inChannels[channel] + " 채널에 참가하였습니다.");  
                }
                else
                {
                    SendSystemMessage(lobbyChannels[channel] + " 채널에 참가하였습니다.");
                }*/

                chatLog.gameObject.SetActive(true);
                
            }
            
            if (Input.GetKey(KeyCode.Return) && chatInput.text != "")
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
                    if (SteamManager.Instance.IsStartGame())
                    {
                        //ChatManager.Instance.SendChat(inChannels[channel],chatInput.text);
                    }
                    else
                    {
                        //ChatManager.Instance.SendChat(lobbyChannels[channel],chatInput.text);
                    }
                    
                }

                chatInput.ActivateInputField();
                
                chatInput.text = "";
            }
        }
    }
}
