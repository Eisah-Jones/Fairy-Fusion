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
    public UIManager uiManager;
    
    // Models
    public ChamberInteractionModel chamberInteractionModel;
    public PlayerCollisionModel playerCollisionModel;
    public ElementCollisionModel elementCollisionModel;

    private LevelGenerator levelGen = new LevelGenerator();

    public GameObject player; // Need to load dynamically
    public List<GameObject> playerList;

    public GameObject[] elemPrefabs; // Possibly move elsewhere?

    public ParticleSystem[] particles = new ParticleSystem[5]; // Move to particle manager

	private List<PlayerInfo> pInfoList = new List<PlayerInfo>();

    // Map tilemaps and hit boxes
    public Tilemap ground;
    public Tilemap groundCollider;
    public Tilemap groundTrigger;

    // Level information
    public int numPlayers;
    public int numberOfLives = 3;
    private int winner = -1;
    private bool isPaused = false;
    private bool isOver = false;
    private bool checkingPauseInput = true;
    private bool isInitialized = false;

    private AudioSource asource;
    // Set testing varible to start from Level and not MainMenu
    public void Start()
    {
        int n = 2;
        bool testing = true;
        if (testing)
            InitLevelManager(n);
    }


    // Use this for initialization
    public void InitLevelManager (int num) {
        numPlayers = num; // will eventually be given by player selection menu

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
        asource = gameObject.AddComponent<AudioSource>();
        // Create Model References
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);
        
        // Set UI manager after getting canvas reference
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();

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

        soundManager.PlaySoundByName(asource, "DrumsFury",false, .5f, 1f);
        //soundManager.StartBGMusic();
        // Spawn Resources
        levelGen.SpawnResources(resourceManager);

        isInitialized = true;
    }


    void FixedUpdate()
    {
        if (!isInitialized) return;

        if (!isPaused && !isOver)
            SendControllerInputsToPlayer(controllerManager.GetControllerInputs());
        cameraManager.UpdateCameraPosition(playerList);
    }


    public bool GetIsGameOver()
    {
		int alive_count = 0;
		winner = 0;

		foreach (PlayerInfo info in pInfoList)
		{
			if (info.lives <= 0)
				alive_count += 1;
			else
				winner = info.playerNum;
		}

		return alive_count == numberOfLives;
    }


    public List<GameObject> GetPlayerList()
    {
        return playerList;
    }


    public int GetNumPlayers()
    {
        return numPlayers;
    }

    public KillCounter GetKillCounter()
    {
        return uiManager.killCounter;
    }


    public int GetWinner()
    {
        return winner;
    }


    public void SetIsOver(bool b)
    {
        isOver = b;
    }


    private void SendControllerInputsToPlayer(List<ControllerInputs> i)
    {
        int mapping = 0;
        foreach( GameObject p in playerList ){
            p.GetComponent<PlayerController>().UpdatePlayerMovement(i[mapping]);
            p.GetComponent<PlayerController>().UpdatePlayerInputs(i[mapping++]);
        }
    }

    // #### These functions below need to be moved to sensical script ####
    public void SpawnParticleEffectAtPosition(Vector3 pos, int particleIndex)
    {
        Instantiate(particles[particleIndex], pos, transform.rotation);
    }

    public void StopBGMusic()
    {
        soundManager.StopSound(asource);
    }
    public string GetTriggerTile(int x, int y)
    {
        return levelGen.GetTerrainMap()[x, y];
    }
}
