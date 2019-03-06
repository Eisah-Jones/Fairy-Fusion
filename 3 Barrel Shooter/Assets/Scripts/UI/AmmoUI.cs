using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
	public Image image;
    private float opacity = 0.75f;


	public void SetFill(float fill, string name)
    {
		image.fillAmount = fill;
        Color c = Color.white;
        if (name == "Water")
        {
            c = Color.blue;
            c.a = opacity;
            image.color = c;
        }
        else if (name == "Fire")
        {
            c = Color.red;
            c.a = opacity;
            image.color = c;
        }
        else if (name == "Leaf")
        {
            c = Color.green;
            c.a = opacity;
            image.color = c;
        }
        else if (name == "Rock")
        {
            c = new Color(139f/255, 69f/255, 19f/255);
            c.a = opacity;
            image.color = c;
        }
        else
        {
            c = image.color;
            c.a = opacity;
            image.color = c;
        }
	}
}
