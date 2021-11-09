
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public enum ItemType
{
    Part,
    Accessary
}
public class InventoryManager : MonoBehaviour
{
    #region 싱글톤
    public static InventoryManager Instance;
    #endregion
    
    #region 인벤토리
    private Dictionary<ItemType, List<string>> inventory = new Dictionary<ItemType, List<string>>();

    public List<string> GetInventory(ItemType type)
    {
        return inventory[type];
    }

    public void AddItem(ItemData item, int amount)
    {
        if (!inventory.ContainsKey(item.itemType))
        {
            inventory.Add(item.itemType, new List<string>());

            for (int i = 0; i < amount; i++)
            {
                inventory[item.itemType].Add(item.uniqueID); 
            }
            
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            inventory[item.itemType].Add(item.uniqueID);   
        }
    }

    public void RemoveItem(ItemData item, int index)
    {
        if (!inventory.ContainsKey(item.itemType) || (inventory[item.itemType].Count - 1) < index)
        {
            return;
        }

        inventory[item.itemType][index].Remove(index);
    }
    #endregion

    #region 유니티 일반 함수
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
