using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingRight_Item : UIItem_Base
{
    [SerializeField] private RawImage profile_Icon;
    [SerializeField] private TMP_Text nickName_Text;

    public void SetProfile(Texture2D texture)
    {
        profile_Icon.texture = texture;
    }

    public void SetNickName(string nick)
    {
        nickName_Text.text = nick;
    }
}
