using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public bool isOpen;

    private void Start()
    {
        left = transform.GetChild(0).gameObject;
        right = transform.GetChild(1).gameObject;
        left.GetComponent<RectTransform>().localPosition = new Vector2(-800f, 0f);
        right.GetComponent<RectTransform>().localPosition = new Vector2(800f, 0f);
        isOpen = true;
    }


    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
    }
}
