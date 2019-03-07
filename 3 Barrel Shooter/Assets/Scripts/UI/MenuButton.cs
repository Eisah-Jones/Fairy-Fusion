using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : UIElement
{
    private Button button;
    private string buttonName;
    private string elementType = "Button";

    public void InitButton(GameObject b)
    {
        button = b.GetComponent<Button>();
        buttonName = b.name;
    }


    public void Interact(int dir)
    {
        Click();
    }


    public void Click()
    {
        button.onClick.Invoke();
    }


    public void Select()
    {
        button.Select();
    }


    public string GetElementType()
    {
        return elementType;
    }


    public string GetElementName()
    {
        return buttonName;
    }


    public Button GetElementObject()
    {
        return button;
    }
}
