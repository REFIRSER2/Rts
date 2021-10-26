using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Item : UIItem_Base
{
    [SerializeField] private Text itemTitle_Text;
    [SerializeField] private Text price_Text;

    public void SetName(string name)
    {
        itemTitle_Text.text = name;
    }

    public void SetPrice(string price)
    {
        price_Text.text = price;
    }
}
