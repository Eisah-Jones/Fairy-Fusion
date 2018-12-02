using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBody;

    private bool facingRight = true;
    private bool flipped = false;
    [SerializeField]
    private float speed =20f;
    public string inputHorizontal = "Horizontal";
    public string inputVertical = "Vertical";
    public string shootButton = "ShootButton_P1";
    public string vacuumButton = "VacuumButton_P1";
    float horizontal;
    float vertical;
    bool suck= false;
    bool shoot= false;
    float last_heading;


    private VacuumController vacControl;

    public void InitPlayerController(VacuumController vc){
        vacControl = vc;
    }

    public void UpdatePlayerInputs(ControllerInputs inputs)
    {
        //Debug.Log(inputs.Left_Trigger + ":" + inputs.Right_Trigger);
        if (inputs.Left_Trigger == 1) { suck = true; } else suck = false;
        if (inputs.Right_Trigger == 1) { shoot = true; } else shoot = false;




        vacControl.HandleVacuumStateInput(suck);
        vacControl.HandleShootStateInput(shoot);
    }


    // This function is called every frame by the level manager, called in fixed update
    public void UpdatePlayerMovement(ControllerInputs inputs)
    {
        // Getting the horizontal axis


        //float r_horizontal = inputs.Right_Stick_Horizontal;
        //if (r_horizontal < 0)
        //    transform.rotation = new Quaternion(0.0f, 0.0f, 90.0f, 1.0f);
        //else if (r_horizontal > 0)
        //    transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        ////Change the position of the player
        //Vector2 movement = new Vector2(l_horizontal * speed, l_vertical * speed);
        //transform.Translate(movement * Time.deltaTime,Space.World);
        horizontal = inputs.Left_Stick_Horizontal;
        vertical = -inputs.Left_Stick_Veritcal;

        float r_vertical = inputs.Right_Stick_Veritcal;
        float r_horizontal = inputs.Right_Stick_Horizontal;
        float heading = Mathf.Atan2(r_vertical, r_horizontal);



        //Debug.Log(heading);

        //Change the position of the player
        Vector2 movement = new Vector2(horizontal * speed, vertical * speed);
        transform.Translate(movement * Time.deltaTime, Space.World);
        //change rotation of pplayer if no input received keeps same rotation as last time it got input prevents snapping back to 0,0 heading
        if (heading != 0)
        {
            float last_heading = heading;
            transform.rotation = Quaternion.Euler(0f, 0f, last_heading * Mathf.Rad2Deg);
        }





    }
}