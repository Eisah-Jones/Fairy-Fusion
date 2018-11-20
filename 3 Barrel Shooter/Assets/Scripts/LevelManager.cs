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
    public ControllerManager controllerManager;

    private LevelGenerator levelGen = new LevelGenerator();

    public GameObject player;
    public List<GameObject> playerList;

    public bool processCollision;
    [SerializeField]
    public GameObject[] elemPrefabs = new GameObject[3];

    // Use this for initialization
    void Start () {
        elementManager.initElementManager();
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);
        controllerManager = GetComponent<ControllerManager>();

        int numPlayers = 2;
        playerList = levelGen.SpawnPlayers(player, GetComponent<LevelManager>(), numPlayers);
        levelGen.SpawnResources(GetComponent<LevelManager>(), elementManager, elemPrefabs);

        processCollision = true;
	}

    //Continually check to see if game is over, track player states, log information, etc.
    //Fixed update because of physics calculations
    private void FixedUpdate()
    {
        // Get each controller inputs
        List<ControllerInputs> controllerInputs = controllerManager.GetControllerInputs();
        // Send these inputs to the player
        SendControllerInputsToPlayer(controllerInputs);
    }

    private void SendControllerInputsToPlayer(List<ControllerInputs> i)
    {
        int mapping = 0;
        foreach( GameObject p in playerList){
            p.GetComponent<PlayerController>().UpdatePlayerMovement(i[mapping]);
            p.GetComponent<PlayerController>().UpdatePlayerInputs(i[mapping]);
            mapping++;
        }
    }

    public void SetProcessCollision(bool b){
        Debug.Log("Set Process Collision " + b.ToString());
        processCollision = b;
    }

    public int GetNumPlayers(){
        return playerList.Count;
    }
}
