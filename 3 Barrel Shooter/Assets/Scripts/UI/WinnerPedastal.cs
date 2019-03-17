using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinnerPedastal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] logs = new GameObject[4];

    public float maxYsize = -200f;
    public float minYsize = -550f;
    public float startYpos = -400f;
    public int num_Players;
    public int[] pKills = new int[4];
    
    Vector3 log0;
    Vector3 log0start;
    Vector3 log1;
    Vector3 log1start;
    Vector3 log2;
    Vector3 log2start;
    Vector3 log3;
    Vector3 log3start;
    public float speed = 2f;
    public bool canMove = false;
    public Vector3[] endPlatforms = new Vector3[4]; // { log0, log1, log2, log3 };
    public Vector3[] startPlatforms = new Vector3[4]; // { log0, log1, log2, log3 };
    public Canvas c;


    private ControllerManager cm;
    private LoadingScreen loadingScreen;
    private Button restart;
    private Button menu;
    private Button currentButton;
    private bool isDetectingInput = true;

    private List<GameObject> pList;
    private AudioSource blipPlayer;

    private bool isLoadingLevel = false;
    private bool isLoadingMenu = false;

    void Start()
    {
        cm = new ControllerManager();
        cm.InitControllerManagerMenus(4);
        endPlatforms = new Vector3[4] { log0, log1, log2, log3 };

        blipPlayer = gameObject.AddComponent<AudioSource>();
        blipPlayer.clip = Resources.Load<AudioClip>("Sounds/blip");

        pList = new List<GameObject>();
        pList.Add(logs[0].transform.GetChild(0).gameObject);
        pList.Add(logs[1].transform.GetChild(0).gameObject);
        pList.Add(logs[2].transform.GetChild(0).gameObject);
        pList.Add(logs[3].transform.GetChild(0).gameObject);

        restart = GameObject.Find("Restart").GetComponent<Button>();
        currentButton = restart;
        currentButton.Select();
        menu = GameObject.Find("Menu").GetComponent<Button>();
        GameObject init = GameObject.Find("InitWin");
        InitWinnerPed initInfo = init.GetComponent<InitWinnerPed>();
        StartWinSequence(initInfo.playerKills, initInfo.numPlayers);
        Destroy(init);
        loadingScreen = GameObject.Find("LoadingScreen").GetComponent<LoadingScreen>();
        loadingScreen.Open();
    }


    private void Update()
    {
        if (canMove)
        {
            MovePlatforms();
        }

        if(cm != null && GetAInput(cm.GetControllerInputs()))
        {
            currentButton.onClick.Invoke();
        }
        else if (GetLSInput(cm.GetControllerInputs()) && isDetectingInput)
        {
            if (currentButton == restart)
            {
                currentButton = menu;
            }
            else
            {
                currentButton = restart;
            }
            blipPlayer.Play();
            currentButton.Select();
            StartCoroutine("InputDetectReset");
        }
    }


    private IEnumerator InputDetectReset()
    {
        isDetectingInput = false;
        yield return new WaitForSeconds(0.25f);
        isDetectingInput = true;
    }


    public bool GetAInput(List<ControllerInputs> ci)
    {
        foreach(ControllerInputs c in ci)
        {
            if (c.A_Button)
                return true;
        }
        return false;
    }


    public bool GetLSInput(List<ControllerInputs> ci)
    {
        foreach (ControllerInputs c in ci)
        {
            if (c.Left_Stick_Horizontal < 0 || c.Left_Stick_Horizontal > 0)
                return true;
        }
        return false;
    }

    public void MovePlatforms()
    {
        int n = 0;
        for (int i = 0; i < num_Players; i++)
        {
            float step = speed * Time.deltaTime;
            logs[i].transform.localPosition = endPlatforms[i]; //Vector3.Lerp(logs[i].transform.localPosition, endPlatforms[i], step);
            if (logs[i].transform.localPosition == endPlatforms[1])
            {
                n++;
            }
        }
        if (n == num_Players)
            canMove = false;
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
        num_Players = numPlayers;
        Reset_Set_StartPositions();
        CalculatePedastalPositions(playerKills, numPlayers);
        SetWinText(playerKills, numPlayers);
        MovePlatforms();
    }

    public void Reset_Set_StartPositions()
    {
        for (int i =0; i <logs.Length; i++)
        {
            startPlatforms[i] = logs[i].transform.localPosition = new Vector3(logs[i].transform.localPosition.x, startYpos, logs[i].transform.localPosition.z);
        }
    }

    public void CalculatePedastalPositions(int[] playerKills, int numPlayers)
    {
        num_Players = numPlayers;
        //ResetStartPositions();
        float max = Mathf.Max(playerKills);

 

        if (numPlayers == 2)
        {
            if (playerKills[0] == max)
            {
                //logs[0].transform.localPosition = Vector3.MoveTowards(new Vector3(-325f, startYpos, 0f), new Vector3(-325f, maxYsize, 0f), 1 * Time.deltaTime);
                // ped1.transform.localPosition = new Vector3(1, maxYsize, 1);
                endPlatforms[0] = new Vector3(-325f, maxYsize, 0f);
            
            }
            else
            {
                float n;
                if ((playerKills[0] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[0] / max * 432 - 632; }
                endPlatforms[0]  = new Vector3(-325f, n, 0f);
            }
            if (playerKills[1] == max)
            {
                endPlatforms[1]  = new Vector3(375f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[1] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[1] / max * 432 - 632; }
                endPlatforms[1] = new Vector3(375, n, 0f);

            }

            logs[2].gameObject.SetActive(false);
            logs[3].gameObject.SetActive(false);
        }
        if (numPlayers == 3)
        {
            if (playerKills[0] == max)
            {
                endPlatforms[0] = new Vector3(-375f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[0] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[0] / max * 432 - 632; }
                endPlatforms[0]  = new Vector3(-375f, n, 0f);
            }
            if (playerKills[1] == max)
            {
                endPlatforms[1]  = new Vector3(25f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[1] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[1] / max * 432 - 632; }
                endPlatforms[1]  = new Vector3(25f, n, 0f);

            }
            if (playerKills[2] == max)
            {
                endPlatforms[2] = new Vector3(375f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[2] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[2] / max * 432 - 632; }
                endPlatforms[2]  = new Vector3(375f, n, 0f);

            }

            logs[2].gameObject.SetActive(true);

            logs[3].gameObject.SetActive(false);
        }
        if (numPlayers == 4)
        {
            if (playerKills[0] == max)
            {
                endPlatforms[0] = new Vector3(-500f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[0] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[0] / max * 432 - 632; }
                endPlatforms[0] = new Vector3(-500f, n, 0f);
            }
            if (playerKills[1] == max)
            {
                endPlatforms[1]  = new Vector3(-150f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[1] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[1] / max * 432 - 632; }
                endPlatforms[1]  = new Vector3(-150f, n, 0f);

            }
            if (playerKills[2] == max)
            {
                endPlatforms[2] = new Vector3(200f, maxYsize, 0f);
            }
            else
            {
                float n;
                if ((playerKills[2] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[2] / max * 432 - 632; }
                endPlatforms[2] = new Vector3(200f, n, 0f);

            }
            if (playerKills[3] == max)
            {
                endPlatforms[3]  = new Vector3(550f, maxYsize, 0f); //= logs[3].transform.localPosition
            }
            else
            {
                float n;
                if ((playerKills[3] / max * 432 - 632) < -550f)
                {
                    n = minYsize;
                }
                else { n = playerKills[3] / max * 432 - 632; }
                endPlatforms[3] = new Vector3(550f, n, 0f); // = logs[3].transform.localPosition
            }

            logs[2].gameObject.SetActive(true);
            logs[3].gameObject.SetActive(true);
        }

        InitPlayerAnimations(playerKills, numPlayers);
    }


    public void InitPlayerAnimations(int[] playerKills, int numPlayers)
    {
        int max = Mathf.Max(playerKills);
        int min = Mathf.Min(playerKills);
   

        int mid = 0;
        for (int i = 0; i < numPlayers; i++)
        {
            Gif g = pList[i].GetComponent<Gif>();
            if (playerKills[i] == max) //Winner
            {
                g.imageDir = "WinAnimations/P" + (i + 1) + "/Win/";
                g.numImages = 4;
                g.ReloadImages();
            }
            else if (playerKills[i] == min)
            {
                g.imageDir = "WinAnimations/P" + (i + 1) + "/Cry/";
                g.numImages = 6;
                g.ReloadImages();
            }
            else if (mid == 0)
            {
                mid++;
                g.imageDir = "WinAnimations/P" + (i + 1) + "/Clap/";
                g.numImages = 2;
                g.ReloadImages();
            }
            else
            {
                g.imageDir = "WinAnimations/P" + (i + 1) + "/Peace/";
                g.numImages = 2;
                g.ReloadImages();
            }
        }
    }

    public void LoadMenu()
    {
        if (!isLoadingMenu)
        {
            isLoadingMenu = true;
            Destroy(GameObject.Find("Level Manager Initializer"));
            SceneManager.LoadScene(0);
        }
    }

    public void LoadLevel()
    {
        if (!isLoadingLevel)
        {
            isLoadingLevel = true;
            SceneManager.LoadScene(1);
        }
    }
}
