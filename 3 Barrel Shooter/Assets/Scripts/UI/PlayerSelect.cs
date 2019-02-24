using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : UIElement
{
    private Text text;
    private string playerColor;
    private string elementType = "PlayerSelect";

    private string join  = "Press A to Join";
    private string readyUp = "Press A to Ready";
    private string readied = "Ready";

    private bool isReady = false;
    private bool back = false;

    public void InitPlayerSelect(Text t)
    {
        text = t;
        text.text = join;
    }


    public void Interact(int dir)
    {
        if (dir == 1) //A button pressed
        {
            if (text.text == join)
                text.text = readyUp;
            else if (text.text == readyUp)
            {
                text.text = readied;
                isReady = true;
            }
        }
        else if (dir == -1) // B button pressed
        {
            if (text.text == readied)
            {
                text.text = readyUp;
                isReady = false;
            }
            else if (text.text == readyUp)
                text.text = join;
            else if (text.text == join) //Go back to main menu
                back = true;
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
        text.text = join;
        back = false;
    }
}
