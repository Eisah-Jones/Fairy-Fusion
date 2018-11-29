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
    float l_horizontal;
    float l_vertical;
    bool suck;
    bool shoot;
    float last_heading;


    private VacuumController vacControl;

    public void InitPlayerController(VacuumController vc){
        vacControl = vc;
    }

    public void UpdatePlayerInputs(ControllerInputs inputs)
    {
        suck = inputs.Xbox_A;
        shoot = inputs.Xbox_B;



        vacControl.HandleVacuumStateInput(suck);
        vacControl.HandleShootStateInput(shoot);
    }


    // This function is called every frame by the level manager, called in fixed update
    public void UpdatePlayerMovement(ControllerInputs inputs)
    {
        // Getting the horizontal axis
        l_horizontal = inputs.Left_Stick_Horizontal;
        l_vertical = -inputs.Left_Stick_Veritcal;

        float r_horizontal = inputs.Right_Stick_Horizontal;
        if (r_horizontal < 0)
            transform.rotation = new Quaternion(0.0f, 0.0f, 90.0f, 1.0f);
        else if (r_horizontal > 0)
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        //Change the position of the player
        Vector2 movement = new Vector2(l_horizontal * speed, l_vertical * speed);
        transform.Translate(movement * Time.deltaTime,Space.World);
       

       

  
    }
}