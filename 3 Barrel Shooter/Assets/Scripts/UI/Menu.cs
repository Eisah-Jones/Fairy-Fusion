using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu
{

    private GameObject menu;
    private string menuName;
    private int menuIndex;

    private List<UIElement> menuUIElements = new List<UIElement>();
    private UIElement activeElement;
    private int activeElementIndex;

    private PlayerSelectHandler playerSelectHandler = null;

    public void InitMenu(GameObject m)
    {
        menu = m;
        menuName = m.name;
        if (menuName == "PlayerSelect")
        {
            menu.AddComponent<PlayerSelectHandler>();
            playerSelectHandler = menu.GetComponent<PlayerSelectHandler>();
            playerSelectHandler.InitPlayerSelectHandler();
        }


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
        if (menuUIElements.Count == 0) return;
        activeElementIndex = (activeElementIndex + dir) % menuUIElements.Count;
        if (activeElementIndex < 0) activeElementIndex = menuUIElements.Count - 1;
        activeElement = menuUIElements[activeElementIndex];
        SelectElement();
        SetElementParticleEffects();
    }


    public void SetElementParticleEffects()
    {
        foreach (UIElement iElement in menuUIElements)
        {
            if (iElement.GetElementType() == "Button")
            {
                MenuButton mb = (MenuButton)iElement;
                Button b = mb.GetElementObject();
                if (b.transform.childCount == 0) continue;
                GameObject particleEffect = b.transform.GetChild(0).gameObject;
                if (particleEffect.name == "Button Particles")
                {
                    if (iElement.GetElementName() == activeElement.GetElementName()) particleEffect.SetActive(true);
                    else particleEffect.SetActive(false);
                }
            }
        }
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


    public PlayerSelectHandler GetPlayerSelectHandler()
    {
        return playerSelectHandler;
    }


    public void ResetMenu()
    {
        if (menuName == "MainMenu") return;

        if (menuName == "PlayerSelect")
        {
            playerSelectHandler.ResetMenu();
            menu.SetActive(false);
        }
        else
        {
            activeElementIndex = 0;
            SetActiveElement(0);
        }
    }
}
