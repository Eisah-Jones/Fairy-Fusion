using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour {

    LevelManager levelManager;
    Vacuum v;
    BoxCollider2D vacuumArea;

    bool canShoot;
    int shootNum = 10;
    int tempShoot = 0;

    //Sets the vacuum for the player, should be called after player gameObject instantiation 
    public void SetVacuum(Vacuum vac, LevelManager lm){
        v = vac;
        vacuumArea = GetComponent<BoxCollider2D>();
        levelManager = lm;
        canShoot = true;
    }

	// Use this for initialization
	void Start () {
		
    }
	
	// Update is called once per frame
	void Update () {
        tempShoot += 1;
        if (tempShoot > shootNum){
            canShoot = true;
            tempShoot = 0;
        }
	}

    public void SetIsCombiningElements(bool b)
    {
        v.SetIsCombiningElements(b);
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

        if (!canShoot)
            return;

        canShoot = false;

        if (isShooting && !v.GetVacuumOn()){
            Vacuum.Chamber.InventoryInfo result = v.Shoot();
            //If element null, we could not shoot else can shoot
            if (result != null)
            {
                //Instantiate element!!
                ProjectileSpawner p = GetComponent<ProjectileSpawner>();
                p.GetComponent<ProjectileSpawner>().shootProjectile(result.GetElementID(), levelManager);
                v.SetCombinationChambers();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.tag);

        if (collision.tag == "Walls" || collision.tag == "Player") return;

        string cName = collision.tag.Split('-')[1];
        int id = int.Parse(collision.tag.Split('-')[0]);
        v.AddToChamber(cName, id);
        //If the item was added to the chamber destroy, else spit it back out randomly

        Destroy(collision.gameObject);
        v.SetCombinationChambers();

        v.DebugVac();
    }
}
