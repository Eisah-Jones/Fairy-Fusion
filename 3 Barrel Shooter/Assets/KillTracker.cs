using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KillTracker : MonoBehaviour
{
    LevelManager lm;
    
    public GameObject canvas;
    public int numPlayers= 2;
    private GameObject[] killcounts = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        numPlayers = lm.numPlayers;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        killcounts[0] = canvas.transform.GetChild(5).gameObject;
        Debug.Log(canvas.transform.GetChild(5).gameObject.name);
        killcounts[1] = canvas.transform.GetChild(6).gameObject;
        killcounts[2] = canvas.transform.GetChild(7).gameObject;
        killcounts[3] = canvas.transform.GetChild(8).gameObject;
        ActivateKillDisplay();
    }


    
    void ActivateKillDisplay()
    {
        for (int i = 0; i < numPlayers; i++)
        {
  
            killcounts[i].SetActive(true);
            killcounts[i].transform.GetChild(0).gameObject.SetActive(true);
         
        }
    }

    public void updateCanvasElements(Dictionary<string, int> killDict)
    {
  
   
        foreach (KeyValuePair<string, int> player in killDict)
        {
            Text t;
            if (player.Key == "Player1")
            {
                t = killcounts[0].GetComponent<Text>();
                t.text = player.Value.ToString();
            }
            else if (player.Key == "Player2")
            {
                t = killcounts[1].GetComponent<Text>();
                t.text = player.Value.ToString();
            }
            else if (player.Key == "Player3")
            {
                t = killcounts[2].GetComponent<Text>();
                t.text = player.Value.ToString();
            }
            else if (player.Key == "Player4")
            {
                t = killcounts[3].GetComponent<Text>();
                t.text = player.Value.ToString();
            }
      
        

            
            
        }
    }
}
