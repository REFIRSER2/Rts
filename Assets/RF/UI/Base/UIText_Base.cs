using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIText_Base : TextMeshProUGUI
{
    [SerializeField] public bool disable_WordWrap = false;

    public override string text
    {
        get
        {
            return base.text;
        }
        set
        {
            if (disable_WordWrap)
            {
                string rep = value.Replace(' ', '\u00A0');
                base.text = rep;
                return;
            }

            base.text = value;
        }
    }
}
