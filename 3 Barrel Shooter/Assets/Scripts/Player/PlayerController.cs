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

    bool suckLeft;
    bool shootLeft;

    bool suckRight;
    bool shootRight;

    public bool Burning = false;
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
        // Vacuum Sucking
        suckLeft = inputs.Left_Bumper;
        suckRight = inputs.Right_Bumper;
        vacControl.HandleVacuumStateInput(suckLeft, suckRight);

        // Vacuum Shooting
        if (inputs.Left_Trigger > 0.75f || Input.GetKey(KeyCode.LeftShift)) { shootLeft = true; } else shootLeft = false;
        if (inputs.Right_Trigger > 0.75f || Input.GetKey(KeyCode.Space)) { shootRight = true; } else shootRight = false;
        vacControl.HandleShootStateInput(shootLeft, shootRight, GetPlayerName(), fairies);

        // Change player speed based on action
        if (suckLeft || suckRight || shootLeft || shootRight) { speed = 4f; }
        else { speed = 6f; }

        //Chamber changing
        int direction = 0;
        if (inputs.X_Button) { direction = 1; }
        else if (inputs.B_Button) { direction = -1; }
        vacControl.HandleChamberStateInput(direction);
    }


    public string GetPlayerName(){
        return playerBody.GetComponent<PlayerInfo>().GetPlayerName();
    }


    // This function is called every frame by the level manager, called in fixed update
    public void UpdatePlayerMovement(ControllerInputs inputs)
    {
        ////Change the position of the player
        horizontal = inputs.Left_Stick_Horizontal;
        vertical = -inputs.Left_Stick_Vertical;

        // gets rotation input from right stick 
        float r_vertical = inputs.Right_Stick_Vertical;
        float r_horizontal = inputs.Right_Stick_Horizontal;
        float heading = Mathf.Atan2(r_vertical, r_horizontal);

        //Change the position of the player
        Vector2 movement = new Vector2(horizontal * speed, vertical * speed);

		//changes the characters direction it faces
		if (r_horizontal < 0) {
			spriteR.flipX = true;
		}
		else if (r_horizontal > 0){
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

        playerBody.GetComponent<Rigidbody2D>().AddForce(heading * 800);
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