using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public GameObject[] elemPrefabs = new GameObject[3];

    public ParticleSystem[] particles = new ParticleSystem[3];

    public List<Text> testHUD;
	public GameObject endScreen;
	public Text winText;

	private List<PlayerInfo> pInfoList = new List<PlayerInfo>();
    // Use this for initialization
    void Start () {
        elementManager.initElementManager();
        chamberInteractionModel = new ChamberInteractionModel(elementManager);
        playerCollisionModel = new PlayerCollisionModel(elementManager);
        elementCollisionModel = new ElementCollisionModel(elementManager);
        controllerManager = GetComponent<ControllerManager>();

        int numPlayers = 2;
        playerList = levelGen.SpawnPlayers(player, GetComponent<LevelManager>(), numPlayers);
		foreach (GameObject player in playerList) {
			pInfoList.Add (player.GetComponent<PlayerInfo> ());
		}
        //levelGen.SpawnResources(GetComponent<LevelManager>(), elementManager, elemPrefabs);

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
		for (int i = 0; i<2; i++)
        	UpdateGUI(i);
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


	private void UpdateGUI(int index){
        PlayerInfo pi = playerList[index].GetComponent<PlayerInfo>();
        testHUD[index].text = "Health: " + pi.GetPlayerHealth() + "\n";
		testHUD[index].text += "Chamber 0: " +  pi.GetVacuum().GetChamberByIndex(0).GetElementNameByIndex(0) + " " + pi.GetVacuum().GetChamberByIndex(0).GetAmountByIndex(0) + "\n";
		testHUD[index].text += "  COMBO 0: " + pi.GetVacuum().GetCombinationByIndex(0).name + "\n";
		testHUD[index].text += "Chamber 1: " + pi.GetVacuum().GetChamberByIndex(1).GetElementNameByIndex(0) + " " + pi.GetVacuum().GetChamberByIndex(1).GetAmountByIndex(0) + "\n";
		testHUD[index].text += "  COMBO 1: " + pi.GetVacuum().GetCombinationByIndex(1).name + "\n";
		testHUD[index].text += "Chamber 2: " + pi.GetVacuum().GetChamberByIndex(2).GetElementNameByIndex(0) + " " + pi.GetVacuum().GetChamberByIndex(2).GetAmountByIndex(0) + "\n";
		testHUD[index].text += "  COMBO 2: " + pi.GetVacuum().GetCombinationByIndex(2).name + "\n";
        string v;
        if (pi.GetVacuum().GetIsCombiningElements())
            v = "COMBO ";
        else
            v = "Chamber ";
		testHUD[index].text += "**CURRENT: " + v + pi.GetVacuum().GetCurrentChamberIndex();
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
