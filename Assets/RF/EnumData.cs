using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    public enum ErrorCode
    {
        BadUnauthorizedException = 401,
        
    }

    public enum ChatChannel
    {
        Global,
        Local,
        Whisper,
        Notify,
        Party,
        Guild,
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
