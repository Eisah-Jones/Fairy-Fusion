using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : UIElement
{
    private Slider slider;
    private string sliderName;
    private string elementType = "Slider";

    public void InitButton(GameObject s)
    {
        slider = s.GetComponent<Slider>();
        sliderName = s.name;
    }
    

    public void Select()
    {
        slider.Select();
    }


    public void Interact(int dir)
    {
        slider.normalizedValue = slider.normalizedValue + 0.01f * dir;
    }


    public string GetElementType()
    {
        return elementType;
    }


    public string GetElementName()
    {
        return sliderName;
    }
}
