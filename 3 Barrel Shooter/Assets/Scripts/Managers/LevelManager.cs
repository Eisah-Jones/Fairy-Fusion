using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


//This function manages all level information
// Stores references to scripts that can be accessed by the player
public class LevelManager: MonoBehaviour {

    // Managers
    public ElementManager elementManager;
    public ControllerManager controllerManager;
    public SpriteManager spriteManager;
    public ResourceManager resourceManager;
    public ParticleManager particleManager;
    public FluidManager fluidManager;
    public SoundManager soundManager;
    public CameraManager cameraManager;

    // Models
    public ChamberInteractionModel chamberInteractionModel;
    public PlayerCollisionModel playerCollisionModel;
    public ElementCollisionModel elementCollisionModel;

    public Countdown countdown;

    private LevelGenerator levelGen = new LevelGenerator();

    public GameObject player;
    public List<GameObject> playerList;

    public GameObject cam;
    public List<GameObject> cameras;

    public bool processCollision;

    public GameObject[] elemPrefabs;

    public ParticleSystem[] particles = new ParticleSystem[5];

    public Text testHUD;
	public GameObject endScreen;
	public Text winText;

	private List<PlayerInfo> pInfoList = new List<PlayerInfo>();

    public Tilemap ground;
    public Tilemap groundCollider;
    public Tilemap groundTrigger;

    private int numPlayers;

    // Use this for initialization
    void Start () {
        numPlayers = 2; // will eventually be given by player selection menu

        // Init/Create Manager References
        elementManager = new ElementManager();
        elementManager.initElementManager();

        controllerManager = new ControllerManager();
        controllerManager.InitControllerManager(this);

        spriteManager = new SpriteManager();

        resourceManager = new ResourceManager();

        fluidManager = new FluidManager();

        //particleManager = new ParticleManager();

        cameraManager = new CameraManager();
        cameraManager.InitCameraManager(numPlayers);

        soundManager = new SoundManager();
        soundManager.InitSoundManager();

        // Create Model References
        chamberInteractionModel = new ChamberInteractionModel(elementManager);

        playerCollisionModel = new PlayerCollisionModel(elementManager);

        elementCollisionModel = new ElementCollisionModel(elementManager);

        countdown = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Countdown>();
        countdown.InitStart();

        // Load element prefabs
        elemPrefabs = elementManager.LoadElementPrefabs();

        // Generate the level map
        levelGen.GenerateLevel(ground, groundCollider, groundTrigger, spriteManager, resourceManager);

        // Spawn and setup players and cameras
        playerList = levelGen.SpawnPlayers(player, GetComponent<LevelManager>(), numPlayers);
        foreach (GameObject p in playerList) {
			pInfoList.Add (p.GetComponent<PlayerInfo> ());
            cameraManager.AddCamera();
		}
        cameraManager.SetCameraRatio();
        cameraManager.UpdateCameraPosition(playerList);

        // Spawn Resources
        levelGen.SpawnResources(resourceManager);

        countdown.startPreCountDown();
	}


    //Continually track player states, log information, etc.
    //Fixed update because of physics calculations
    private void FixedUpdate()
    {
        // Retrieve and send controller inputs to the player
        SendControllerInputsToPlayer(controllerManager.GetControllerInputs());
        cameraManager.UpdateCameraPosition(playerList);
    }


	//Checks for a winner each frame
	private void Update()
    {
		int alive_count = 0;
		int winner = 0;

		foreach (PlayerInfo info in pInfoList)
        {
			if (info.lives <= 0)
				alive_count += 1;
			else
				winner = info.playerNum;
		}

		if (alive_count == 1) {
            GameObject.FindGameObjectWithTag("CountText").SetActive(false);
            GameObject.FindGameObjectWithTag("Minimap").SetActive(false);
            endScreen.SetActive (true);
			winText.text = string.Format ("Player {0} Wins!", winner);
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


    public int GetNumPlayers(){
        return numPlayers;
    }


    // These functions may need to be moved to sensical script
    public void SpawnParticleEffectAtPosition(Vector3 pos, int particleIndex)
    {
        Instantiate(particles[particleIndex], pos, transform.rotation);
    }


    public string GetTriggerTile(int x, int y)
    {
        return levelGen.GetTerrainMap()[x, y];
    }
}
