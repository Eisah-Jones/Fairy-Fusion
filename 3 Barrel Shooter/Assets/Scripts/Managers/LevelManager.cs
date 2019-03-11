using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


//This function manages all level information
// Stores references to scripts that can be accessed by the player
public class LevelManager: MonoBehaviour
{

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

    private LevelGenerator levelGen;

    public GameObject player; // Need to load dynamically
    public List<GameObject> playerList;

	public Sprite[] uisprites;

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
    private AudioSource asource1;

    public bool testing = false;

    private Sprite[] waterSprites;


    // Set testing varible to start from Level and not MainMenu
    public void Start()
    {
        int n = 2;
        //bool testing = false;
        if (testing)
            InitLevelManager(n);
    }


    // Use this for initialization
    public void InitLevelManager (int num) {
        numPlayers = num;

        // Init/Create Manager References
        elementManager = new ElementManager();
        elementManager.initElementManager();

        controllerManager = new ControllerManager();
        controllerManager.InitControllerManager(this);

        spriteManager = new SpriteManager();
        resourceManager = new ResourceManager();
        fluidManager = new FluidManager();

        //particleManager = new ParticleManager();

        gameObject.AddComponent<CameraManager>();
        cameraManager = gameObject.GetComponent<CameraManager>();
        cameraManager.InitCameraManager(numPlayers);

        // Set UI manager after getting canvas reference
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        uiManager.InitUIManager();

        soundManager = new SoundManager();
        soundManager.InitSoundManager();
        asource = gameObject.AddComponent<AudioSource>();
        asource1 = gameObject.AddComponent<AudioSource>();

        // Create Model References
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);

        // Load element prefabs
        elemPrefabs = elementManager.LoadElementPrefabs();

        // Generate the level map
        gameObject.AddComponent<LevelGenerator>();
        levelGen = gameObject.GetComponent<LevelGenerator>();
        levelGen.GenerateLevel(ground, groundCollider, groundTrigger, spriteManager, resourceManager);

        // Spawn and setup players and cameras
        playerList = levelGen.SpawnPlayers(player, GetComponent<LevelManager>(), numPlayers);
		SetUISprites ();
        foreach (GameObject p in playerList)
        {
			PlayerInfo pInfo = p.GetComponent<PlayerInfo> ();
			pInfoList.Add (pInfo);
            cameraManager.AddCamera();
			FairyController controller = p.GetComponentInChildren<FairyController> ();
			controller.LT = GameObject.Find("PlayerUI" + pInfo.playerNum.ToString()).transform.Find("LTI").GetComponent<Image>();
			controller.RT = GameObject.Find("PlayerUI" + pInfo.playerNum.ToString()).transform.Find("RTI").GetComponent<Image>();
			controller.combo = GameObject.Find("PlayerUI" + pInfo.playerNum.ToString()).transform.Find("Combo").GetComponent<Image>();
			controller.uisprites = uisprites;
		}
        cameraManager.SetCameraRatio();
        cameraManager.UpdateCameraPosition(playerList);

        soundManager.PlaySoundByName(asource, "DrumsFury", false, .5f, 1f);
        
        levelGen.SpawnResources(resourceManager);

        AnimateWater(levelGen.GetWaterTiles(groundTrigger));

        isInitialized = true;
    }


    void FixedUpdate()
    {
        if (!isInitialized) return;

        if (!isPaused && !isOver)
            SendControllerInputsToPlayer(controllerManager.GetControllerInputs());
        cameraManager.UpdateCameraPosition(playerList);
    }


    public void AnimateWater(List<int[]> tiles)
    {
        waterSprites = spriteManager.GetWaterSprites();
        StartCoroutine(AnimateWaterSprite(tiles));
    }


    private IEnumerator AnimateWaterSprite(List<int[]> tiles)
    {
        int i = 0;
        bool top = false;
        Tile t = (Tile)ScriptableObject.CreateInstance("Tile");
        while (true)
        {
            yield return new WaitForSeconds(0.075f);

            foreach (int[] coord in tiles)
            {
                t.sprite = waterSprites[i];
                Vector3Int pos = new Vector3Int(coord[0], coord[1], 0);
                groundTrigger.SetTile(pos, t);
                groundTrigger.RefreshTile(pos);
            }

            if (!top) i++;
            else i--;
            if (i == 9) { top = true;  yield return new WaitForSeconds(0.033f); }
            else if (i == 4) { top = false; yield return new WaitForSeconds(0.033f); }
        }
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

    public void PlayEndRoundSound()
    {
        soundManager.PlaySoundOneShotName(asource1, "AirHorn", false, .5f, 1.0f);

    }
    private void SendControllerInputsToPlayer(List<ControllerInputs> i)
    {
        int mapping = 0;
        foreach( GameObject p in playerList ){
            p.GetComponent<PlayerController>().UpdatePlayerInputs(i[mapping]);
            p.GetComponent<PlayerController>().UpdatePlayerMovement(i[mapping++]);
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


	private void SetUISprites(){
		Sprite[] bullets = Resources.LoadAll<Sprite>("Sprites/Bullets");
		uisprites [0] = bullets [11];
		uisprites [1] = bullets[5];
		uisprites [2] = bullets [3];
		uisprites [3] = bullets[0];
		uisprites [4] = Resources.Load<Sprite>("Sprites/Leaf");
		uisprites [5] = bullets [2];
		uisprites [6] = bullets [8];
		uisprites [7] = bullets [4];
		uisprites [8] = bullets [7];
		uisprites [9] = bullets [10];
		uisprites [10] = bullets [1];
	}

}
