using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_UI : UI_Base
{
    [SerializeField] private TMP_Text notice_Text;
    [SerializeField] private GameObject shop_OBJ;
    [SerializeField] private GameObject inv_OBJ;
    [SerializeField] private GameObject option_OBJ;

    [SerializeField] private RawImage profile_Icon;
    
    [SerializeField] private List<RawImage> party_Profiles = new List<RawImage>();
    
    private float noticeResetTime = 0F;
    public void SetNotice(string str)
    {
        notice_Text.text = str;
    }

    public void RefreshParty()
    {
        int index = 0;
        
        foreach (var profile in party_Profiles)
        {
            profile.texture = null;
            profile.gameObject.SetActive(false);
        }
        
        foreach (var party in SteamManager.Instance.GetPartyMembers())
        {
            party_Profiles[index].texture = party.Value.profile;
            party_Profiles[index].gameObject.SetActive(true);
            
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

    private void Awake()
    {
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
}
