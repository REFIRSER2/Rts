using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Base : MonoBehaviour
{
    #region UI
    public virtual void On_Open()
    {
        
    }

    public virtual void On_Close()
    {
        
    }
    #endregion
    
    #region Unity General Funcs
    private void Awake()
    {

    }
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
