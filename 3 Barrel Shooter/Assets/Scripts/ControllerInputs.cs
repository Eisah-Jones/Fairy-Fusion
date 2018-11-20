using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputs
{

    // Button inputs
    public bool Xbox_A;
    public bool Xbox_B;
    public bool Xbox_X;
    public bool Xbox_Y;

    public bool Right_Bumper;
    public bool Left_Bumper;

    public bool Back_Button;
    public bool Start_Button;

    public bool Right_Click_Stick;
    public bool Left_Click_Stick;

    public bool Dpad_Up;
    public bool Dpad_Down;
    public bool Dpad_Left;
    public bool Dpad_Right;

    public bool Xbox_Button;

    // Thumb sticks and triggers!
    public float Right_Stick_Horizontal;
    public float Left_Stick_Horizontal;

    public float Right_Stick_Veritcal;
    public float Left_Stick_Veritcal;

    public ControllerInputs RefreshInputs(Controller c)
    {
        Xbox_A = Input.GetButton(c.Xbox_A);
        Xbox_B = Input.GetButton(c.Xbox_B);

        Left_Stick_Horizontal = Input.GetAxisRaw(c.Left_Stick_Horizontal);
        Left_Stick_Veritcal = Input.GetAxisRaw(c.Left_Stick_Vertical);
        Right_Stick_Horizontal = Input.GetAxisRaw(c.Right_Stick_Horizontal);
        Right_Stick_Veritcal = Input.GetAxisRaw(c.Right_Stick_Vertical);

        return this;
    }
}