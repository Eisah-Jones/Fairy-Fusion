using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectHandler : MonoBehaviour
{
    private int minNumPlayers = 1;
    private int numPlayersReady = 0;
    private int[] status;

    private List<UIElement> playerSelectUIElements = new List<UIElement>();

    private GameObject startMatch;

    private bool back = false;
    private bool isStartingGame = false;
    private bool isLoadingLevel = false;

    public void InitPlayerSelectHandler()
    {
        status = new int[] { 0, 0, 0, 0 };

        foreach (Transform element in transform)
        {
            if (element.CompareTag("PlayerSelect"))
            {
                PlayerSelect ps = new PlayerSelect();
                ps.InitPlayerSelect(element.gameObject);
                playerSelectUIElements.Add(ps);
            }
        }

        startMatch = transform.GetChild(4).gameObject;
        startMatch.SetActive(false);
    }


    public void HandleInput(List<ControllerInputs> ci)
    {
        if (isStartingGame) return;
        GetNumPlayersReady();

        for (int i = 0; i < ci.Count; i++)
        {
            ControllerInputs input = ci[i];

            if (numPlayersReady >= minNumPlayers && input.Start_Button && !isLoadingLevel)
            {
                isLoadingLevel = true;
                AudioSource selectSource = gameObject.AddComponent<AudioSource>();
                selectSource.clip = Resources.Load<AudioClip>("Sounds/select");
                selectSource.Play();
                //// Maybe do a short countdown to allow for cancelling?
                GameObject levelManagerInitializer = Instantiate(new GameObject());
                levelManagerInitializer.name = "Level Manager Initializer";
                levelManagerInitializer.AddComponent<LevelManagerInit>();
                levelManagerInitializer.GetComponent<LevelManagerInit>().SetNumPlayers(numPlayersReady);
                DontDestroyOnLoad(levelManagerInitializer);
                GameObject loadingScreen = GameObject.Find("LoadingScreen");
                loadingScreen.GetComponent<LoadingScreen>().Close();
                StartCoroutine("LoadLevel");
            }
            else if (numPlayersReady >= minNumPlayers)
            {
                startMatch.SetActive(true);
            }
            else if (startMatch.activeSelf)
            {
                startMatch.SetActive(false);
            }

            if (input.A_Button)
            {
                playerSelectUIElements[i].Interact(1);
                status[i] = status[i] + 1;
            }
            else if (input.B_Button)
            {
                playerSelectUIElements[i].Interact(-1);
                PlayerSelect ps = (PlayerSelect) playerSelectUIElements[i];
                back = ps.GetBack();
                status[i] = status[i] - 1;
            }
        }
    }


    private void GetNumPlayersReady()
    {
        numPlayersReady = 0;
        foreach (PlayerSelect ps in playerSelectUIElements)
        {
            if (ps.IsReady()) numPlayersReady++;
        }
    }


    public bool GetBack()
    {
        return back;
    }


    public void ResetMenu()
    {
        numPlayersReady = 0;
        back = false;
        foreach (PlayerSelect ps in playerSelectUIElements)
        {
            ps.Reset();
        }
    }


    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}
