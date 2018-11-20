using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Generates the level
// Procedurally generates terrain
// Spawns in resources and players
public class LevelGenerator : MonoBehaviour {

    private int levelWidth  = 20;
    private int levelHeight = 15;

    Tilemap ground;


    //Array for storing level floor map
    private string[,] terrainMap;

    public void GenerateLevel(){

        ground = transform.GetComponent<Tilemap>();
        Debug.Log(ground);

        int startingPosX = -levelWidth / 2;
        int startingPosY = -levelHeight / 2;
        for (int i = 0; i < levelWidth; i++){
            for (int j = 0; j < levelHeight; j++){
                Vector3Int pos = new Vector3Int(i, j, 1);
                ground.SetTileFlags(pos, TileFlags.None);
                ground.SetColor(pos, Color.green);
            }
        }
        return;
    }

    public List<GameObject> SpawnPlayers(GameObject p, LevelManager lm, int numPlayers){

        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < numPlayers; i++){
            GameObject player = Instantiate(p); // Spawn player object
            player.AddComponent<PlayerInfo>(); // Add PlayerInfo script
            player.GetComponent<PlayerInfo>().InitPlayerInfo(lm, i+1); // Initialize player info script
            GameObject playerGun = player.transform.Find("Gun").gameObject; // Get a reference to the player's gun object
            playerGun.AddComponent<VacuumController>(); // Add vacuum controller
            playerGun.GetComponent<VacuumController>().SetVacuum(player.GetComponent<PlayerInfo>().GetVacuum(), lm);
            playerGun.AddComponent<ProjectileSpawner>(); // Add a projectile spawner script to the player gun
            PlayerController pc = player.gameObject.GetComponent<PlayerController>();
            pc.InitPlayerController(playerGun.GetComponent<VacuumController>());
            result.Add(player); //Add player to our player list
        }

        return result;
    }

    public void SpawnResources(LevelManager lm, ElementManager em, GameObject[] p){
        List<Vector2> l = new List<Vector2>();

        int lim = -1;
        int x = 0;

        int id;

        for (int i = 4; i > lim; i--){
            id = int.Parse(p[i].tag.Split('-')[0]);
            GameObject o = Instantiate(p[i], new Vector2(x, 0), Quaternion.identity);
            o.GetComponent<ElementObject>().initElement(lm, em.GetElementDataByID(id) , false);
            x += 3;
        }
    }


	// Use this for initialization
	void Start () {
        //GenerateLevel();
        //SpawnPlayers();
	}
}
