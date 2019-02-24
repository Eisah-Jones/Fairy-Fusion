using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerInit : MonoBehaviour
{
    public int numPlayers;

    public void SetNumPlayers(int n)
    {
        numPlayers = n;
    }

    private void Awake()
    {
        // Initializie Level Manager from here
    }
}
