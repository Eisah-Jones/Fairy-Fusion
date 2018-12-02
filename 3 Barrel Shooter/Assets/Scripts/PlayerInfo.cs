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
	private string rearea = "";

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
		Vector3 respawn = new Vector3(0,0,0);
        //Can specify respawn location before Coroutine is started and save as a temporary class variable
		switch (rearea) {
		case "1":
			respawn = GetVector (4);
			break;
		case "2":
			respawn = GetVector(3);
			break;
		case "3":
			respawn = GetVector(2);
			break;
		case "4":
			respawn = GetVector(1);
			break;
		}
		transform.position = respawn;
        startedRespawn = !startedRespawn;
        health = 100.0f;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
		if (collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged") {
			rearea = collision.name;
			return;
		}
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

	private Vector3 GetRandomVector(int x_range, int y_range){
		if (x_range >= 0f && y_range >= 0f)
			return new Vector3(Random.Range(0f,x_range),Random.Range(0f,y_range),-2f);
		else if(x_range < 0f && y_range >= 0f)
			return new Vector3(Random.Range(x_range,0f),Random.Range(0f,y_range),-2f);
		else if(x_range >= 0f && y_range < 0f)
			return new Vector3(Random.Range(0,x_range),Random.Range(y_range,0f),-2f);
		else if(x_range < 0f && y_range < 0f)
			return new Vector3(Random.Range(x_range,0f),Random.Range(y_range,0f),-2f);
		return new Vector3 (0, 0, 0);
	}

	private Vector3 GetVector(int area){
		Vector3 re = new Vector3(0,0,0);
		switch(area){
		case 1:
			re = GetRandomVector(-9,4);
			break;
		case 2:
			re = GetRandomVector(9,4);
			break;
		case 3:
			re = GetRandomVector(-9,-4);
			break;
		case 4:
			re = GetRandomVector(9,-4);
			break;
		}
		return re;
	}
}
