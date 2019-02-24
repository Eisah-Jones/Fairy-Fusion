﻿using System.Collections;
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
    
    // UI Elements
    public GameObject canvas;
    public GameObject pause;
    public GameObject win;
    public GameObject minimap;
    public GameObject cdown;

    public Button resumeButtonPause;
    public Button menuButtonPause;
    public Button quitButtonPause;

    public Button menuButtonWin;
    public Button quitButtonWin;

    public Button currentButton;

    // Move this to another script
    public Image skullIcon;
   // public Countdown countdown;

    private LevelGenerator levelGen = new LevelGenerator();

    public GameObject player;
    public List<GameObject> playerList;

    public GameObject cam;
    public List<GameObject> cameras;

    public bool processCollision;

    public GameObject[] elemPrefabs;

    public ParticleSystem[] particles = new ParticleSystem[5];
    
	public GameObject endScreen;
	public Text winText;

	private List<PlayerInfo> pInfoList = new List<PlayerInfo>();

    public Tilemap ground;
    public Tilemap groundCollider;
    public Tilemap groundTrigger;

    public int numPlayers;
    public int numberOfLives = 3;
    private bool isPaused = false;
    private bool isOver = false;
    private bool checkingPauseInput = true;

    private bool isInitialized = false;

    // Move these to another script
    public Dictionary<string, int> killDict = new Dictionary<string, int>();
    public Text killfeed;



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

        // Create Model References
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);

        // Create UI References
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        // Set UI manager after getting canvas reference
        uiManager = canvas.GetComponent<UIManager>();

        pause = canvas.transform.GetChild(0).gameObject;
        win = canvas.transform.GetChild(1).gameObject;
        cdown = canvas.transform.GetChild(2).gameObject;
        killfeed = canvas.transform.GetChild(3).gameObject.GetComponent<Text>();
        minimap = canvas.transform.GetChild(4).gameObject;
        

        resumeButtonPause = pause.transform.GetChild(0).GetComponent<Button>();
        menuButtonPause = pause.transform.GetChild(1).GetComponent<Button>();
        quitButtonPause = pause.transform.GetChild(2).GetComponent<Button>();

        menuButtonWin = win.transform.GetChild(1).GetComponent<Button>();
        quitButtonWin = win.transform.GetChild(2).GetComponent<Button>();

        //countdown = canvas.GetComponent<Countdown>();
        //countdown.InitStart();

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

        //initialize kill dict
        InitKillDict();
        CreateScoreCounters();
        //  killfeed = GameObject.FindGameObjectWithTag("KillFeed").GetComponent<Text>();

        //countdown.startPreCountDown();

        isInitialized = true;
    }


    //Continually track player states, log information, etc.
    //Fixed update because of physics calculations
    private void FixedUpdate()
    {
        if (!isInitialized) return;

        if (!isPaused && !isOver)
            SendControllerInputsToPlayer(controllerManager.GetControllerInputs());
        cameraManager.UpdateCameraPosition(playerList);
    }


	//Checks for a winner each frame
	private void Update()
    {
        if (!isInitialized) return;

        int alive_count = 0;
		int winner = 0;

        isPaused = uiManager.GetPaused();
        if (cdown.activeSelf != !isPaused)
            cdown.SetActive(!isPaused);

		foreach (PlayerInfo info in pInfoList)
        {
			if (info.lives <= 0)
				alive_count += 1;
			else
				winner = info.playerNum;
		}

		if (alive_count == numberOfLives) {
            cdown.SetActive(false);
            minimap.SetActive(false);
            endScreen.SetActive (true);
			winText.text = string.Format ("Player {0} Wins!", winner);
            isOver = true;
		}
	}


    private void SendControllerInputsToPlayer(List<ControllerInputs> i)
    {
        int mapping = 0;
        foreach( GameObject p in playerList ){
            p.GetComponent<PlayerController>().UpdatePlayerMovement(i[mapping]);
            p.GetComponent<PlayerController>().UpdatePlayerInputs(i[mapping++]);
        }
    }


    public int GetNumPlayers()
    {
        return numPlayers;
    }

    // #### These functions below need to be moved to sensical script ####
    public void SpawnParticleEffectAtPosition(Vector3 pos, int particleIndex)
    {
        Instantiate(particles[particleIndex], pos, transform.rotation);
    }


    public string GetTriggerTile(int x, int y)
    {
        return levelGen.GetTerrainMap()[x, y];
    }

    private void InitKillDict()
    {
        foreach (var player in playerList)
        {
            killDict[player.GetComponent<PlayerInfo>().GetPlayerName()] = 0;

        }
    }


    public Dictionary<string, int> GetKillDict()
    {
        return killDict;
    }


    private void CreateScoreCounters()
    {

    }


    public void addKill(string killed, string killedBy)
    {
        Debug.Log("adding... " + killed + ";" + killedBy);
        killDict[killedBy] += 1;
        Debug.Log(killDict);
        updateKillFeed(killed, killedBy);
        canvas.GetComponent<KillTracker>().updateCanvasElements(killDict);
    }


    private void updateKillFeed(string killed, string killedBy)
    {
        Debug.Log(killedBy + " utterly destroyed " + killed);
        killfeed.text = killedBy + " utterly destroyed " + killed;
        StartCoroutine("WaitTilNextKill", 5);

    }


    IEnumerator WaitTilNextKill(int n)
    {
        yield return new WaitForSeconds(n);
        killfeed.text = "";
    }
}
