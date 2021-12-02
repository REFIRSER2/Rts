using System;
using System.Collections;
using System.Collections.Generic;
using RF.Photon;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_UI : UI_Base
{
    #region 변수
    [SerializeField] private TMP_Text notice_Text;
    [SerializeField] private GameObject roomList_OBJ;
    [SerializeField] private GameObject shop_OBJ;
    [SerializeField] private GameObject inv_OBJ;
    [SerializeField] private GameObject option_OBJ;

    [SerializeField] private GameObject play_Btn;
    [SerializeField] private GameObject leave_Btn;
    
    [SerializeField] private TMP_Dropdown mode_Select;
    
    [SerializeField] private RawImage profile_Icon;
    
    [SerializeField] private List<RawImage> party_Profiles = new List<RawImage>();
    #endregion

    #region 공지
    private float noticeResetTime = 0F;
    public void SetNotice(string str)
    {
        notice_Text.text = str;
    }

    #endregion


    #region 파티 시스템

    public void onLeaveParty()
    {
        SteamManager.Instance.LeaveLobby();
        //LobbyManager.Instance.LeaveParty(SteamManager.Instance.steamID.ToString());
    }
    
    public async void RefreshParty()
    {
        int index = 0;
        
        foreach (var profile in party_Profiles)
        {
            profile.texture = null;
            profile.gameObject.SetActive(false);
        }
        
        foreach (var item in SteamManager.Instance.GetLobbyMembers())
        {
            var image = await SteamFriends.GetLargeAvatarAsync(item.Key);
            if (image != null)
            {
                var texture = SteamManager.Instance.GetProfileIcon(image.Value);
                party_Profiles[index].texture = texture;
                party_Profiles[index].gameObject.SetActive(true);
            }

            index++;
        }
    }
    
    private async void RefreshProfile()
    {
        var image = await SteamFriends.GetLargeAvatarAsync(SteamManager.Instance.steamID);

        if (image != null)
        {
            var texture = SteamManager.Instance.GetProfileIcon(image.Value);
            profile_Icon.texture = texture;
            profile_Icon.gameObject.SetActive(true);
        }   
    }
    #endregion
    
    #region 매치메이킹
    public void onStart()
    {
        switch (LobbyManager.Instance.GetGameMode())
        {
            case 3:
                if (!roomList_OBJ.activeSelf)
                {
                    roomList_OBJ.SetActive(true);
                }   
                break;
        }
    }
    
    public void onPlay()
    {
        play_Btn.SetActive(false);
        leave_Btn.SetActive(true);
        
        PhotonManager.Instance.CreateQuickRoom(MainManager.Instance.userInfo.rank,LobbyManager.Instance.GetGameMode());
        LobbyManager.Instance.RequestMemberFollow();
        
        //StartCoroutine("FindQuickMatch");
        //matchTimer_Text.gameObject.SetActive(true);
        //LobbyManager.Instance.FindQuickMatch();
        //LobbyManager.Instance.FindQuickMatch();
        //ServerManager.Instance.FindQuickMatch(gameMode, findQuickMatchAction);
        //
        //
    }
    
    public void onLeave()
    {
        //LobbyManager.Instance.LeaveQuickMatch();
        //ServerManager.Instance.LeaveQuickMatch();
        play_Btn.SetActive(true);
        leave_Btn.SetActive(false);
        
        //StopCoroutine("FindQuickMatch");
    }

    public void onSelectMode(int mode)
    {
        LobbyManager.Instance.SetGameMode(mode);
    }
    #endregion
    
    #region 버튼 이벤트
    public void onShop()
    {
        shop_OBJ.SetActive(true);
    }

    public void onInventory()
    {
        
    }

    public void onFriend()
    {
        
    }
    
    public void onReport()
    {
        var report = UI_Manager.Instance.CreatePopup<Report_Popup>();
    }

    public void onOption()
    {
        
    }

    public void onHome()
    {
        if (roomList_OBJ != null)
        {
            roomList_OBJ.SetActive(false); 
        }
        
        if (shop_OBJ != null)
        {
            shop_OBJ.SetActive(false); 
        }

        if (inv_OBJ != null)
        {
            inv_OBJ.SetActive(false);   
        }

        if (option_OBJ != null)
        {
            option_OBJ.SetActive(false);   
        }
    }

    public void onExit()
    {
        UI_Manager.Instance.CreatePopup<Exit_Popup>();
    }
    #endregion

    #region 유니티 기본 내장 함수
    private void Awake()
    {
        mode_Select.onValueChanged.AddListener(onSelectMode);
        
        /*
        Action findQuickMatchAction;
        Action leaveQuickMatchAction;
        Action cancelQuickMatchAction;
        
        findQuickMatchAction = () =>
        {
            play_Btn.SetActive(false); 
            leave_Btn.SetActive(true);
        };
        leaveQuickMatchAction = () =>
        {
            play_Btn.SetActive(true);
            leave_Btn.SetActive(false);
        };
        cancelQuickMatchAction = () =>
        {
            play_Btn.SetActive(true);
            leave_Btn.SetActive(false);   
        };
        
        LobbyManager.Instance.findQuickMatchAction = findQuickMatchAction;
        LobbyManager.Instance.leaveQuickMatchAction = leaveQuickMatchAction;
        LobbyManager.Instance.cancelQuickMatchAction = cancelQuickMatchAction;*/
        
        RefreshParty();
        RefreshProfile();
    }

    private void Update()
    {
        if (noticeResetTime > 0F)
        {
            if (notice_Text.text != "")
            {
                if (notice_Text.color.a > 0)
                {
                    notice_Text.color = new Color(notice_Text.color.r, notice_Text.color.g, notice_Text.color.b,
                        Mathf.Lerp(notice_Text.color.a, 0, Time.deltaTime));
                }
                else
                {
                    SetNotice("");
                }
            }   
        }
        else
        {
            noticeResetTime -= Time.deltaTime;
        }
    }
    #endregion
}
