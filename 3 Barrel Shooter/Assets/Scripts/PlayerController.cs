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
    bool suck;
    bool shoot;


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
        horizontal = inputs.Left_Stick_Horizontal;
        vertical = -inputs.Left_Stick_Veritcal;

        //Debug.Log(horizontal + " : " + vertical);

        //Change the position of the player
        Vector2 movement = new Vector2(horizontal * speed, vertical * speed);
        transform.Translate(movement * Time.deltaTime);
    }
}