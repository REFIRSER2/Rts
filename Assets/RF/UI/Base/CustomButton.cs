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

    [SerializeField] private bool isChangeImageColor = false;

    [SerializeField] private Color normalBGColor;
    [SerializeField] private Color highlightBGColor;
    [SerializeField] private Color selectedBGColor;
    [SerializeField] private Color disabledBGColor;

    [SerializeField] private bool isChangeTextColor;

    [SerializeField] private Text title_Text;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color disableColor;
    
    [SerializeField] private List<CustomButton> selectList = new List<CustomButton>();

    [SerializeField]private bool isSelect = false;
    public bool isDisable = false;
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDisable)
        {
            onClick.Invoke();  
            
            foreach (var btn in selectList)
            {
                btn.UnSelect();
            }   
            this.Select();
        }

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
            if (highlightBackground && !isSelect && !isDisable)
            {
                background.sprite = highlightBackground;
            }   
        }

        if (isChangeImageColor)
        {
            if (!isSelect)
            {
                background.color = highlightBGColor;
            }
        }

        if (isChangeTextColor && !isSelect)
        {
            title_Text.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isChangeImage)
        {
            if (normalBackground != null && !isSelect && !isDisable)
            {
                background.sprite = normalBackground;
                Debug.Log("exit");
            }    
        }
        
        if (isChangeImageColor)
        {
            if (!isSelect)
            {
                background.color = normalBGColor;
            }
        }
        
        if (isChangeTextColor && !isSelect)
        {
            title_Text.color = normalColor;
        }
    }

    public void Select()
    {
        if (isChangeImage)
        {
            if (selectedBackground != null)
            {
                background.sprite = selectedBackground;
                isSelect = true;
            }
        }
        
        if (isChangeImageColor)
        {
            background.color = selectedBGColor;
        }

        if (isChangeTextColor)
        {
            title_Text.color = selectedColor;
        }
    }

    public void UnSelect()
    {
        if (isChangeImage)
        {
            if (normalBackground != null)
            {
                background.sprite = normalBackground;
            }
        }
        
        if (isChangeImageColor)
        {
            background.color = normalBGColor;
        }
        
        if (isChangeTextColor)
        {
            title_Text.color = normalColor;
        }
        
        isSelect = false;
    }

    public void SetDisable(bool isDisable)
    {
        if (isChangeImage)
        {
            if (isDisable && disabledBackground != null)
            {
                background.sprite = disabledBackground;
            }
            else
            {
                background.sprite = normalBackground;
            }
        }    
        
        if (isChangeImageColor)
        {
            background.color = disabledBGColor;
        }
        
        if (isChangeTextColor)
        {
            title_Text.color = disableColor;
        }
        
        this.isDisable = isDisable;
    }

    #region Unity General Funcs
    private void Awake()
    {
        if (isSelect)
        {
            if (isChangeImage)
            {
                if (selectedBackground != null)
                {
                    background.sprite = selectedBackground;
                }
            }

            if (isChangeTextColor)
            {
                title_Text.color = selectedColor;
            }       
        }
 
    }

    private void Update()
    {
        
    }
    #endregion
}
