using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputs
{

    // Button inputs
    public float Left_Trigger;
    public float Right_Trigger;

    public bool X_Button;
    public bool B_Button;
    public bool A_Button;

    public bool Right_Bumper;
    public bool Left_Bumper;

    public bool Back_Button;
    public bool Start_Button;



    public bool Dpad_Up;
    public bool Dpad_Down;
    public bool Dpad_Left;
    public bool Dpad_Right;

    public bool Xbox_Button;
    public bool Left_Stick_Click;
    public bool Right_Stick_Click;
    // Thumb sticks and triggers!
    public float Right_Stick_Horizontal;
    public float Left_Stick_Horizontal;

    public float Right_Stick_Vertical;
    public float Left_Stick_Vertical;

    public ControllerInputs RefreshInputs(Controller c)
    {
        Left_Trigger = Input.GetAxisRaw(c.Left_Trigger);
        Right_Trigger = Input.GetAxisRaw(c.Right_Trigger);

        Right_Bumper = Input.GetButton(c.Right_Bumper);
        Left_Bumper = Input.GetButton(c.Left_Bumper);

        Left_Stick_Click = Input.GetButtonDown(c.Left_Stick_Click);
        Right_Stick_Click = Input.GetButtonDown(c.Right_Stick_Click);


        Left_Stick_Horizontal = Input.GetAxisRaw(c.Left_Stick_Horizontal);
        Left_Stick_Vertical = Input.GetAxisRaw(c.Left_Stick_Vertical);

        Right_Stick_Horizontal = Input.GetAxisRaw(c.Right_Stick_Horizontal);
        Right_Stick_Vertical = Input.GetAxisRaw(c.Right_Stick_Vertical);

        X_Button = Input.GetButtonDown(c.X_Button);
        B_Button = Input.GetButtonDown(c.B_Button);
        A_Button = Input.GetButtonDown(c.A_Button);

        Start_Button = Input.GetButtonUp(c.Start_Button);

        return this;
    }
}