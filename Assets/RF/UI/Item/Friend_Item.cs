using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class Friend_Item : UIItem_Base
{
    [SerializeField] private Text nickname_Text;
    [SerializeField] private Text status_Text;

    [SerializeField] private RawImage profile_Icon;
    
    [SerializeField] private Color online_Color;
    [SerializeField] private Color isPlay_Color;

    private EnumData.Status status;

    private Friend friend;

    public void SetFriend(Friend fr)
    {
        friend = fr;
    }
    
    public void SetNickname(string nickname)
    {
        nickname_Text.text = nickname;
    }
    
    public void SetStatus(EnumData.Status s)
    {
        status = s;

        switch (status)
        {
            case EnumData.Status.Online:
                status_Text.text = "Online";
                status_Text.color = online_Color;
                break;
            case EnumData.Status.IsPlay:
                status_Text.text = "IsPlay";
                status_Text.color = isPlay_Color;
                break;
        }
    }

    public async void SetProfile(SteamId id)
    {
        var image = await SteamFriends.GetLargeAvatarAsync(id);

        if (image != null)
        {
            var texture = SteamManager.Instance.GetProfileIcon(image.Value);
            profile_Icon.texture = texture;
        }
    }

    public void onClick()
    {
        friend.InviteToGame( SteamManager.Instance.Nickname + " 님이 게임에 초대하였습니다.");
        friend.SendMessage(SteamManager.Instance.Nickname + " 님이 게임에 초대하였습니다.");
    }

    private void Awake()
    {
        
    }
}
