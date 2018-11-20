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
        //GetInput();
	}

    // Sets the vacuum state based on controller input
    public void HandleVacuumStateInput(bool state){
        v.SetVacuum(state);
        vacuumArea.enabled = state;
    }

    // Sets the chamber based on controller input
    public void HandleChamberStateInput(int direction){
        if (!v.GetVacuumOn())
            v.changeChamber(direction);
    }

    // Sets the shoot staten based on controller input
    public void HandleShootStateInput(bool isShooting){
        if (isShooting && !v.GetVacuumOn()){
            Vacuum.Chamber.InventoryInfo result = v.Shoot();

            //If element if -1, we could not shoot else can shoot
            if (result != null)
            {
                //Instantiate element!!
                ProjectileSpawner p = GetComponent<ProjectileSpawner>();
                p.GetComponent<ProjectileSpawner>().shootProjectile(result.GetElementID(), levelManager);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        string cName = collision.tag.Split('-')[1];
        int id = int.Parse(collision.tag.Split('-')[0]);
        v.AddToChamber(cName, id);
        //If the item was added to the chamber destroy, else spit it back out randomly
        Destroy(collision.gameObject);

        v.DebugVac();
    }
}
