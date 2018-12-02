using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBody;
	public Animator player_animator;
	private Animator gun_animator;


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
    bool combinationToggle = false; //If true then canshoot combination, else cannot
    float last_heading;

    private VacuumController vacControl;



    public void InitPlayerController(VacuumController vc){
        vacControl = vc;
    }

    public void UpdatePlayerInputs(ControllerInputs inputs)
    {
        if (inputs.Left_Stick_Click){
            combinationToggle = !combinationToggle;
            vacControl.SetIsCombiningElements(combinationToggle);
        }

        if (inputs.Left_Trigger == 1) { suck = true; } else suck = false;
        if (inputs.Right_Trigger == 1) { shoot = true; } else shoot = false;

        vacControl.HandleVacuumStateInput(suck);
        vacControl.HandleShootStateInput(shoot);

        bool r_bumper = inputs.Right_Bumper;
        bool l_bumper = inputs.Left_Bumper;

        int d = 0;

        if (r_bumper) { d = 1; } else if (l_bumper) { d = -1; }

        vacControl.HandleChamberStateInput(d);
    }


    // This function is called every frame by the level manager, called in fixed update
    public void UpdatePlayerMovement(ControllerInputs inputs)
    {
        ////Change the position of the player
        horizontal = inputs.Left_Stick_Horizontal;
        vertical = -inputs.Left_Stick_Veritcal;

        float r_vertical = inputs.Right_Stick_Veritcal;
        float r_horizontal = inputs.Right_Stick_Horizontal;
        float heading = Mathf.Atan2(r_vertical, r_horizontal);

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

	private void Update()
	{
		gun_animator.SetBool ("Sucking", suck);
	}

	void Start()
	{
		gun_animator = GetComponentInChildren<Animator>();
	}
}