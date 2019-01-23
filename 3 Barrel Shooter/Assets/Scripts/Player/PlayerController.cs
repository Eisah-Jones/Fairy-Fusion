using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBody;
    public GameObject fairies;
	public Animator player_animator;

    private bool isMoving = false;
    [SerializeField]
    private float speed = 2f;
    public string inputHorizontal = "Horizontal";
    public string inputVertical = "Vertical";
    public string shootButton = "ShootButton_P1";
    public string vacuumButton = "VacuumButton_P1";
    float horizontal;
    float vertical;
    bool suck = false;
    bool shoot = false;
    public bool Burning = false;
    bool combinationToggle = false; //If true then canshoot combination, else cannot
    float last_heading;
    public LevelManager lm;
    public AudioSource audioSource;
    private VacuumController vacControl;
	private SpriteRenderer spriteR;

    void Start()
    {
		spriteR = gameObject.GetComponent<SpriteRenderer>();
        lm = FindObjectOfType<LevelManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void InitPlayerController(VacuumController vc){
        vacControl = vc;
        playerBody = this.gameObject;
        fairies = transform.Find("Fairies").gameObject;
    }

    public void UpdatePlayerInputs(ControllerInputs inputs)
    {
        if (inputs.Left_Stick_Click || Input.GetKeyDown(KeyCode.C))
        {
            combinationToggle = !combinationToggle;
            vacControl.SetIsCombiningElements(combinationToggle);
        }

        if (inputs.Left_Trigger == 1 || Input.GetKey(KeyCode.LeftShift)) { suck = true; } else suck = false;
        if (inputs.Right_Trigger == 1 || Input.GetKey(KeyCode.Space)) { shoot = true; } else shoot = false;

        vacControl.HandleVacuumStateInput(suck);
        vacControl.HandleShootStateInput(shoot, GetPlayerName());

        if (suck || shoot) { speed = 4f; }
        else { speed = 6f; }


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
        horizontal = inputs.Left_Stick_Horizontal;
        vertical = -inputs.Left_Stick_Vertical;
        // gets rotation input from right stick 
        float r_vertical = inputs.Right_Stick_Vertical;
        float r_horizontal = inputs.Right_Stick_Horizontal;
        float heading = Mathf.Atan2(r_vertical, r_horizontal);

        //COMMENT/UNCOMMENT HERE FOR Keyboard input
//        float r_vertical = Input.GetAxisRaw("Vertical");
//        float r_horizontal = Input.GetAxisRaw("Horizontal");
//        float vertical = r_vertical; 
//        float horizontal = r_horizontal;
//        float heading = Mathf.Atan2(r_vertical, r_horizontal);

        //Change the position of the player
        Vector2 movement = new Vector2(horizontal * speed, vertical * speed);

		//changes the characters direction it faces
		if (horizontal < 0) {
			spriteR.flipX = true;
		}
		else if (horizontal > 0){
			spriteR.flipX = false;
		}




        transform.Translate(movement * Time.deltaTime, Space.World);
        if (!Mathf.Approximately(horizontal, 0f) || !Mathf.Approximately(vertical, 0f))
        {
            isMoving = true;
            if (audioSource!=null)
                lm.soundManager.PlaySoundsByID(audioSource, 3);
			player_animator.SetBool ("Moving", true);
           
        }
        else if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            if (audioSource != null)
                lm.soundManager.StopSound(audioSource);
			player_animator.SetBool ("Moving", false);
        }

        //change rotation of pplayer if no input received keeps same rotation as last time it got input prevents snapping back to 0,0 heading
        if (heading != 0)
        {
            float last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, last_heading * Mathf.Rad2Deg);
        }
    }

	private void Update()
	{
		
	}




    // #### BELOW ARE PLAYER EFFECTS!! ####

    public void HandleEffects(List<string> effects, Transform t){
        foreach (string e in effects){
            //Debug.Log("e: "+ e);
            if (e == "Knockback") { Knockback(t); }
            else if (e == "Burn") 
            {
                StartCoroutine("Burn");
                Burning = false;
            } // Burn coroutine currently does not work
            else if (e == "Pushback") { Pushback(t); }
        }
    }


    private IEnumerator ResetForces(){
        yield return new WaitForSeconds(0.05f);
        Vector2 v = playerBody.GetComponent<Rigidbody2D>().velocity;
        playerBody.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //Debug.Log(playerBody.GetComponent<Rigidbody2D>().velocity);
    }


    private void Knockback(Transform t){
        Vector2 heading = playerBody.transform.position - t.position;
        heading = heading.normalized;

        playerBody.GetComponent<Rigidbody2D>().AddForce(heading * 500);
        StartCoroutine("ResetForces");
    }


    private void Pushback(Transform t)
    {
        Vector2 heading = playerBody.transform.position - t.position;
        heading = heading.normalized;

        playerBody.GetComponent<Rigidbody2D>().AddForce(heading * 80);
        StartCoroutine("ResetForces");
    }

    private IEnumerator Burn(){

       
        int burnHits = Random.Range(1, 5);
        for (int i = 0; i < burnHits; i++ )
        {
            int hitPoints = Random.Range(1, 3);
            if (gameObject.GetComponent<PlayerInfo>().health <= 0)
            {
                Burning = false;
                yield return 0;

            }

            else if (Burning)
            {
                gameObject.GetComponent<PlayerInfo>().RemovePlayerHealth(hitPoints);
                float waitTime = Random.Range(0.5f, 2.0f);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}