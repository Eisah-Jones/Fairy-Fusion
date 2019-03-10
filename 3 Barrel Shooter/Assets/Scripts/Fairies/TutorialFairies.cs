using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFairies : MonoBehaviour
{
    private BoxCollider2D harnessArea;

    public void InitTutorialFairies()
    {
        harnessArea = GetComponent<BoxCollider2D>();
    }


    public void SetHarnessArea(bool b)
    {
        harnessArea.enabled = b;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
