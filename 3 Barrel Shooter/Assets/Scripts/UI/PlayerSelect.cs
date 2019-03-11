using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : UIElement
{
    private string elementType = "PlayerSelect";

    private GameObject prompt;
    private GameObject playerImage;
    private GameObject ready;

    private bool isReady = false;
    private bool back = false;

    public void InitPlayerSelect(GameObject ps)
    {
        prompt = ps.transform.GetChild(2).gameObject;
        playerImage = ps.transform.GetChild(1).gameObject;
        ready = ps.transform.GetChild(3).gameObject;
        playerImage.SetActive(false);
        ready.SetActive(false);
    }


    public void Interact(int dir)
    {
        if (!isReady && dir == 1)
        {
            isReady = !isReady;
            playerImage.SetActive(true);
            prompt.SetActive(false);
            ready.SetActive(true);
        }
        else if (!isReady && dir == -1) { back = true; } // Go to main menu
        else if (isReady && dir == -1) 
        { 
            isReady = !isReady;
            playerImage.SetActive(false);
            prompt.SetActive(true);
            ready.SetActive(false);
        }
    }

    // Not applicable
    public void Click()
    {
        return;
    }

    // Not applicable
    public void Select()
    {
        return;
    }


    public string GetElementType()
    {
        return elementType;
    }

    // Not applicable
    public string GetElementName()
    {
        return null;
    }


    public bool IsReady()
    {
        return isReady;
    }


    public bool GetBack()
    {
        return back;
    }


    public void Reset()
    {

    }
}
