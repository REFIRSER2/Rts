using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInfo_Item : UIItem_Base
{
    [SerializeField] private RawImage modelView;
    
    [SerializeField] private Text title_Text;
    
    [SerializeField] private Slider energy_Slider;
    [SerializeField] private Slider weight_Slider;
    [SerializeField] private Slider speed_Slider;
    [SerializeField] private Slider health_Slider;
    [SerializeField] private Slider sight_Slider;
    [SerializeField] private Slider damage_Slider;
    [SerializeField] private Slider dps_Slider;
    [SerializeField] private Slider reach_Slider;

    public void SetModel(Texture texture)
    {
        modelView.texture = texture;
    }
    
    public void SetTitle(string title)
    {
        title_Text.text = title;
    }

    public void SetEnergy(int value)
    {
        energy_Slider.value = value;
    }
    
    public void SetWeight(int value)
    {
        weight_Slider.value = value;
    }
    
    public void SetSpeed(int value)
    {
        speed_Slider.value = value;
    }
    
    public void SetHealth(int value)
    {
        health_Slider.value = value;
    }
    
    public void SetSight(int value)
    {
        sight_Slider.value = value;
    }
    
    public void SetDamage(int value)
    {
        damage_Slider.value = value;
    }
    
    public void SetDps(int value)
    {
        dps_Slider.value = value;
    }
    
    public void SetReach(int value)
    {
        reach_Slider.value = value;
    }
    
    private void Awake()
    {
        
    }
}
