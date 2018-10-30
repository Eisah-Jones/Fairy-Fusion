using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	private bool facingRight = true;
	private bool flipped = false;
    [SerializeField]
    private float speed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
	}

    public void Move(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
		if (flipped) {
			flipped = false;
			transform.Rotate (0f, 180f, 0f);
		}
			
    }

    private void GetInput(){

        if (Input.GetKey(KeyCode.W)){
            Move(Vector2.up);
        }

        if (Input.GetKey(KeyCode.A))
        {
			if (facingRight) {
				facingRight = false;
				flipped = true;
			}
			Move(Vector2.right);

        }

        if (Input.GetKey(KeyCode.D))
        {
			if (!facingRight) {
				facingRight = true;
				flipped = true;
			}
            Move(Vector2.right);

        }

        if (Input.GetKey(KeyCode.S))
        {
            Move(Vector2.down);
        }
    }
}
