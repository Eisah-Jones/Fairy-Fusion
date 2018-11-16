using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerBody;
    private bool facingRight = true;
    private bool flipped = false;
    [SerializeField]
    private float speed =5f;
    public string inputHorizontal = "Horizontal";
    public string inputVertical = "Vertical";
    float horizontal;
    float vertical;

    void Start()
    {
     
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw(inputHorizontal);
        float vertical = Input.GetAxisRaw(inputVertical);
        Debug.Log("Vert: " +vertical.ToString()+ "Horizont: "+ horizontal.ToString());


    }

    void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        Vector2 movement = new Vector2(horizontal * speed, vertical * speed);
        transform.Translate(movement * Time.deltaTime, Space.World);
   

    }

        //   // Use this for initialization
        //   void Start () {

        //}

        //// Update is called once per frame
        //void Update () {
        //       GetInput();
        //}

        //   public void Move(Vector2 direction)
        //   {
        //       transform.Translate(direction * speed * Time.deltaTime);
        //	if (flipped) {
        //		flipped = false;
        //           playerBody.transform.Rotate(0f, 180f, 0f);
        //	}

        //   }

        //   private void GetInput(){

        //       if (Input.GetKey(KeyCode.W)){
        //           Move(Vector2.up);
        //       }

        //       if (Input.GetKey(KeyCode.A))
        //       {
        //		if (facingRight) {
        //			facingRight = false;
        //			flipped = true;
        //		}
        //		Move(Vector2.left);

        //       }

        //       if (Input.GetKey(KeyCode.D))
        //       {
        //		if (!facingRight) {
        //			facingRight = true;
        //			flipped = true;
        //		}
        //           Move(Vector2.right);

        //       }

        //       if (Input.GetKey(KeyCode.S))
        //       {
        //           Move(Vector2.down);
        //       }
        //   }
    
}
