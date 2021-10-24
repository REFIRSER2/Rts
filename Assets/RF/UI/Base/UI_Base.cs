using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    #region UI

    //public string name;
    public virtual void On_Open()
    {
        
    }

    public virtual void On_Close()
    {
        
    }

    public void Remove()
    {
        UI_Manager.Instance.RemoveUI(this);
    }
    #endregion
    
    #region Unity General Funcs
    private void OnEnable()
    {
        On_Open();
    }

    private void OnDisable()
    { 
        On_Close();
    }
    #endregion
}
