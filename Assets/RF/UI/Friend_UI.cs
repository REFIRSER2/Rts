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
    #region 변수
    [SerializeField] private Transform content;

    [SerializeField] private List<RawImage> squad_Profiles = new List<RawImage>();
    [SerializeField] private float refresh_Time = 5F;

    private Dictionary<SteamId, Friend_Item> friendItems = new Dictionary<SteamId, Friend_Item>();
    #endregion
    
    #region 오픈/클로즈 오버라이드
    public override void On_Open()
    {
        base.On_Open();
    }

    public override void On_Close()
    {
        base.On_Close();
    }
    #endregion

    #region 친구 목록창 새로고침
    private void AddFriend(Friend friend, EnumData.Status status)
    {
        if (friendItems.ContainsKey(friend.Id))
        {
            friendItems[friend.Id].SetFriend(friend);
            friendItems[friend.Id].SetProfile(friend.Id);
            friendItems[friend.Id].SetNickname(friend.Name);
            friendItems[friend.Id].SetStatus(status);
            return;
        }

        var item = UI_Manager.Instance.CreateUIItem<Friend_Item>();

        item.SetFriend(friend);
        item.SetProfile(friend.Id);
        item.SetNickname(friend.Name);
        item.SetStatus(status);

        item.transform.SetParent(content, false);
        
        friendItems.Add(friend.Id, item);
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
    
    IEnumerator RefreshTimer()
    {
        yield return new WaitForSeconds(refresh_Time);
        StartCoroutine("RefreshTimer");
        Refresh();
    }
    #endregion

    #region 유니티 기본 내장 함수
    private void Awake()
    {
        Refresh();
        StartCoroutine("RefreshTimer");
    }
    #endregion


}
