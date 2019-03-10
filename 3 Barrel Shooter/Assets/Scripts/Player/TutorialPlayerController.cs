using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D playerRigidBody;
    public GameObject fairies;
    private PlayerAffector playerAffector;
    private float speed = 2.5f;

    public void InitTutorialController(LevelManager lm)
    {
        player = transform.gameObject;
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        fairies = transform.Find("Fairies").gameObject;
    }


    public void Stage1(float leftHorizontal, float leftVertical, float rightHorizontal, float rightVertical)
    {

        // gets rotation input from right stick
        float heading = Mathf.Atan2(rightVertical, rightHorizontal);

        //Change the position of the player
        float horizontalSpeed = leftHorizontal * speed;
        float verticalSpeed = -leftVertical * speed;

        Vector2 movement = new Vector2(horizontalSpeed, verticalSpeed);

        //AnimatePlayer(rightVertical, rightHorizontal, movement, heading, leftVertical, leftHorizontal);

        // Move the player
        transform.Translate(movement * Time.deltaTime, Space.World);

        //change rotation of player if no input received keeps same rotation as last time it got input prevents snapping back to 0,0 heading
        if (heading == 0 && leftHorizontal > 0)
        {
            float last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, 1f);
        }
        else if (heading != 0)
        {
            float last_heading = heading;
            fairies.transform.rotation = Quaternion.Euler(0f, 0f, last_heading * Mathf.Rad2Deg);
        }
    }
}
