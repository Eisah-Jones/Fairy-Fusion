using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D playerRigidBody;
    public GameObject fairies;
    private PlayerAffector playerAffector;
    private Animator player_animator;
    private float speed = 2.5f;
    private float last_heading;
    private SpriteRenderer spriteR;
    private AudioSource audioSource;

    private TutorialMenu tm;
    private TutorialFairies tf;

    public bool hasMoved = false;
    public bool hasAimed = false;
    public bool hasDashed = false;
    public bool hasHarnessedLeft = false;
    public bool hasCastedLeft = false;
    public bool hasHarnessedRight = false;


    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    public DashState dashState;
    public float dashTimer;
    private float dashForce = 30f;
    private float maxTime = 5f;


    public void InitTutorialController()
    {
        player = transform.gameObject;
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        player_animator = player.GetComponent<Animator>();
        spriteR = player.GetComponent<SpriteRenderer>();
        fairies = transform.Find("Fairies").gameObject;
        tf = fairies.GetComponent<TutorialFairies>();
        tf.InitTutorialFairies();
        audioSource = player.GetComponent<AudioSource>();
    }


    public void HandleStage(int s, float leftHorizontal, float leftVertical, float rightHorizontal, float rightVertical, bool dash, bool leftBumper, bool leftTrigger, bool rightBumper)
    {

        // gets rotation input from right stick
        float heading = Mathf.Atan2(rightVertical, rightHorizontal);

        //Change the position of the player
        float horizontalSpeed = leftHorizontal * speed;
        float verticalSpeed = -leftVertical * speed;

        Vector2 movement = new Vector2(horizontalSpeed, verticalSpeed);

        AnimatePlayer(rightVertical, rightHorizontal, movement, heading, false, leftVertical, leftHorizontal);

        // Move the player
        transform.Translate(movement * Time.deltaTime, Space.World);

        //change rotation of player if no input received keeps same rotation as last time it got input prevents snapping back to 0,0 heading
        if (heading == 0 && rightHorizontal > 0)
        {
            last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, 1f);
        }
        else if (heading != 0)
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
                    playerRigidBody.AddForce(dir * dashForce, ForceMode2D.Impulse);
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

        switch (s)
        {
            case 1:
                if (horizontalSpeed > 0 || verticalSpeed > 0) hasMoved = true;
                if (heading != 0f) hasAimed = true;
                break;
            case 2:
                if (!hasDashed && dash) hasDashed = dash;
                break;
            case 3:
                if (!hasHarnessedLeft && leftBumper)
                {
                    tf.SetHarnessArea(leftBumper);
                }
                else if (!hasHarnessedLeft)
                {
                    hasHarnessedLeft = tf.GetHarnessedLeft();
                }
                break;
            case 4:
                if (!hasCastedLeft && leftTrigger)
                {
                    StartCoroutine("SetCasting");
                    tf.ShootFire(true);
                }
                else if (hasCastedLeft)
                {
                    tf.ShootFire(false);
                }
                break;
            case 5:
                if (!hasHarnessedRight && rightBumper)
                {
                    tf.SetHarnessArea(rightBumper);
                }
                else if (!hasHarnessedRight)
                {
                    hasHarnessedRight = tf.GetHarnessedRight();
                }
                break;
        }
    }


    public void SetAnimsFalse()
    {
        player_animator.SetBool("Up", false);
        player_animator.SetBool("Down", false);
        player_animator.SetBool("Side", false);
        spriteR.flipX = false;
    }


    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }


    private IEnumerator SetCasting()
    {
        yield return new WaitForSeconds(2f);
        hasCastedLeft = true;
    }


    private void AnimatePlayer(float vertical, float horizontal, Vector2 movement, float heading, bool dash, float l_vertical = 0.0f, float l_horizontal = 0.0f)
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
            //if (audioSource != null)
                //levelManager.soundManager.PlaySoundByName(audioSource, "Grasswalk");
            player_animator.SetBool("Moving", true);

        }
        else if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
        {
            //Debug.Log("LM: " + levelManager);
            //if (audioSource != null)
                //levelManager.soundManager.StopSound(audioSource);
            player_animator.SetBool("Moving", false);
        }
        //change rotation of player if no input received keeps same rotation as last time it got input prevents snapping back to 0,0 heading
        if (heading == 0 && horizontal > 0)
        {
            last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, 1f);
        }
        else if (heading != 0)
        {
            last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, last_heading * Mathf.Rad2Deg);
        }
    }
}
