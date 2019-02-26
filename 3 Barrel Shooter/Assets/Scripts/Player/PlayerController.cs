using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBody;
    public GameObject fairies;
	public Animator player_animator;

    public LevelManager lm;
    public AudioSource audioSource;

    public string inputHorizontal = "Horizontal";
    public string inputVertical = "Vertical";
    public string shootButton = "ShootButton_P1";
    public string vacuumButton = "VacuumButton_P1";

    [SerializeField]
    private float speed = 2f;
    private float speedMultiplier = 1f;

    private float horizontal;
    private float vertical;

    private bool suckLeft;
    private bool shootLeft;

    private bool suckRight;
    private bool shootRight;

    private float last_heading;
    private FairyController vacControl;
	private SpriteRenderer spriteR;

    private PlayerAffector playerAffector;


    public void InitPlayerController(FairyController vc)
    {
        vacControl = vc;
        playerBody = gameObject;
        fairies = transform.Find("Fairies").gameObject;
        gameObject.AddComponent<PlayerAffector>();
        playerAffector = gameObject.GetComponent<PlayerAffector>();
        playerAffector.InitPlayerAffector(playerBody);

        spriteR = gameObject.GetComponent<SpriteRenderer>();
        lm = FindObjectOfType<LevelManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
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
        speedMultiplier = playerAffector.GetSpeedMultiplier();
        float horizontalSpeed = horizontal * speed * speedMultiplier;
        float verticalSpeed   = vertical * speed * speedMultiplier;
        Vector2 movement = new Vector2(horizontalSpeed, verticalSpeed);

        AnimatePlayer(r_vertical, r_horizontal, movement, heading, vertical, horizontal);
    }


    public void HandleEffects(List<string> effects, Transform t)
    {
        playerAffector.HandleEffects(effects, t);
    }


    private void SetAnimsFalse()
    {
        player_animator.SetBool("Up", false);
        player_animator.SetBool("Down", false);
        player_animator.SetBool("Side", false);
        spriteR.flipX = false;
    }


    private void AnimatePlayer(float vertical, float horizontal, Vector2 movement, float heading, float l_vertical = 0.0f, float l_horizontal =0.0f)
    {
        //changes the characters direction it faces
        //changes orientation to face side
        if (horizontal == -1 || horizontal == 1)
        {
            SetAnimsFalse();
            player_animator.SetBool("Side", true);
        }

        if (player_animator.GetBool("Side") == true)
        {
            if (horizontal < 0)
            {
                spriteR.flipX = true;
            }
            else if (horizontal > 0)
            {
                spriteR.flipX = false;
            }
        }

        //changes orientation to face up
        if (vertical == 1)
        {
            SetAnimsFalse();
            player_animator.SetBool("Up", true);
        }
        //changes orientation to face down
        if (vertical == -1)
        {
            SetAnimsFalse();
            player_animator.SetBool("Down", true);
        }

        transform.Translate(movement * Time.deltaTime, Space.World);
        Debug.Log("H: " + horizontal + ", V: " + vertical);
        
        if (!Mathf.Approximately(l_horizontal, 0f) || !Mathf.Approximately(l_vertical, 0f))
        {
            if (audioSource != null)
                lm.soundManager.PlaySoundByName(audioSource, "Grasswalk");
            player_animator.SetBool("Moving", true);

        }
        else if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            if (audioSource != null)
                lm.soundManager.StopSound(audioSource);
            player_animator.SetBool("Moving", false);
        }

        //change rotation of player if no input received keeps same rotation as last time it got input prevents snapping back to 0,0 heading
        if (heading != 0)
        {
            float last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, last_heading * Mathf.Rad2Deg);
        }
    }
}