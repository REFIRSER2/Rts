using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    #region Singletone
    public static CommandManager Instance;
    #endregion
    
    #region Command

    public Dictionary<string, Action<string>> cmdList = new Dictionary<string, Action<string>>();
    
    public Action<string> whisperCMD = (args) =>
    {
        string[] table = args.Split(' ');

        string nickName = "";
        string message = "";
        
        if (table[0] != "")
        {
            nickName = table[0];
        }

        if (table[1] != "")
        {
            message = args.Replace(nickName, "");
        }

        if (nickName != "" && message != "")
        {
            Backend.Chat.Whisper(nickName, message);
        }
        else
        {
            ChatManager.Instance.GetChatUI().OnNotifyMessage("시스템", "상대방의 닉네임 혹은 보낼 메시지를 제대로 입력해주세요.");
        }
    };

    public Action<string> muteCMD = (args) =>
    {
        string[] table = args.Split(' ');

        string nickName = "";

        if (table[0] != "")
        {
            nickName = table[0];
        }

        if (nickName != "")
        {
            Backend.Chat.BlockUser(args, callback =>
            {
                if (callback)
                {
                    ChatManager.Instance.GetChatUI().OnNotifyMessage("시스템", args + "님을 성공적으로 차단하였습니다.");
                }
                else
                {
                    ChatManager.Instance.GetChatUI().OnNotifyMessage("시스템", "오류로 인해 " + args + "님을 차단하지 못하였습니다.");
                }
            });    
        }
        else
        {
            ChatManager.Instance.GetChatUI().OnNotifyMessage("시스템", "차단하려는 상대방의 닉네임을 입력해주세요.");
        }
    };
    
    public Action<string> unMuteCMD = (args) =>
    {
        string[] table = args.Split(' ');

        string nickName = "";

        if (table[0] != "")
        {
            nickName = table[0];
        }

        if (nickName != "")
        {
            Backend.Chat.UnblockUser(nickName);    
            ChatManager.Instance.GetChatUI().OnNotifyMessage("시스템", nickName + "님의 차단을 성공적으로 해제하였습니다.");
        }
        else
        {
            ChatManager.Instance.GetChatUI().OnNotifyMessage("시스템", "차단하려는 상대방의 닉네임을 입력해주세요.");
        }
    };

    #endregion
    
    #region Unity General Funcs
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        cmdList.Add("/whisper", whisperCMD);
        cmdList.Add("/w", whisperCMD);
        cmdList.Add("/mute", muteCMD);
        cmdList.Add("/unmute", unMuteCMD);
    }
    #endregion
}
