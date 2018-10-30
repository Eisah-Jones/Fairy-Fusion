using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour {
	
    Vacuum v;
    BoxCollider2D vacuumArea;
	Shooting shoot_script;

	private int shoot_type = 0;

	// Use this for initialization
	void Start () {
        v = new Vacuum();
        vacuumArea = GetComponent<BoxCollider2D>();
		shoot_script = GetComponent<Shooting> ();
    }
	
	// Update is called once per frame
	void Update () {
        //Get the user input
        GetInput();
	}

    private void GetInput(){
        //if (Input.GetMouseButtonDown(0))
        //{
        //    v.SetVacuum(true);
        //    vacuumArea.enabled = true;
        //}
        //else
        //{
        //    v.SetVacuum(false);
        //    vacuumArea.enabled = false;
        //}

        bool isLMouseDown = Input.GetMouseButton(0);
        Debug.Log(isLMouseDown);
        v.SetVacuum(isLMouseDown);
        vacuumArea.enabled = isLMouseDown;

		if (Input.GetKeyDown (KeyCode.Q) && !v.GetVacuumOn ()) {
			v.changeChamber (-1);
		} else if (Input.GetKeyDown (KeyCode.E) && !v.GetVacuumOn ()) {
			v.changeChamber (1);
		} else if (Input.GetKey (KeyCode.Mouse1) && !v.GetVacuumOn ()) {
			shoot_type = v.GetCurrentChamberElement ();
		}

		if (shoot_type != 0) {
			Shoot (shoot_type);
		}
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Rock")
        {
            v.AddToChamber(2);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Water")
        {
            v.AddToChamber(3);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Fire")
        {
            v.AddToChamber(1);
            Destroy(other.gameObject);
        }

        v.DebugVac();
    }


	void Shoot(int type){
		if (type == 1) {
			shoot_script.Fire ();
		} else if (type == 2) {
			shoot_script.Rock ();
		} else if (type == 3) {
			shoot_script.Water ();
		}
		shoot_type = 0;
	}
}
