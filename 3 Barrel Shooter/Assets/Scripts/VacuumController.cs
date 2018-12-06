using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour {

    LevelManager levelManager;
    Vacuum v;
    BoxCollider2D vacuumArea;
    [SerializeField]
    public ParticleSystem flamethrower;
    [SerializeField]
    public ParticleSystem watercanon;
    [SerializeField]
    public ParticleSystem steamblast;
    public GameObject throwTransform;
    bool canShoot;
    int shootNum = 10;
    int tempShoot = 0;
    bool FlameOn = false;
    public ParticleSystem thrower;
    //Sets the vacuum for the player, should be called after player gameObject instantiation 
    public void SetVacuum(Vacuum vac, LevelManager lm){
        v = vac;
        vacuumArea = GetComponent<BoxCollider2D>();
        levelManager = lm;
        canShoot = true;
    }

    // Use this for initialization
    void Start () {
// flamethrower.Stop();
    }
    
    // Update is called once per frame
    void Update () {
        if (FlameOn)
        {
            // GetComponentInParent<Transform>().rotation = throwTransform.transform.localRotation;
            //thrower.transform.position = throwTransform.transform.position;
            //thrower.transform.rotation = throwTransform.transform.rotation;
            //thrower.transform.LookAt(throwTransform.transform.forward);
        }
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

        if (!isShooting)
        {

            FlameOn = false;
            //thrower.Stop();
        }
        canShoot = false;

        if (isShooting && !v.GetVacuumOn()){
            Vacuum.Chamber.InventoryInfo result = v.Shoot();
            //If element null, we could not shoot else can shoot
            if (result != null)
            {
                //if (result.GetElementID() == 1)
                //{
                //    if (!FlameOn)
                //    {
                //        FlameOn = true;
                //        thrower = Instantiate(flamethrower, throwTransform.transform.position, throwTransform.transform.rotation);
                //        thrower.Play();
                       
                                           
                //    }

                //}
                //else if (result.GetElementID() == 3)
                //{
                //    if (!FlameOn)
                //    {
                //        FlameOn = true;
                //        thrower = Instantiate(watercanon, throwTransform.transform.position, throwTransform.transform.rotation);
                //        thrower.Play();
                       

                //    }

                //}
                //else if (result.GetElementID() == 6)
                //{
                //    if (!FlameOn)
                //    {
                //        FlameOn = true;
                //        thrower = Instantiate(steamblast, throwTransform.transform.position, throwTransform.transform.rotation);
                //        thrower.Play();
                     

                //    }

                //}
                //else
                //{
                    //Instantiate element!!
                    //if(thrower != null)
                        //thrower.Stop();
                    //FlameOn = false;
                    ProjectileSpawner p = GetComponent<ProjectileSpawner>();
                    p.GetComponent<ProjectileSpawner>().shootProjectile(result.GetElementID(), levelManager);
                    v.SetCombinationChambers();
                //}
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.tag);

        if (collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged") return;

        string cName = collision.tag.Split('-')[1];
        int id = int.Parse(collision.tag.Split('-')[0]);

        if (v == null){
            return; //tempfix
        }
        int addedResult = v.AddToChamber(cName, id);
        //If the item was added to the chamber destroy, else spit it back out randomly

        if (addedResult != -1)
        {
            Destroy(collision.gameObject);
            v.SetCombinationChambers();
        }

        v.DebugVac();
    }
}
