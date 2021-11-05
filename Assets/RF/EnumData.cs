using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumData
{
    public enum ErrorCode
    {
        BadUnauthorizedException = 401,
        
    }

    public enum IngameChat
    {
        Public,
        Private,
    }

    public enum LobbyChat
    {
        Normal,
        Room,
    }

    public enum ItemType
    {
        PartWeapon = 0,
        PartBody = 1,
        PartLeg = 2,
        PartArm = 3,
        Accessary = 4,
        Ship = 5,
    }

    public enum ItemCat
    {
        Part,
        Accessary,
        Ship,
    }

    public enum Currently
    {
        Gold = 0,
        Cash = 1,
    }

    public enum Status
    {
        Online,
        IsPlay,
    }
}
