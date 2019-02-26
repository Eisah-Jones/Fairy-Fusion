using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectHandler : MonoBehaviour
{
    private int minNumPlayers = 1;
    private int numPlayersReady = 0;

    private List<UIElement> playerSelectUIElements = new List<UIElement>();

    private bool back = false;

    public void InitPlayerSelectHandler()
    {
        foreach (Transform element in transform)
        {
            if (element.CompareTag("PlayerSelect"))
            {
                PlayerSelect ps = new PlayerSelect();
                ps.InitPlayerSelect(element.GetComponent<Text>());
                playerSelectUIElements.Add(ps);
            }
        }
    }


    public void HandleInput(List<ControllerInputs> ci)
    {
        GetNumPlayersReady();

        for (int i = 0; i < ci.Count; i++)
        {
            ControllerInputs input = ci[i];

            if (numPlayersReady >= minNumPlayers && input.Start_Button)
            {
                //// Maybe do a short countdown to allow for cancelling?
                GameObject levelManagerInitializer = Instantiate(new GameObject());
                levelManagerInitializer.name = "Level Manager Initializer";
                levelManagerInitializer.AddComponent<LevelManagerInit>();
                levelManagerInitializer.GetComponent<LevelManagerInit>().SetNumPlayers(numPlayersReady);
                DontDestroyOnLoad(levelManagerInitializer);
                SceneManager.LoadScene(1); // Start the match with joined players
            }

            if (input.A_Button)
            {
                playerSelectUIElements[i].Interact(1);
            }
            else if (input.B_Button)
            {
                playerSelectUIElements[i].Interact(-1);
                PlayerSelect ps = (PlayerSelect) playerSelectUIElements[i];
                back = ps.GetBack();
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
}
