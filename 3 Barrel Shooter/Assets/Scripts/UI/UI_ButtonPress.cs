using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonPress : MonoBehaviour
{
    private Image image;
    int frame = 0;
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
        frame++;
        if (frame < switchFrame)
        {
            image.sprite = imageIdle;
        }
        else
        {
            image.sprite = imagePressed;
        }

        if (frame == switchFrame*2) frame = 0;
    }
}
