using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    private GameObject menu;
    private string menuName;
    private int menuIndex;

    private List<UIElement> menuUIElements = new List<UIElement>();
    private UIElement activeElement;
    private int activeElementIndex;

    public void InitMenu(GameObject m)
    {
        menu = m;
        menuName = m.name;
        Debug.Log(menuName);

        foreach (Transform element in menu.transform)
        {
            if (element.tag == "Button")
            {
                MenuButton b = new MenuButton();
                b.InitButton(element.gameObject);
                menuUIElements.Add(b);
            }
            else if (element.tag == "Slider")
            {
                MenuSlider s = new MenuSlider();
                s.InitButton(element.gameObject);
                menuUIElements.Add(s);
            }
        }

        ResetMenu();
    }


    public void SetActiveElement(int dir)
    {
        activeElementIndex = (activeElementIndex + dir) % menuUIElements.Count;
        activeElement = menuUIElements[activeElementIndex];
        SelectElement();
    }


    public void SelectElement()
    {
        activeElement.Select();
    }


    public GameObject GetMenu()
    {
        return menu;
    }


    public string GetName()
    {
        return menuName;
    }


    public UIElement GetActiveElement()
    {
        return activeElement;
    }


    public void ResetMenu()
    {
        activeElementIndex = 0;
        SetActiveElement(0);
    }
}
