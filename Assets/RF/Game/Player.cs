using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int team = -1;

    public void SetTeam(int num)
    {
        team = num;
    }

    public int GetTeam()
    {
        return team;
    }
}
