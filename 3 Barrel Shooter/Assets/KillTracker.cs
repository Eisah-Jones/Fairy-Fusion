using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KillTracker : MonoBehaviour
{
    LevelManager lm;
    
    public int numPlayers= 4;
    private GameObject[] killcounts = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        numPlayers = lm.numPlayers;
		for (int i = 0; i < numPlayers; i++) {
			GameObject pUI = GameObject.Find ("PlayerUI" + (i+1).ToString ());
			killcounts [i] = pUI.transform.Find("KillCounter").gameObject;
		}
        //ActivateKillDisplay();
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
                t = killcounts[0].GetComponentInChildren<Text>();
                t.text = player.Value.ToString();
            }
            else if (player.Key == "Player2")
            {
				t = killcounts[1].GetComponentInChildren<Text>();
                t.text = player.Value.ToString();
            }
            else if (player.Key == "Player3")
            {
				t = killcounts[2].GetComponentInChildren<Text>();
                t.text = player.Value.ToString();
            }
            else if (player.Key == "Player4")
            {
				t = killcounts[3].GetComponentInChildren<Text>();
                t.text = player.Value.ToString();
            }
      
        

            
            
        }
    }
}
