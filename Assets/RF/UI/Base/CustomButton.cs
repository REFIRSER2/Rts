using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onClick;

    [SerializeField] private bool isChangeImage = false;
    
    [SerializeField] private Image normalBackground;
    [SerializeField] private Image highlightBackground;
    [SerializeField] private Image selectedBackground;
    [SerializeField] private Image disabledBackground;
    
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
        if (normalBackground != null)
        {
            if (normalBackground.gameObject.activeSelf)
            {
                normalBackground.gameObject.SetActive(false);
            }   
        }

        if (selectedBackground != null)
        {
            if (selectedBackground.gameObject.activeSelf)
            {
                selectedBackground.gameObject.SetActive(false);
            }    
        }


        if (disabledBackground != null)
        {
            if (disabledBackground.gameObject.activeSelf)
            {
                disabledBackground.gameObject.SetActive(false);
            }   
        }

        if (highlightBackground != null)
        {
            if (!highlightBackground.gameObject.activeSelf)
            {
                highlightBackground.gameObject.SetActive(true);  
            }    
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (normalBackground != null)
        {
            if (!normalBackground.gameObject.activeSelf)
            {
                normalBackground.gameObject.SetActive(true);
            }   
        }

        if (selectedBackground != null)
        {
            if (selectedBackground.gameObject.activeSelf)
            {
                selectedBackground.gameObject.SetActive(false);
            }    
        }

        if (disabledBackground != null)
        {
            if (disabledBackground.gameObject.activeSelf)
            {
                disabledBackground.gameObject.SetActive(false);
            }    
        }

        if (highlightBackground != null)
        {
            if (highlightBackground.gameObject.activeSelf)
            {
                highlightBackground.gameObject.SetActive(false);  
            }     
        }
   
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
