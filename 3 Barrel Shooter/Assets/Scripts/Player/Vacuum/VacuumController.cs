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
    public float maxFairyScale;
    private Vector3 originalFairyScale;
    bool suckLeft;
    bool suckRight;

    public List<GameObject> fairies;
    private GameObject[] currentFairies;
    private GameObject[] fairyPositions;

    //Sets the vacuum for the player, should be called after player gameObject instantiation 
    public void SetVacuum(Vacuum vac, LevelManager lm)
    {
        v = vac;

        projectileSpawner = transform.GetChild(0);

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
        fairyPositions[0] = transform.GetChild(1).gameObject;
        fairyPositions[1] = transform.GetChild(2).gameObject;
        fairyPositions[2] = transform.GetChild(3).gameObject;

        currentFairies[0].transform.position = fairyPositions[0].transform.position;
        currentFairies[1].transform.position = fairyPositions[1].transform.position;
        currentFairies[2].transform.position = fairyPositions[2].transform.position;

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
        if (tempShoot > shootNum && !canShoot)
        {
            canShoot = true;
            tempShoot = 0;
        }

    }


    private void SetFairyPos()
    {
        int chamberIndex1 = v.GetCurrentChamberIndex();
        int chamberIndex2 = (chamberIndex1 + 1) % 3;
        int chamberIndex3 = (chamberIndex2 + 1) % 3;

        currentFairies[chamberIndex1].transform.position = fairyPositions[0].transform.position;
        currentFairies[chamberIndex2].transform.position = fairyPositions[1].transform.position;
        currentFairies[chamberIndex3].transform.position = fairyPositions[2].transform.position;
    }


    private void UpdateFairyPos()
    {
        int chamberIndex1 = v.GetCurrentChamberIndex();
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
        Vacuum.Chamber[] chambers = v.GetChambers();
        for (int i = 0; i < 3; i++)
        {
            Vacuum.Chamber c = chambers[i];
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
        Vacuum.Chamber[] chambers = v.GetChambers();
        for (int i = 0; i < 3; i++)
        {
            Vacuum.Chamber c = chambers[i];

            if (c.GetAmountByIndex(0) == -1)
            {
                if (currentFairies[i].tag != fairies[0].tag)
                {
                    Destroy(currentFairies[i]);
                    currentFairies[i] = Instantiate(fairies[0]);
                    SetFairyPos();
                }
            }
            else
            {
                if (currentFairies[i].tag != fairies[c.GetContents()[0].GetElementID()].tag)
                {
                    Destroy(currentFairies[i]);
                    currentFairies[i] = Instantiate(fairies[c.GetContents()[0].GetElementID()]);
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

        if ((stateLeft || stateRight) != vacuumArea.enabled)
        {
            v.SetVacuum(stateLeft, stateRight);
            vacuumArea.enabled = stateLeft || stateRight;
            levelManager.soundManager.PlaySoundsByID(audioSource, 1);
            if (!(stateLeft || stateRight))
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
    public void HandleShootStateInput(bool shootingLeft, bool shootingRight, string playerName, GameObject f)
    {
        v.SetShootingBools(shootingLeft, shootingRight);

        if (!canShoot || v.GetVacuumOn() || (!shootingLeft && !shootingRight)) { return; }

        canShoot = false; // temp fire rate setup

        Vacuum.Chamber.InventoryInfo result = null;

        if (shootingLeft && shootingRight)
        {
            //Shoot a combination of the two chambers
            result = v.Shoot(true, -1);
        }
        else if (shootingLeft)
        {
            //Shoot the first chamber
            result = v.Shoot(false, 0);
        }
        else
        {
            //Shoot the second chamber
            result = v.Shoot(false, 1);
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


    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged") return;

        if (collision.tag == "TEST") 
        {
            if (levelManager.GetTriggerTile((int)projectileSpawner.position.x, (int)projectileSpawner.position.y) == "Water")
            {
                v.AddToChamber("Water", 3, suckLeft);
            }
            v.SetCombinationChambers();
            return;
        }

        string[] collisionInfo = collision.tag.Split('-');
        int result = -1;

        if (collisionInfo[0] == "R") // The player is harvesting some resource
        {
            Resource r = collision.GetComponent<Resource>();
            Shaker s = collision.GetComponent<Shaker>();
            if (r.CanCollect() && (v.IsCurrentChamberEmpty(true) || v.GetCurrentChamber(true).GetElementNameByIndex(0) == collisionInfo[2]))
            {
                s.beingSucked = true; // shakes the resource
          
                result = v.AddToChamber(collisionInfo[2], int.Parse(collisionInfo[1]), suckLeft);
                r.DecrementResource();
            }
        }
        v.SetCombinationChambers();
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        Shaker s = other.GetComponent<Shaker>();

        if (!v.GetVacuumOn() && s != null)
        {
            s.beingSucked = false;
        }
  
    }


}
