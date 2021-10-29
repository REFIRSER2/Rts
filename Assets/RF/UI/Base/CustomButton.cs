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

    [SerializeField] private Image background;
    
    [SerializeField] private Sprite normalBackground;
    [SerializeField] private Sprite highlightBackground;
    [SerializeField] private Sprite selectedBackground;
    [SerializeField] private Sprite disabledBackground;

    [SerializeField] private bool isChangeTextColor;

    [SerializeField] private Text title_Text;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color disableColor;
    
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
        if (isChangeImage)
        {
            if (highlightBackground)
            {
                background.sprite = highlightBackground;
            }   
        }

        if (isChangeTextColor)
        {
            title_Text.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isChangeImage)
        {
            if (normalBackground != null)
            {
                background.sprite = normalBackground;
            }    
        }
        
        if (isChangeTextColor)
        {
            title_Text.color = normalColor;
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
