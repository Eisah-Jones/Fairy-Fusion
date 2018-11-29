using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    //Level manager should be passed to player when instantiated
    LevelManager levelManager;
    Vacuum vacuum;
    BoxCollider2D vacuumArea;

    public float health;
    public int playerNum;

    private bool startedRespawn;

    public void InitPlayerInfo(LevelManager lm, int pNum){
        levelManager = lm;
        vacuum = new Vacuum(levelManager);

        health = 100.0f;
        playerNum = pNum;

        startedRespawn = false;

    }


    public Vacuum GetVacuum(){
        return vacuum;
    }


    public float GetPlayerHealth(){
        return health;
    }


    public bool isDead(){
        return health <= 0.0f;
    }


    private void Update()
    {
        //Check to see if player is dead
        if(isDead()){
            //Destroy the player object and notify player of death!!
            //LevelManager will handle the game conditions, do not have to worry about that

            //Check levelManager if player can respawn?

            // Can either destroy the player object and have levelManager reinstantiate,
            //Destroy(transform.gameObject);
            // or (preferably) disable controls, make player invisible, and teleport them, then reestablish control
            //transform.gameObject.SetActive(false);
            //Start respawn coroutine
            if (!startedRespawn)
            {
                startedRespawn = !startedRespawn;
                StartCoroutine("respawn");
            }
        }
    }

    private IEnumerator respawn(){
        yield return new WaitForSeconds(1.0f);
        //Can specify respawn location before Coroutine is started and save as a temporary class variable
        transform.position = new Vector3(-6, 1, -2);
        startedRespawn = !startedRespawn;
        health = 100.0f;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Get the element script from the collision gameobject for reference
        ElementObject element = collision.gameObject.GetComponent<ElementObject>();
        bool isProjectile = element.GetIsProjectile();

        //If it is not a projectile, then there is no interaction (except maybe physics, so we don't care)
        if (!isProjectile){
            return;
        }

        int elemID = element.GetID();
        string elemName = element.GetName();
        PlayerCollisionModel.CollisionResult result = levelManager.playerCollisionModel.HandleCollision(health, elemName);
        health = result.health;
        //Will apply player effects later
    }
}
