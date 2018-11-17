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

    public void SpawnPlayer(GameObject p, LevelManager lm){
        GameObject o = Instantiate(p);
        o.AddComponent<PlayerInfo>();
        o.GetComponent<PlayerInfo>().initPlayerInfo(lm);
        GameObject g = o.transform.Find("Gun").gameObject;
        g.AddComponent<VacuumController>();
        g.GetComponent<VacuumController>().SetVacuum(o.GetComponent<PlayerInfo>().GetVacuum(), lm);
        g.AddComponent<ProjectileSpawner>();
    }

    public void SpawnResources(LevelManager lm, ElementManager em, GameObject[] p){
        List<Vector2> l = new List<Vector2>();
        //l.Add()
        int lim = 3;
        int x = 0;

        int id;

        for (int i = 0; i < lim; i++){
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
