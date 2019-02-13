using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


//This function manages all level information
// Stores references to scripts that can be accessed by the player
public class LevelManager: MonoBehaviour {

    public ElementManager elementManager = new ElementManager();
    public ChamberInteractionModel chamberInteractionModel;
    public PlayerCollisionModel playerCollisionModel;
    public ElementCollisionModel elementCollisionModel;
    public ControllerManager controllerManager;
    public SpriteManager spriteManager;
    public ResourceManager resourceManager;
    public ParticleManager particleManager;
    public FluidManager fluidManager;
    public SoundManager soundManager;
    public Countdown countdown;
    private LevelGenerator levelGen = new LevelGenerator();

    public GameObject player;
    public List<GameObject> playerList;

    public GameObject cam;
    public List<GameObject> cameras;

    public bool processCollision;

    public GameObject[] elemPrefabs = new GameObject[3];

    public ParticleSystem[] particles = new ParticleSystem[5];

    public Text testHUD;
	public GameObject endScreen;
	public Text winText;

	private List<PlayerInfo> pInfoList = new List<PlayerInfo>();

    public Tilemap ground;
    public Tilemap groundCollider;
    public Tilemap groundTrigger;
    public GameObject wall;

    private int numPlayers;

    // Use this for initialization
    void Start () {
        numPlayers = 2;
        elementManager.initElementManager();
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);
        controllerManager = GetComponent<ControllerManager>();
        spriteManager = new SpriteManager();
        resourceManager = new ResourceManager();
        fluidManager = new FluidManager();
        //particleManager = new ParticleManager();
        soundManager = new SoundManager();
        soundManager.InitSoundManager();
        countdown = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Countdown>();
        countdown.InitStart();
        levelGen.GenerateLevel(ground, groundCollider, groundTrigger, spriteManager, resourceManager);

        playerList = levelGen.SpawnPlayers(player, GetComponent<LevelManager>(), numPlayers);
        foreach (GameObject p in playerList) {
			pInfoList.Add (p.GetComponent<PlayerInfo> ());
            GameObject cameraObject = Instantiate(cam, Vector3.zero, Quaternion.identity);
            cameras.Add(cameraObject);
		}
        InitializeCameras();
        UpdateCameras();

        levelGen.SpawnResources(resourceManager);
        countdown.startPreCountDown();
        processCollision = true;
	}


    public void SpawnParticleEffectAtPosition(Vector3 pos, int particleIndex)
    {
        Instantiate(particles[particleIndex], pos, transform.rotation);
    }


    public string GetTriggerTile(int x, int y)
    {
        return levelGen.GetTerrainMap()[x, y];
    }


    private void InitializeCameras()
    {
        if (cameras.Count == 2)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
        }
        else if (cameras.Count == 3)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0.25f, 0.0f, 0.5f, 0.5f);
        }
        else if (cameras.Count == 4)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            cameras[3].GetComponent<Camera>().rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
    }


    private void UpdateCameras()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            GameObject c = cameras[i];
            Vector3 playerPos = playerList[i].transform.position;
            c.transform.position = new Vector3(playerPos.x, playerPos.y, -12f);
        }
    }


    //Continually check to see if game is over, track player states, log information, etc.
    //Fixed update because of physics calculations
    private void FixedUpdate()
    {
        // Retrieve and send controller inputs to the player
        SendControllerInputsToPlayer(controllerManager.GetControllerInputs());
        UpdateCameras();
    }


	//Checks for a winner each frame
	private void Update(){
		int alive_count = 0;
		int winner = 0;
		foreach (PlayerInfo info in pInfoList) {
			if (info.lives <= 0)
				alive_count += 1;
			else
				winner = info.playerNum;
		}
		if (alive_count == 1) {
			endScreen.SetActive (true);
			winText.text = string.Format ("Player {0} Wins!", winner);
			//Time.timeScale = 0f;
		}
			
	}


    private void SendControllerInputsToPlayer(List<ControllerInputs> i)
    {
        int mapping = 0;
        foreach( GameObject p in playerList ){
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
