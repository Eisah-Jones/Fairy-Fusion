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
    public SoundManager soundManager;

    private LevelGenerator levelGen = new LevelGenerator();

    public GameObject player;
    public List<GameObject> playerList;

    public GameObject camera;
    public List<GameObject> cameras;

    public bool processCollision;

    public GameObject[] elemPrefabs = new GameObject[3];

    public ParticleSystem[] particles = new ParticleSystem[3];

    public Text testHUD;
	public GameObject endScreen;
	public Text winText;

	private List<PlayerInfo> pInfoList = new List<PlayerInfo>();

    public Tilemap ground;
    public Tilemap groundCollider;
    public GameObject wall;

    // Use this for initialization
    void Start () {
        elementManager.initElementManager();
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);
        controllerManager = GetComponent<ControllerManager>();
        spriteManager = new SpriteManager();
        resourceManager = new ResourceManager();
        particleManager = new ParticleManager();
        soundManager = new SoundManager();

        levelGen.GenerateLevel(ground, groundCollider, spriteManager, resourceManager, wall);

        int numPlayers = 2;
        playerList = levelGen.SpawnPlayers(player, GetComponent<LevelManager>(), numPlayers);
		foreach (GameObject p in playerList) {
			pInfoList.Add (p.GetComponent<PlayerInfo> ());
            GameObject cameraObject = Instantiate(camera, Vector3.zero, Quaternion.identity);
            cameras.Add(cameraObject);
		}
        InitializeCameras();
        UpdateCameras();

        // Sound testing
        //soundManager.PlaySoundsByID(playerList[0].GetComponent<AudioSource>(), 0);

        levelGen.SpawnResources(resourceManager);

        processCollision = true;
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
        UpdateGUI();
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
			Time.timeScale = 0f;
		}
			
	}

    //Temp GUI
    private void UpdateGUI(){
        PlayerInfo pi = playerList[0].GetComponent<PlayerInfo>();
        testHUD.text = "Health: " + pi.GetPlayerHealth() + "\n";
        testHUD.text += "Chamber 0: " +  pi.GetVacuum().GetChamberByIndex(0).GetElementNameByIndex(0) + " " + pi.GetVacuum().GetChamberByIndex(0).GetAmountByIndex(0) + "\n";
        testHUD.text += "  COMBO 0: " + pi.GetVacuum().GetCombinationByIndex(0).name + "\n";
        testHUD.text += "Chamber 1: " + pi.GetVacuum().GetChamberByIndex(1).GetElementNameByIndex(0) + " " + pi.GetVacuum().GetChamberByIndex(1).GetAmountByIndex(0) + "\n";
        testHUD.text += "  COMBO 1: " + pi.GetVacuum().GetCombinationByIndex(1).name + "\n";
        testHUD.text += "Chamber 2: " + pi.GetVacuum().GetChamberByIndex(2).GetElementNameByIndex(0) + " " + pi.GetVacuum().GetChamberByIndex(2).GetAmountByIndex(0) + "\n";
        testHUD.text += "  COMBO 2: " + pi.GetVacuum().GetCombinationByIndex(2).name + "\n";
        string v;
        if (pi.GetVacuum().GetIsCombiningElements())
            v = "COMBO ";
        else
            v = "Chamber ";
        testHUD.text += "**CURRENT: " + v + pi.GetVacuum().GetCurrentChamberIndex();
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
