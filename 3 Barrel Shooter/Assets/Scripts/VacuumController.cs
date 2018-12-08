using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour {

    LevelManager levelManager;
    Vacuum v;
    BoxCollider2D vacuumArea;
    Transform projectileSpawner;
    public GameObject throwTransform;
    bool canShoot;
    int shootNum = 10;
    int tempShoot = 0;

    //Sets the vacuum for the player, should be called after player gameObject instantiation 
    public void SetVacuum(Vacuum vac, LevelManager lm){
        v = vac;
        vacuumArea = GetComponent<BoxCollider2D>();
        projectileSpawner = transform.Find("ProjectileSpawn").transform;
        levelManager = lm;
        canShoot = true;
    }

    // Use this for initialization
    void Start () {
// flamethrower.Stop();
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
    public void HandleShootStateInput(bool isShooting, string playerName){

        if (!canShoot)
            return;

        canShoot = false; // temp fire rate setup

        //Chamber is empty, puff air
        if (v.GetCurrentChamber().GetAmountByIndex(0) == -1)
        {
            
        }

        else if (isShooting && !v.GetVacuumOn()){
            Vacuum.Chamber.InventoryInfo result = v.Shoot();
            int eID = result.GetElementID();
            string eName = result.GetElementName();
            string projectileType = levelManager.elementManager.GetProjectileTypeByID(eID);
            ProjectileSpawner p = GetComponent<ProjectileSpawner>();
            // Check to see if we are shooting a fluid
            if (projectileType == "Fluid"){
                int shotResult = p.ShootFluid(eID, levelManager, playerName, projectileSpawner);
                if (shotResult != -1) // Something was shot, change chamber
                {
                    v.RemoveFromCurrentChamber(eName, shotResult);
                }
            }

            else
            {
                p.ShootProjectile(eID, levelManager, playerName);
                v.SetCombinationChambers();
            }
        }
    }


    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Walls" || collision.tag == "Player") return;

        string cName = collision.tag.Split('-')[1];
        int id = int.Parse(collision.tag.Split('-')[0]);
        int result = v.AddToChamber(cName, id);

        if (result == -1){
            return;
        }
        //If the item was added to the chamber destroy, else spit it back out randomly

        Destroy(collision.gameObject);
        v.SetCombinationChambers();
    }
}
