﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FairyController : MonoBehaviour {

    LevelManager levelManager;
    Fairies fairies;
    BoxCollider2D fairyArea;
    Transform projectileSpawner;
    public GameObject throwTransform; // Is this needed?
    bool canShootRight;
    bool canShootLeft;
    bool canShootCombo;
    public AudioSource audioSource;
    public float maxFairyScale;
    private Vector3 originalFairyScale;
    bool suckLeft;
    bool suckRight;

    public List<GameObject> fairyPrefabs;
    private GameObject[] currentFairies;
    private GameObject[] fairyPositions;

    private bool isDelayingFrame;
    private int frameDelayNum;

    //Sets the vacuum for the player, should be called after player gameObject instantiation 
    public void SetVacuum(Fairies far, LevelManager lm)
    {
        fairies = far;
        projectileSpawner = transform.GetChild(0);

        LoadFairies();
        AssignFairies();

        fairyArea = GetComponent<BoxCollider2D>();
        levelManager = lm;
        canShootRight = true;
        canShootLeft = true;
        canShootCombo = true;

        isDelayingFrame = true;
        frameDelayNum = 2;
    }


    // Use this for initialization
    void Start () {
        audioSource = GetComponentInParent<AudioSource>();
    }
    

    // Update is called once per frame
    void Update () {
        UpdateFairies();
    }


    public void LoadFairies()
    {
        TextAsset txt = (TextAsset)Resources.Load("Fairies/loadFairies", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        fairyPrefabs = new List<GameObject>();

        foreach (string line in lines)
        {
            fairyPrefabs.Add(Resources.Load<GameObject>("Fairies/" + line));
        }
    }


    public void AssignFairies()
    {
        currentFairies = new GameObject[3];
        currentFairies[0] = Instantiate(fairyPrefabs[0]);
        currentFairies[1] = Instantiate(fairyPrefabs[0]);
        currentFairies[2] = Instantiate(fairyPrefabs[0]);

        fairyPositions = new GameObject[5];
        fairyPositions[0] = transform.GetChild(1).gameObject;
        fairyPositions[1] = transform.GetChild(2).gameObject;
        fairyPositions[2] = transform.GetChild(3).gameObject;

        currentFairies[0].transform.position = fairyPositions[0].transform.position;
        currentFairies[1].transform.position = fairyPositions[1].transform.position;
        currentFairies[2].transform.position = fairyPositions[2].transform.position;
    }


    private void SetFairyPos()
    {
        int chamberIndex1 = fairies.GetCurrentChamberIndex();
        int chamberIndex2 = (chamberIndex1 + 1) % 3;
        int chamberIndex3 = (chamberIndex2 + 1) % 3;

        currentFairies[chamberIndex1].transform.position = fairyPositions[0].transform.position;
        currentFairies[chamberIndex2].transform.position = fairyPositions[1].transform.position;
        currentFairies[chamberIndex3].transform.position = fairyPositions[2].transform.position;
    }


    private void UpdateFairyPos()
    {
        int chamberIndex1 = fairies.GetCurrentChamberIndex();
        int chamberIndex2 = (chamberIndex1 + 1) % 3;
        int chamberIndex3 = (chamberIndex2 + 1) % 3;

        currentFairies[chamberIndex1].transform.position = Vector2.MoveTowards(currentFairies[chamberIndex1].transform.position, fairyPositions[0].transform.position, 10f * Time.deltaTime);
        currentFairies[chamberIndex2].transform.position = Vector2.MoveTowards(currentFairies[chamberIndex2].transform.position, fairyPositions[1].transform.position, 10f * Time.deltaTime);
        currentFairies[chamberIndex3].transform.position = Vector2.MoveTowards(currentFairies[chamberIndex3].transform.position, fairyPositions[2].transform.position, 10f * Time.deltaTime);
    }


    public float GetScaleSize( float scalePercent)
    {
        return scalePercent * (0.35f);
    }


    private void UpdateFairySize()
    {
        Fairies.Fairy[] chambers = fairies.GetChambers();
        for (int i = 0; i < 3; i++)
        {
            Fairies.Fairy c = chambers[i];
            Transform[] fairyTs = currentFairies[i].GetComponentsInChildren<Transform>();
            elementData ed = levelManager.elementManager.GetElementDataByID(c.GetElementIDByIndex(0)) ;
            if (ed != null && c.GetAmountByIndex(0) == 0)
            {
                fairyTs[1].localScale = new Vector3(.3f,.3f,.3f);
                fairyTs[2].localScale = new Vector3(.8f, .8f, .8f);
                fairyTs[3].localScale = new Vector3(.3f,.3f,.3f);
            }
            else if (ed != null)
            {
                float scalePercent = (float)c.GetAmountByIndex(0) / ed.chamberCapacity;
                maxFairyScale = 1 * scalePercent+.3f;
                float scale = GetScaleSize(scalePercent) + .3f;
                if (scale >=.65f)
                {
                    fairyTs[1].localScale = new Vector3(.65f, .65f, .65f);
                    fairyTs[3].localScale = new Vector3(.65f, .65f, .65f);
                }
                else
                {
                    fairyTs[1].localScale = new Vector3(scale, scale, scale);
                    fairyTs[3].localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }


    private void UpdateFairies(){
        Fairies.Fairy[] chambers = fairies.GetChambers();
        for (int i = 0; i < 3; i++)
        {
            Fairies.Fairy c = chambers[i];

            // If the chamber is empty, spawn in white fairy
            if (c.GetAmountByIndex(0) == -1)
            {
                if (currentFairies[i].tag != fairyPrefabs[0].tag)
                {
                    Destroy(currentFairies[i]);
                    currentFairies[i] = Instantiate(fairyPrefabs[0]);
                    SetFairyPos();
                }
            }
            else // If the chamber is not empty, make sure the correct fairy is still there
            {
                if (currentFairies[i].tag != fairyPrefabs[c.GetContents()[0].GetElementID()].tag)
                {
                    Destroy(currentFairies[i]);
                    currentFairies[i] = Instantiate(fairyPrefabs[c.GetContents()[0].GetElementID()]);
                    SetFairyPos();
                }
            }
        }
        UpdateFairySize();
        UpdateFairyPos();
    }


    // Sets the vacuum state based on controller input
    public void HandleVacuumStateInput(bool stateLeft, bool stateRight){

        suckLeft = stateLeft;
        suckRight = stateRight;

        if ((stateLeft || stateRight) != fairyArea.enabled)
        {
            fairies.SetVacuum(stateLeft, stateRight);
            fairyArea.enabled = stateLeft || stateRight;
            levelManager.soundManager.PlaySoundsByID(audioSource, 1);
            if (!(stateLeft || stateRight))
            {
                levelManager.soundManager.StopSound(audioSource);
            }
        }
    }


    // Sets the chamber based on controller input
    public void HandleChamberStateInput(int direction){
        if (!fairies.GetVacuumOn())
            fairies.changeChamber(direction);
    }


    // Sets the shoot staten based on controller input
    public void HandleShootStateInput(bool shootingLeft, bool shootingRight, string playerName, GameObject f)
    {
        fairies.SetShootingBools(shootingLeft, shootingRight);

        // Frame delay to make shooting combos easier
        if (isDelayingFrame)
        {
            StartCoroutine("StartFrameDelay");
            return;
        }
        isDelayingFrame = true;

        if ((!canShootRight && !canShootLeft) || fairies.GetVacuumOn() || (!shootingLeft && !shootingRight)) { return; }

        //canShoot = false; // temp fire rate setup

        Fairies.Fairy.InventoryInfo result = null;

        if (shootingLeft && shootingRight)
        {
            if (!canShootCombo) return;
            //Shoot a combination of the two chambers
            result = fairies.Shoot(true, -1);
            if (result != null)
            {
                canShootCombo = false;
                result.GetElementName();
                IEnumerator c = ShootReset(true, true, (int)levelManager.elementManager.GetElementDataByID(result.GetElementID()).fireRate);
                StartCoroutine(c);
            }
        }
        else if (shootingLeft)
        {
            //Shoot the first chamber
            if (!canShootLeft) return;
            result = fairies.Shoot(false, 0);
            if (result != null)
            {
                canShootLeft = false;
                result.GetElementName();
                IEnumerator c = ShootReset(true, false, (int)levelManager.elementManager.GetElementDataByID(result.GetElementID()).fireRate);
                StartCoroutine(c);
            }
        }
        else
        {
            //Shoot the second chamber
            if (!canShootRight) return;
            result = fairies.Shoot(false, 1);
            if (result != null)
            {
                canShootRight = false;
                result.GetElementName();
                IEnumerator c = ShootReset(false, false, (int)levelManager.elementManager.GetElementDataByID(result.GetElementID()).fireRate);
                StartCoroutine(c);
            }
        }

        if (result == null) { return; }

        int eID = result.GetElementID();
        string eName = result.GetElementName();
        string projectileType = levelManager.elementManager.GetProjectileTypeByID(eID);
        ProjectileSpawner p = GetComponent<ProjectileSpawner>();
        if (projectileType == "Fluid")
        {
            string o = projectileSpawner.parent.parent.GetComponent<PlayerInfo>().GetPlayerName();
            int shotResult = p.ShootFluid(eID, levelManager, playerName, projectileSpawner, o);
            if (shotResult != -1) // Something was shot, update chamber
            {
                fairies.RemoveFromCurrentChamber(eName, shotResult);
            }
        }
        else
        {
            p.ShootProjectile(eID, levelManager, playerName, projectileSpawner);
            fairies.RemoveFromCurrentChamber(eName, 1);
            fairies.SetCombinationChambers();
        }
    }


    public IEnumerator ShootReset(bool isLeft, bool isCombo, int resetFrames)
    {
        for (int i = 0; i < resetFrames; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (isCombo) canShootCombo = true;
        else if (isLeft) canShootLeft = true;
        else canShootRight = true;
    }


    public IEnumerator StartFrameDelay()
    {
        for (int i = 0; i < frameDelayNum; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        isDelayingFrame = false;
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged") return;

        if (collision.tag == "TEST") 
        {
            if (levelManager.GetTriggerTile((int)projectileSpawner.position.x, (int)projectileSpawner.position.y) == "Water")
            {
                fairies.AddToChamber("Water", 3, suckLeft);
            }
            fairies.SetCombinationChambers();
            return;
        }

        string[] collisionInfo = collision.tag.Split('-');
        int result = -1;

        if (collisionInfo[0] == "R") // The player is harvesting some resource
        {
            Resource r = collision.GetComponent<Resource>();
            Shaker s = collision.GetComponent<Shaker>();
            if (r.CanCollect() && (fairies.IsCurrentChamberEmpty(true) || fairies.GetCurrentChamber(true).GetElementNameByIndex(0) == collisionInfo[2]))
            {
                s.beingSucked = true; // shakes the resource
          
                result = fairies.AddToChamber(collisionInfo[2], int.Parse(collisionInfo[1]), suckLeft);
                r.DecrementResource();
            }
        }
        fairies.SetCombinationChambers();
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        Shaker s = other.GetComponent<Shaker>();

        if (!fairies.GetVacuumOn() && s != null)
        {
            s.beingSucked = false;
        }
  
    }


}