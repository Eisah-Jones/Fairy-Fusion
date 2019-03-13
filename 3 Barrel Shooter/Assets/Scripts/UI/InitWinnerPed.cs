using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitWinnerPed : MonoBehaviour
{
    int[] playerKills;
    int numPlayers;

    private void Awake()
    {
        GameObject.Find("Manager").GetComponent<WinnerPedastal>().StartWinSequence(playerKills, numPlayers);
    }

    public InitWinnerPed(int[] pk, int np)
    {
        playerKills = pk;
        numPlayers = np;
    }
}
