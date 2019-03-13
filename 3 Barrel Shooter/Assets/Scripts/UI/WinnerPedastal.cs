using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerPedastal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] logs = new GameObject[4];

    public float maxYsize = -200f;
    public float minYsize = -550f;

    public int numPlayers;
    //public int[] pKills = new int[4];

    public Canvas c;

    void Start()
    {
        //CalculatePedastalPositions(2, 2);
    }


    public void AnimatePlayers()
    {

    }

    public void SetWinText(int[] playerKills, int numPlayers)
    {
        for (int i = 0; i < numPlayers; i++)
        {
            logs[i].GetComponentInChildren<Text>().text = playerKills[i].ToString();
        }
    }

    public void StartWinSequence(int[] playerKills, int numPlayers) // start win seq calls all necessary functions
    {
        CalculatePedastalPositions(playerKills, numPlayers);
        SetWinText(playerKills, numPlayers);
        
    }


    public void CalculatePedastalPositions(int[] playerKills, int numPlayers)
    {
        float max = Mathf.Max(playerKills);
        if (numPlayers == 2)
        {
            if (playerKills[0] == max)
            {
               // ped1.transform.localPosition = new Vector3(1, maxYsize, 1);
                logs[0].transform.localPosition = new Vector3(-325f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[0] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[0] / max * 432 - 632; }
                logs[0].transform.localPosition = new Vector3(-325f, n, 0f);
            }
            if (playerKills[1] == max)
            {
                logs[1].transform.localPosition = new Vector3(375f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[1] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[1] / max * 432 - 632; }
                logs[1].transform.localPosition = new Vector3(375, n, 0f);

            }

            logs[2].gameObject.SetActive(false);
            logs[3].gameObject.SetActive(false);
        }
        if (numPlayers == 3)
        {
            if (playerKills[0] == max)
            {
                logs[0].transform.localPosition = new Vector3(-375f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[0] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[0] / max * 432 - 632; }
                logs[0].transform.localPosition = new Vector3(-375f, n, 0f);
            }
            if (playerKills[1] == max)
            {
                logs[1].transform.localPosition = new Vector3(25f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[1] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[1] / max * 432 - 632; }
                logs[1].transform.localPosition = new Vector3(25f, n, 0f);

            }
            if (playerKills[2] == max)
            {
                logs[2].transform.localPosition = new Vector3(375f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[2] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[2] / max * 432 - 632; }
                logs[2].transform.localPosition = new Vector3(375f, n, 0f);

            }

            logs[2].gameObject.SetActive(true);

            logs[3].gameObject.SetActive(false);
        }
        if (numPlayers == 4)
        {
            if (playerKills[0] == max)
            {
                logs[0].transform.localPosition = new Vector3(-500f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[0] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[0] / max * 432 - 632; }
                logs[0].transform.localPosition = new Vector3(-500f, n, 0f);
            }
            if (playerKills[1] == max)
            {
                logs[1].transform.localPosition = new Vector3(-150f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[1] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[1] / max * 432 - 632; }
                logs[1].transform.localPosition = new Vector3(-150f, n, 0f);

            }
            if (playerKills[2] == max)
            {
                logs[2].transform.localPosition = new Vector3(200f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[2] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[2] / max * 432 - 632; }
                logs[2].transform.localPosition = new Vector3(200f, n, 0f);

            }
            if (playerKills[3] == max)
            {
                logs[3].transform.localPosition = new Vector3(550f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[3] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[3] / max * 432 - 632; }
                logs[3].transform.localPosition = new Vector3(550f, n, 0f);
            }

            logs[2].gameObject.SetActive(true);
            logs[3].gameObject.SetActive(true);
       

        }

       // SetWinText(playerKills, numPlayers);
    }


}
