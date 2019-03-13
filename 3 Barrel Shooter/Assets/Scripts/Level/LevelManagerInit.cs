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

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Level")
            StartCoroutine("InitLevel");
    }

    private IEnumerator InitLevel()
    {
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>().InitLevelManager(numPlayers);
        Destroy(this);
    }
}
