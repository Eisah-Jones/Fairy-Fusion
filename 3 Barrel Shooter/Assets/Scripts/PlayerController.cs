using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
    }

    private void GetInput(){

        if (Input.GetKey(KeyCode.W)){
            Move(Vector2.up);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector2.left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector2.right);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Move(Vector2.down);
        }
    }
}
