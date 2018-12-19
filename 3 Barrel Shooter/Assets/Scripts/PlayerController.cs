using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBody;
	public Animator player_animator;
	private Animator gun_animator;


    [SerializeField]
    private float speed =20f;
    public string inputHorizontal = "Horizontal";
    public string inputVertical = "Vertical";
    public string shootButton = "ShootButton_P1";
    public string vacuumButton = "VacuumButton_P1";
    float horizontal;
    float vertical;
    bool suck = false;
    bool shoot = false;
    bool combinationToggle = false; //If true then canshoot combination, else cannot
    float last_heading;

    private VacuumController vacControl;

    public void InitPlayerController(VacuumController vc){
        vacControl = vc;
        playerBody = this.gameObject;
    }

    public void UpdatePlayerInputs(ControllerInputs inputs)
    {
        if (inputs.Left_Stick_Click || Input.GetKeyDown(KeyCode.C)){
            combinationToggle = !combinationToggle;
            vacControl.SetIsCombiningElements(combinationToggle);
        }

        if (inputs.Left_Trigger == 1 || Input.GetKey(KeyCode.LeftShift)) { suck = true; } else suck = false;
        if (inputs.Right_Trigger == 1 || Input.GetKey(KeyCode.Space)) { shoot = true; } else shoot = false;

        vacControl.HandleVacuumStateInput(suck);
        vacControl.HandleShootStateInput(shoot, GetPlayerName());

        bool r_bumper = inputs.Right_Bumper;
        bool l_bumper = inputs.Left_Bumper;

        int d = 0;

        if (r_bumper || Input.GetKeyDown(KeyCode.R)) { d = 1; } else if (l_bumper || Input.GetKeyDown(KeyCode.E)) { d = -1; }

        vacControl.HandleChamberStateInput(d);
    }


    public string GetPlayerName(){
        return playerBody.GetComponent<PlayerInfo>().GetPlayerName();
    }


    // This function is called every frame by the level manager, called in fixed update
    public void UpdatePlayerMovement(ControllerInputs inputs)
    {
        // UNCOMMENT HERE FOR CONTROLLER INPUT
        ////Change the position of the player
        //horizontal = inputs.Left_Stick_Horizontal;
        //vertical = -inputs.Left_Stick_Vertical;
        //// gets rotation input from right stick 
        //float r_vertical = inputs.Right_Stick_Vertical;
        //float r_horizontal = inputs.Right_Stick_Horizontal;
        //float heading = Mathf.Atan2(r_vertical, r_horizontal);

        //COMMENT/UNCOMMENT HERE FOR Keyboard input
        float r_vertical = Input.GetAxisRaw("Vertical");
        float r_horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = r_vertical; 
        float horizontal = r_horizontal;
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


    // #### BELOW ARE PLAYER EFFECTS!! ####

    public void HandleEffects(List<string> effects, Transform t){
        foreach (string e in effects){
            if (e == "Knockback"){
                Knockback(t);
            }

            else if (e == "Burn")
            {
                Burn(t);
            }
        }
    }


    private void Knockback(Transform t){
        Vector2 heading = playerBody.transform.position - t.position;
        heading = heading.normalized;

        playerBody.GetComponent<Rigidbody2D>().AddForce(heading * 500);
    }


    private void Burn(Transform t){
        StartCoroutine("burnCoroutine");
    }

    private IEnumerator burnCoroutine(){

        int burnHits = Random.Range(1, 5);
        for (int i = 0; i < 5; i++ )
        {
            int hitPoints = Random.Range(1, 3);
            gameObject.GetComponent<PlayerInfo>().RemovePlayerHealth(hitPoints);
            float waitTime = Random.Range(0.5f, 2.0f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}