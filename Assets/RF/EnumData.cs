using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    public enum UIType
    {
        First,
        Login,
    }
    
    public enum PopupType
    {
        Sign,
        FindAccount,
        ResetPassword,
        ResetPassword_Empty,
        Password_Empty,
    }
    
    public enum ItemType
    {
        PartWeapon = 0,
        PartBody = 1,
        PartLeg = 2,
        PartArm = 3,
        Accessary = 4,
    }

    public enum Currently
    {
        Gold = 0,
        Cash = 1,
    }
}
