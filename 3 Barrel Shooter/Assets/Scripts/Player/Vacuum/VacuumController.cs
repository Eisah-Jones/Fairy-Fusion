using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public AudioSource audioSource;

    public List<GameObject> fairies;
    private GameObject[] currentFairies;
    private GameObject[] fairyPositions;

    //Sets the vacuum for the player, should be called after player gameObject instantiation 
    public void SetVacuum(Vacuum vac, LevelManager lm)
    {
        v = vac;

        foreach(Transform child in transform)
        {
            if (child.tag == "ProjectileSpawn") { projectileSpawner = child.GetChild(0); break; }
        }


        TextAsset txt = (TextAsset)Resources.Load("Fairies/loadFairies", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        fairies = new List<GameObject>();

        foreach(string line in lines)
        {
            fairies.Add(Resources.Load<GameObject>("Fairies/" + line));
        }

        currentFairies = new GameObject[3];
        currentFairies[0] = Instantiate(fairies[0]);
        currentFairies[1] = Instantiate(fairies[0]);
        currentFairies[2] = Instantiate(fairies[0]);

        fairyPositions = new GameObject[5];
        fairyPositions[0] = transform.GetChild(0).gameObject;
        fairyPositions[1] = transform.GetChild(1).gameObject;
        fairyPositions[2] = transform.GetChild(2).gameObject;
        fairyPositions[3] = transform.GetChild(3).gameObject;
        fairyPositions[4] = transform.GetChild(4).gameObject;

        vacuumArea = GetComponent<BoxCollider2D>();
        levelManager = lm;
        canShoot = true;
    }

    // Use this for initialization
    void Start () {
        audioSource = GetComponentInParent<AudioSource>();

    }
    
    // Update is called once per frame
    void Update () {
        UpdateFairies();

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


    private void UpdateFairyPos(){

        int chamberIndex = v.GetCurrentChamberIndex();

        if (!v.GetIsCombiningElements())
        {
            for (int i = 0; i < 3; i++)
            {
                int j = (i + chamberIndex) % 3;
                currentFairies[j].transform.position = fairyPositions[i].transform.position;
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                int j = (i + chamberIndex) % 3;
                if (i == 0)
                {
                    currentFairies[j].transform.position = fairyPositions[3].transform.position;
                    currentFairies[(j + 1) % 3].transform.position = fairyPositions[4].transform.position;
                }
                else
                {
                    currentFairies[(j + 1) % 3].transform.position = fairyPositions[1].transform.position;
                }
            }
        }
    }

    private void UpdateFairies(){
        Vacuum.Chamber[] chambers = v.GetChambers();

        for (int i = 0; i < 3; i++)
        {
            Vacuum.Chamber c = chambers[i];
            if (c.GetAmountByIndex(0) == -1)
            {
                if (currentFairies[i].tag != fairies[0].tag)
                {
                    //Debug.Log("FFFF: " + currentFairies[i].name + ", " + fairies[0].name);
                    Destroy(currentFairies[i]);
                    currentFairies[i] = Instantiate(fairies[0]);
                }
            }
            else
            {
                if (currentFairies[i].tag != fairies[c.GetContents()[0].GetElementID()].tag)
                {
                    Destroy(currentFairies[i]);
                    currentFairies[i] = Instantiate(fairies[c.GetContents()[0].GetElementID()]);
                }
            }
        }

        UpdateFairyPos();
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
                p.ShootProjectile(eID, levelManager, playerName, projectileSpawner);
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
            v.SetCombinationChambers();
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
