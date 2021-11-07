using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sign_Popup : Popup_Base
{
    [SerializeField] private List<InputField> inputField_List;
    
    [SerializeField] private InputField nickInput;
    [SerializeField] private InputField emailInput;
    [SerializeField] private InputField pwdInput;

    public override void On_Open()
    {
        base.On_Open();
    }

    public override void On_Close()
    {
        base.On_Close();
    }

    public void onApply()
    {
        if (emailInput.text != "" && pwdInput.text != "")
        {
            MainManager.Instance.Sign(this, nickInput.text, pwdInput.text, emailInput.text);  
        }
        else
        {
            
        }
    }

    public void onSign()
    {
        MainManager.Instance.
    }

    private void Update()
    {
        int index = -1;

        bool check = false;
        for (int i = 0; i < inputField_List.Count; i++)
        {
            if (inputField_List[i].isFocused)
            {
                index = i;
                check = true;
                break;
            }
        }

        if (check && Input.GetKeyDown(KeyCode.Tab))
            {
                if (index < inputField_List.Count - 1)
                {
                    index++;
                    inputField_List[index].Select();
                }
                else
                {
                    index = 0;
                    inputField_List[index].Select();
                }
            }
    }
}
