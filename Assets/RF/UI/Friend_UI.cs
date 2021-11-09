using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.UI;

public class Friend_UI : UI_Base
{
    [SerializeField] private Transform content;

    [SerializeField] private List<RawImage> squad_Profiles = new List<RawImage>();
    
    public override void On_Open()
    {
        base.On_Open();
    }

    public override void On_Close()
    {
        base.On_Close();
    }

    private void AddFriend(Friend friend, EnumData.Status status)
    {
        var item = UI_Manager.Instance.CreateUIItem<Friend_Item>();

        item.SetFriend(friend);
        item.SetProfile(friend.Id);
        item.SetNickname(friend.Name);
        item.SetStatus(status);
        
        item.transform.SetParent(content, false);
    }
    
    private void Refresh()
    {
        IEnumerable<Friend> friends = SteamManager.Instance.GetFriends();

        foreach (var friend in friends)
        {
            if (!friend.IsOnline && !friend.IsPlayingThisGame)
            {
                continue;
            }

            if (friend.IsPlayingThisGame)
            {
                AddFriend(friend, EnumData.Status.Online);
                continue;
            }
        }
    }

    private void Awake()
    {
        Refresh();
    }
}
