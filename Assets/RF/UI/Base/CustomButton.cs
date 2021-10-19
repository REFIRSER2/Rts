using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    #region Unity General Funcs
    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }
    #endregion
}
