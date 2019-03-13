using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerInit : MonoBehaviour
{
    public int numPlayers;

    public void SetNumPlayers(int n)
    {
        numPlayers = n;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
            StartCoroutine("InitLevel");
    }

    private IEnumerator InitLevel()
    {
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>().InitLevelManager(numPlayers);
    }
}
