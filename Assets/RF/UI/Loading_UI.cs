using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_UI : UI_Base
{
    [SerializeField] private TMP_Text loading_Text;

    [SerializeField] private Slider progress_Bar;
    [SerializeField] private TMP_Text progress_Text;
    
    private string loading_String = "Loading";
    private int gamemode = 0;
    
    [SerializeField] private Transform leftGroup;
    [SerializeField] private Transform rightGroup;

    public async void AddPlayer(int team, ulong id)
    {
        switch (team)
        {
            case 0:
                var left = UI_Manager.Instance.CreateUIItem<LoadingLeft_Item>();
                left.transform.SetParent(leftGroup);
                Friend lFriend = new Friend(id);
                left.SetNickName(lFriend.Name);
                
                var lImage = await SteamFriends.GetLargeAvatarAsync((ulong)Convert.ToInt64(id));
                if (lImage != null)
                {
                    var texture = SteamManager.Instance.GetProfileIcon(lImage.Value);
                    left.SetProfile(texture);
                }
                break;
            case 1:
                var right = UI_Manager.Instance.CreateUIItem<LoadingRight_Item>();
                right.transform.SetParent(rightGroup);
                Friend rFriend = new Friend(id);
                right.SetNickName(rFriend.Name);
                
                var rImage = await SteamFriends.GetLargeAvatarAsync((ulong)Convert.ToInt64(id));
                if (rImage != null)
                {
                    var texture = SteamManager.Instance.GetProfileIcon(rImage.Value);
                    right.SetProfile(texture);
                }
                break;
        }
    }

    public void SetGameMode(int num)
    {
        gamemode = num;
    }

    public override void On_Open()
    {
        base.On_Open();

        loading_Text.text = loading_String;
        
        StartCoroutine("loading_Anim");
    }
    
    IEnumerator loading_Anim()
    {
        yield return new WaitForSeconds(1F);
        var ao = SceneManager.LoadSceneAsync("Map0" + (gamemode+1));
        ao.allowSceneActivation = false;
        
        string dot_String = ""; 
        float progress = 0f;
        while (!ao.isDone)
        {
            yield return new WaitForSeconds(0.3F);
            
            if (ao.progress < 0.9F)
            {
                progress = Mathf.Clamp(progress+Time.deltaTime*4F, 0F, ao.progress);
            }
            else
            {
                progress = Mathf.Clamp(progress+Time.deltaTime*4F, 0F, 1F);
                if (progress >= 1F)
                {
                    ao.allowSceneActivation = false;
                    this.Remove();
                }
            }

            progress_Bar.value = progress;
            progress_Text.text = Mathf.Ceil(progress * 100) + "%";

            if (dot_String.Length >= 3)
            {
                dot_String = "";
            }
            dot_String = dot_String + ".";

            loading_Text.text = loading_String + " " + dot_String;
        }
    }
}
