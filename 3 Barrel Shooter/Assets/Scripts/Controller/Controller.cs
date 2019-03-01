using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents one players controller, with buttons mapped based on given number
public class Controller
{

    // Triggers
    public string Left_Trigger;
    public string Right_Trigger;

    // Face Buttons
    public string X_Button;
    public string B_Button;
    public string A_Button;


    // Bumpers
    public string Right_Bumper;
    public string Left_Bumper;

    // Middle Buttons
    public string Back_Button;
    public string Start_Button;

    // Stick Clicks
    public string Left_Stick_Click;
    public string Right_Stick_Click;

    // Dpad
    public string Dpad_Up;
    public string Dpad_Down;
    public string Dpad_Left;
    public string Dpad_Right;

    // Thumb sticks
    public string Right_Stick_Horizontal;
    public string Left_Stick_Horizontal;

    public string Right_Stick_Vertical;
    public string Left_Stick_Vertical;


    public void initController(int playerNum){

        // These are Mac OS mappings, we can also add options for windows and linux mappings
        string pNum = playerNum.ToString();
        string platform;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) platform = "Win_";
        else platform = "Mac_";

        Left_Trigger = platform + "Left_Trigger " + pNum;
        Right_Trigger = platform + "Right_Trigger " + pNum;

        X_Button = platform + "X_Button " + pNum;
        B_Button = platform + "B_Button " + pNum;
        A_Button = platform + "A_Button " + pNum;

        Right_Bumper = platform + "Right_Bumper " + pNum;
        Left_Bumper = platform + "Left_Bumper " + pNum;

        Back_Button = platform + "Back_Button " + pNum;
        Start_Button = platform + "Start_Button " + pNum;

        Left_Stick_Click = platform + "Left_Stick_Click " + pNum;
        Right_Stick_Click = platform + "Right_Stick_Click " + pNum;

        Dpad_Up = platform + "Dpad_Up " + pNum;
        Dpad_Down = platform + "Dpad_Down " + pNum;
        Dpad_Left = platform + "Dpad_Left " + pNum;
        Dpad_Right = platform + "Dpad_Right " + pNum;

        Left_Stick_Vertical = platform + "Left_Stick_Vertical " + pNum;
        Left_Stick_Horizontal = platform + "Left_Stick_Horizontal " + pNum;
        Right_Stick_Vertical = platform + "Right_Stick_Vertical " + pNum;
        Right_Stick_Horizontal = platform + "Right_Stick_Horizontal " + pNum;
    }

    public ControllerInputs GetInputs(){
        ControllerInputs result = new ControllerInputs();
        result.RefreshInputs(this);
        return result;
    }
}
