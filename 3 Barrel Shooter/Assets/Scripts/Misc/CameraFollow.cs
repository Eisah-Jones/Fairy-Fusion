using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject player;
	private Camera cam;
	public int cam_num;
	public int player_count;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
		switch(cam_num){
		case 1:
			if (player_count >= 1) {
				player = GameObject.Find ("Player0");
			} else
				cam.enabled = false;
			break;
		case 2:
			if (player_count >= 2) {
				player = GameObject.Find ("Player1");
			} else
				cam.enabled = false;
			break;

		case 3:
			if (player_count >= 3) {
				player = GameObject.Find ("Player0");
			} else
				cam.enabled = false;
			break;
		case 4:
			if (player_count >= 4) {
				player = GameObject.Find ("Player1");
			} else
				cam.enabled = false;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			transform.position = new Vector3(player.transform.position.x,player.transform.position.y,transform.position.z);
		}
		else {
			cam = GetComponent<Camera>();
			switch(cam_num){
			case 1:
				if (player_count >= 1) {
					player = GameObject.Find ("Player0");
				} else
					cam.enabled = false;
				break;
			case 2:
				if (player_count >= 2) {
					player = GameObject.Find ("Player1");
				} else
					cam.enabled = false;
				break;
			
			case 3:
				if (player_count >= 3) {
					player = GameObject.Find ("Player0");
				} else
					cam.enabled = false;
				break;
			case 4:
				if (player_count >= 4) {
					player = GameObject.Find ("Player1");
				} else
					cam.enabled = false;
				break;
			}
		}
	}

}
