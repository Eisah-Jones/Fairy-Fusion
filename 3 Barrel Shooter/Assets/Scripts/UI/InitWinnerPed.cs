using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitWinnerPed : MonoBehaviour
{
    public int[] playerKills;
    public int numPlayers;

    public void SetWinnerPed(int[] pk, int np)
    {
        playerKills = pk;
        numPlayers = np;
    }
}
