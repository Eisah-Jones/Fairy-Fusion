using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    // Stage 0 = controls screen
    // Stage 1 = 
    private int stage = 0;

    UIManager uiManager;
    GameObject tutorialPlayer;
    TutorialPlayerController tutorialPC;

    private bool A_Button;
    private bool B_Button;

    private float RS_Horizontal;
    private float RS_Vertical;
    private float LS_Horizontal;
    private float LS_Vertical;


    public void InitTutorialMenu(UIManager uim, LevelManager lm)
    {
        uiManager = uim;
        tutorialPlayer = Instantiate(Resources.Load<GameObject>("Player/TutorialPlayer"));
        tutorialPC = tutorialPlayer.GetComponent<TutorialPlayerController>();
        tutorialPC.InitTutorialController(lm);
    }


    public void HandleInput(ControllerInputs c)
    {
        // Get controls; check stage; call function

        A_Button = c.A_Button;
        B_Button = c.B_Button;

        RS_Horizontal = c.Right_Stick_Horizontal;
        RS_Vertical = c.Right_Stick_Vertical;
        LS_Horizontal = c.Left_Stick_Horizontal;
        LS_Vertical = c.Left_Stick_Vertical;


        switch(stage)
        {
            case 0:
                ControlStage(A_Button, B_Button);
                break;
            case 1:
                tutorialPC.Stage1(LS_Horizontal, LS_Vertical, RS_Horizontal, RS_Vertical);
                break;
        }
    }

    public void ControlStage(bool a, bool b)
    {
        if (b)
        {
            uiManager.ToMainMenu();
            return;
        }

        stage = 1;
    }
}
