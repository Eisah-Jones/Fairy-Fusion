using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gif : MonoBehaviour
{
    public int numImages = 0;
    public string imageDir = "";
    public bool bounce;

    private Image image;
    private List<Sprite> frames;
    private int framesPerSecond = 10;
    private int index;
    private bool invert = true;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        LoadImages();
    }

    // Update is called once per frame
    void Update()
    {
        float temp = Time.time * framesPerSecond;
        index = (int)(temp % frames.Count);
        if (index == 0) invert = !invert;
        if (bounce && invert)
        {
            index = frames.Count - index - 1;
        }
        image.sprite = frames[index];
    }


    public void ReloadImages()
    {
        LoadImages();
    }


    private void LoadImages()
    {
        frames = new List<Sprite>();
        for (int i = 0; i < numImages; i++)
        {
            frames.Add(Resources.Load<Sprite>(imageDir + i));
        }
    }
}
