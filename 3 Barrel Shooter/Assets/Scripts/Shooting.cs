using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

	public Transform firepoint;
	public GameObject Fire_bullet;
	public GameObject Water_bullet;
	public GameObject Rock_bullet;
	public float Fire_delay = 0.5f;
	public float Rock_delay = 0.5f;
	public float Water_delay = 0.5f;

	private float next_fire = 0.0f;

	public void Fire(){
		if (Time.time > next_fire) {
			Instantiate (Fire_bullet, firepoint.position, firepoint.rotation);
			next_fire = Time.time+Fire_delay;
		}
	}

	public void Water(){
		if (Time.time > next_fire) {
			Instantiate (Water_bullet, firepoint.position, firepoint.rotation);
			next_fire = Time.time+Water_delay;
		}
	}

	public void Rock(){
		if (Time.time > next_fire) {
			Instantiate (Rock_bullet, firepoint.position, firepoint.rotation);
			next_fire = Time.time+Rock_delay;
		}
	}
}
