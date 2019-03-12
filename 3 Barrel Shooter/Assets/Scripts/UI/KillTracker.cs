using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KillTracker : MonoBehaviour
{
    LevelManager lm;

    public int numPlayers;
    Text[] t = new Text[4];


    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        numPlayers = lm.numPlayers;
    }

    public void InitKillTracker()
    {
        int i = 0;
        
        foreach (GameObject p in lm.GetPlayerList())
        {
            PlayerInfo pInfo = p.GetComponent<PlayerInfo>();
            t[i] = GameObject.Find("PlayerUI" + pInfo.playerNum.ToString()).transform.Find("KillCounter").GetComponent<Text>();
            i++;
        }
    }


    public void updateCanvasElements(Dictionary<string, int> kd)
    {
        foreach (KeyValuePair<string, int> player in kd)
        {
            if (player.Key == "Player1")
            {
                t[0].text = player.Value.ToString();
            }

            else if (player.Key == "Player2")
            {
                t[1].text = player.Value.ToString();
            }

            else if (player.Key == "Player3")
            {
                t[2].text = player.Value.ToString();
            }

            else if (player.Key == "Player4")
            {
                t[3].text = player.Value.ToString();
            }
        }
    }
}
