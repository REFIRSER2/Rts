using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    #region Item
    public string uniqueID;
    public ItemType itemType;
    #endregion
    
    #region Part
    public int level = 1;
    public int watt;
    public int weight;
    public int speed;
    public int health;
    public int sight;
    public int damage;
    public int dps;
    public int reach;
    #endregion
}
