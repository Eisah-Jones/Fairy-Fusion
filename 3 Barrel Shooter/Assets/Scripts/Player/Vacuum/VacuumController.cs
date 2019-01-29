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

    //public void SetIsCombiningElements(bool b)
    //{
    //    v.SetIsCombiningElements(b);
    //}


    private void UpdateFairyPos(){

        int chamberIndex = v.GetCurrentChamberIndex();

        for (int i = 0; i < 2; i++)
        {
            int j = (i + chamberIndex) % 3;
            if (i == 0)
            {
                currentFairies[j].transform.position = Vector2.MoveTowards(currentFairies[j].transform.position, fairyPositions[3].transform.position, 10f * Time.deltaTime);
                currentFairies[(j + 1) % 3].transform.position = Vector2.MoveTowards(currentFairies[(j + 1) % 3].transform.position, fairyPositions[4].transform.position, 10f * Time.deltaTime);
            }
            else
            {
                currentFairies[(j + 1) % 3].transform.position = Vector2.MoveTowards(currentFairies[(j + 1) % 3].transform.position, fairyPositions[1].transform.position, 10f * Time.deltaTime);
            }
        }
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
                //Debug.Log("SP:" + scalePercent);
                //Debug.Log("GetAmountIndex: " + c.GetAmountByIndex(0));
                //Debug.Log("Cap: " + ed.chamberCapacity);
                maxFairyScale = 1 * scalePercent+.3f;
                float scale = GetScaleSize(scalePercent) + .3f;
                Debug.Log(scale); // .3f is the original fairy scale
                //Debug.Log(maxFairyScale);
                if (scale >=.65f)
                {
                    fairyTs[1].localScale = new Vector3(.65f, .65f, .65f);

                    //fairyTs[2].localScale += new Vector3(1 * scalePercent, 1 * scalePercent, 1 * scalePercent);
                    fairyTs[3].localScale = new Vector3(.65f, .65f, .65f);
                }
                else{
                    //fairyTs[0].localScale += new Vector3(1 * scalePercent, 1 * scalePercent, 1 * scalePercent);
                    fairyTs[1].localScale = new Vector3(scale,scale, scale);

                    //fairyTs[2].localScale += new Vector3(1 * scalePercent, 1 * scalePercent, 1 * scalePercent);
                    fairyTs[3].localScale = new Vector3(scale, scale,scale);
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
                    int ind = i + 3;
                    //if (i + 3 == 5) { ind = 1; }
                    //else { ind = i + 3; }
                    currentFairies[i].transform.position = fairyPositions[ind].transform.position;
                }
            }
            else
            {
                if (currentFairies[i].tag != fairies[c.GetContents()[0].GetElementID()].tag)
                {
                    Destroy(currentFairies[i]);
                    currentFairies[i] = Instantiate(fairies[c.GetContents()[0].GetElementID()]);

                    currentFairies[i].transform.position = fairyPositions[i+2].transform.position;
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
    public void HandleShootStateInput(bool shootingLeft, bool shootingRight, string playerName)
    {
        v.SetShootingBools(shootingLeft, shootingRight);

        if (!canShoot || v.GetVacuumOn() || (!shootingLeft && !shootingRight))
        {
            //Debug.Log("NOTHING: " + !canShoot + ", " + v.GetVacuumOn()+ ", " + (!shootingLeft && !shootingRight));
            return;
        }

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
        //else // We are picking up some projectile from the ground
        //{
        //    result = v.AddToChamber(collisionInfo[1], int.Parse(collisionInfo[0]));
        //    if (result == -1) { return; } // TODO: Randomly launch projectile from character
        //    Destroy(collision.gameObject);
        //}

        // Update chamber combinations
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
