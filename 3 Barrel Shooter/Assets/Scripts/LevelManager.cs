using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This function manages all level information
// Stores references to scripts that can be accessed by the player
public class LevelManager: MonoBehaviour {

    public ElementManager elementManager = new ElementManager();
    public ChamberInteractionModel chamberInteractionModel;
    public PlayerCollisionModel playerCollisionModel;
    public ElementCollisionModel elementCollisionModel;

    private LevelGenerator levelGen = new LevelGenerator();

    public GameObject player;
    public bool processCollision;
    [SerializeField]
    public GameObject[] elemPrefabs = new GameObject[3];

    // Use this for initialization
    void Start () {
        elementManager.initElementManager();
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);

        levelGen.SpawnPlayer(player, GetComponent<LevelManager>());
        levelGen.SpawnResources(GetComponent<LevelManager>(), elementManager, elemPrefabs);

        processCollision = true;
	}

    //Continually check to see if game is over, track player states, log information, etc.
    private void Update()
    {

    }

    public void SetProcessCollision(bool b){
        Debug.Log("Set Process Collision " + b.ToString());
        processCollision = b;
    }
}
