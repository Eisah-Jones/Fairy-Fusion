using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour {

    Vacuum v;
    BoxCollider2D vacuumArea;


	// Use this for initialization
	void Start () {
        v = new Vacuum();
        vacuumArea = GetComponent<BoxCollider2D>();
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

        if (Input.GetKeyDown(KeyCode.Q) && !v.GetVacuumOn())
        {
            v.changeChamber(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !v.GetVacuumOn())
        {
            v.changeChamber(1);
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
}
