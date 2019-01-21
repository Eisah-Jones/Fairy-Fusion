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
    public void SetVacuum(Vacuum vac, LevelManager lm)
    {
        v = vac;

        foreach(Transform child in transform)
        {
            if (child.tag == "ProjectileSpawn") { projectileSpawner = child.GetChild(0); break; }
        }

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
        if (state != vacuumArea.enabled)
        {
            v.SetVacuum(state);
            vacuumArea.enabled = state;
            levelManager.soundManager.PlaySoundsByID(audioSource, 1);
            if (!state)
            {
                levelManager.soundManager.StopSound(audioSource);
            }
        }
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

        // Chamber is empty, TODO: puff air
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
                if (shotResult != -1) // Something was shot, update chamber
                {
                    v.RemoveFromCurrentChamber(eName, shotResult);
                }
            }

            else
            {
                p.ShootProjectile(eID, levelManager, playerName);
                v.RemoveFromCurrentChamber(eName, 1);
                v.SetCombinationChambers();
            }
        }
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged") return;

        if (collision.tag == "TEST") 
        {
            if (levelManager.GetTriggerTile((int)projectileSpawner.position.x, (int)projectileSpawner.position.y) == "Water")
            {
                v.AddToChamber("Water", 3);
            }
            return;
        }

        string[] collisionInfo = collision.tag.Split('-');
        int result = -1;

        if (collisionInfo[0] == "R") // The player is harvesting some resource
        {
            Resource r = collision.GetComponent<Resource>();
            if (r.CanCollect())
            {
                result = v.AddToChamber(collisionInfo[2], int.Parse(collisionInfo[1]));
                r.DecrementResource();
            }

        } 
        else // We are picking up some projectile from the ground
        {
            result = v.AddToChamber(collisionInfo[1], int.Parse(collisionInfo[0]));
            if (result == -1) { return; } // TODO: Randomly launch projectile from character
            Destroy(collision.gameObject);
        }

        // Update chamber combinations
        v.SetCombinationChambers();
    }

}
