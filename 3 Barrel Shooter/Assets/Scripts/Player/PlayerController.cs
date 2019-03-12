using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBody;
    public GameObject fairies;
	public Animator player_animator;
    public Rigidbody2D rb;
    public LevelManager lm;
    public AudioSource audioSource;
    public AudioSource audioSource1;

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

    private bool dash;

    private float last_heading;

    public DashState dashState;
    public float dashTimer;
    [SerializeField]
    private float maxTime; // dash cool down time
    [SerializeField]
    private float dashForce; 


    private FairyController vacControl;
	private SpriteRenderer spriteR;

    private PlayerAffector playerAffector;
    public bool isDead = false;

    public void InitPlayerController(FairyController vc)
    {
        vacControl = vc;
        playerBody = gameObject;
        fairies = transform.Find("Fairies").gameObject;
        gameObject.AddComponent<PlayerAffector>();
        playerAffector = gameObject.GetComponent<PlayerAffector>();
        playerAffector.InitPlayerAffector(playerBody);
        rb = GetComponent<Rigidbody2D>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        lm = FindObjectOfType<LevelManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource1 = gameObject.AddComponent<AudioSource>();
        maxTime = 2f;
        dashForce = 30f;
    }


    public void UpdatePlayerInputs(ControllerInputs inputs)
    {
        if (isDead) { return; }
        // Vacuum Sucking
        suckLeft = inputs.Left_Bumper;
        suckRight = inputs.Right_Bumper;
        vacControl.HandleVacuumStateInput(suckLeft, suckRight);

        // Vacuum Shooting
        if (inputs.Left_Trigger > 0.75f) { shootLeft = true; } else shootLeft = false;
        if (inputs.Right_Trigger > 0.75f) { shootRight = true; } else shootRight = false;
        vacControl.HandleShootStateInput(shootLeft, shootRight, GetPlayerName(), fairies);

        // Change player speed based on action
        if (suckLeft || suckRight || shootLeft || shootRight) { speed = 4f; }
        else { speed = 6f; }

        //Chamber changing
        //int direction = 0;
        //if (inputs.X_Button) { direction = 1; }
        //else if (inputs.B_Button) { direction = -1; }
        //vacControl.HandleChamberStateInput(direction);
    }


    public string GetPlayerName(){
        return playerBody.GetComponent<PlayerInfo>().GetPlayerName();
    }


    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }


    // This function is called every frame by the level manager, called in fixed update
    public void UpdatePlayerMovement(ControllerInputs inputs)
    {
        if (isDead) { return; }
        ////Change the position of the player
        horizontal = inputs.Left_Stick_Horizontal;
        vertical = -inputs.Left_Stick_Vertical;

        dash = inputs.Right_Stick_Click;
        // gets rotation input from right stick 
        float r_vertical = inputs.Right_Stick_Vertical;
        float r_horizontal = inputs.Right_Stick_Horizontal;
        float heading = Mathf.Atan2(r_vertical, r_horizontal);

        //Change the position of the player
        speedMultiplier = playerAffector.GetSpeedMultiplier();
        float horizontalSpeed = horizontal * speed * speedMultiplier;
        float verticalSpeed   = vertical * speed * speedMultiplier;

        Vector2 movement = new Vector2(horizontalSpeed, verticalSpeed);

        AnimatePlayer(r_vertical, r_horizontal, movement, heading, dash, vertical, horizontal);

        
    }


    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }


    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
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

    public void StartDeathAnimation()
    {
        isDead = !isDead;
        player_animator.SetTrigger("Death");
       
    }
    
    public void SetRevive()
    {
        player_animator.SetTrigger("Revive");
        isDead = !isDead;
    }

    private void AnimatePlayer(float vertical, float horizontal, Vector2 movement, float heading, bool dash, float l_vertical = 0.0f, float l_horizontal =0.0f)
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
        if ( heading == 0 && horizontal > 0)
        {
            last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, 1f);
        }
        else if (heading != 0 )
        {
            last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, last_heading * Mathf.Rad2Deg);
        }
    
        switch (dashState) // initiates dash mechanic
        {
            case DashState.Ready:
                
                if (dash) // if dash input is detected use last heading to calculate dash direction
                {
                    //Debug.Log("Heading: " + heading);
                    //Debug.Log("Last Heading: " + last_heading);
                    Vector2 dir = RadianToVector2(last_heading);
                    rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
                    lm.soundManager.PlaySoundByName(audioSource1, "Dash");
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                
                dashTimer += Time.deltaTime * 3;
                if (dashTimer >= maxTime)
                {
                    dashTimer = maxTime;
                    dashState = DashState.Cooldown;
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }
}