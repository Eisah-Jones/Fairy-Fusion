using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour {

    LevelManager levelManager;
    Vacuum v;
    BoxCollider2D vacuumArea;

    //Sets the vacuum for the player, should be called after player gameObject instantiation 
    public void SetVacuum(Vacuum vac, LevelManager lm){
        v = vac;
        vacuumArea = GetComponent<BoxCollider2D>();
        levelManager = lm;
    }

	// Use this for initialization
	void Start () {
		
    }
	
	// Update is called once per frame
	void Update () {
        //Get the user input
        GetInput();
	}

    private void GetInput(){

        //Save whether or not player is shooting
        bool isRMouseDown = Input.GetMouseButtonDown(1);
        //FIRST we want to see if the player is sucking
        bool isLMouseDown = Input.GetMouseButton(0);
        v.SetVacuum(isLMouseDown);
        vacuumArea.enabled = isLMouseDown;

        // For debugging purposes
        //if (isLMouseDown)
            //Debug.Log(vacuumArea.enabled);

        //If q is pressed and vacuum isn't already on, switch chamber left
        if (Input.GetKeyDown (KeyCode.Q) && !v.GetVacuumOn ()) {
			v.changeChamber (-1);
        } //If e is pressed and vacuum isn't already on, switch chamber right
        else if (Input.GetKeyDown (KeyCode.E) && !v.GetVacuumOn ()) {
			v.changeChamber (1);
		} //If RMB is pressed and vacuum isn't already on, shoot from the current chamber
        else if (isRMouseDown && !v.GetVacuumOn ()) {
            int elementID = v.Shoot();

            //If element if -1, we could not shoot else can shoot
            if (elementID != -1){
                //Instantiate element!!
                ProjectileSpawner p = GetComponent<ProjectileSpawner>();
                p.GetComponent<ProjectileSpawner>().shootProjectile(elementID, levelManager);
            }
		}
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        v.AddToChamber(int.Parse(collision.gameObject.tag[0].ToString()));
        //If the item was added to the chamber destroy, else spit it back out randomly
        Destroy(collision.gameObject);

        v.DebugVac();
    }
}
