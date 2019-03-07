using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonPress : MonoBehaviour
{
    private Image image;
    int i = 0;
    int switchFrame = 60;

    public Sprite imageIdle;
    public Sprite imagePressed;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        i++;
        if (i < switchFrame)
        {
            image.sprite = imageIdle;
        }
        else
        {
            image.sprite = imagePressed;
        }

        if (i == switchFrame*2) i = 0;
    }
}
