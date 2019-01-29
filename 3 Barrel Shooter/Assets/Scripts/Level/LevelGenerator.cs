using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Generates the level
// Procedurally generates terrain
// Spawns in resources and players
public class LevelGenerator : MonoBehaviour {

    private int levelWidth  = 45;
    private int levelHeight = 45;
    private Vector3 center;
    private List<Vector3> spawnPos;
    private string[,] terrainMap; //Array for storing level floor map
    private SpriteManager spriteManager;

    public void GenerateLevel(Tilemap tm, Tilemap cm, Tilemap trigm, SpriteManager sm, ResourceManager rm){

        center = new Vector3(levelWidth / 2, levelHeight / 2, 0);

        spawnPos = new List<Vector3>{new Vector3(-levelWidth/4, levelHeight/4, 0),
                                     new Vector3(levelWidth/4, levelHeight/4, 0),
                                     new Vector3(-levelWidth/4, -levelHeight/4, 0),
                                     new Vector3(levelWidth/4,-levelHeight/4, 0)};

        float scale = 5.0f;
        float xOffset = UnityEngine.Random.Range(0f, 99999f);
        float yOffset = UnityEngine.Random.Range(0f, 99999f);

        terrainMap = new string[levelWidth, levelHeight];
        for (int x = 0; x < terrainMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < terrainMap.GetUpperBound(1); y++)
            {

                float xCoord = (float) x / levelWidth * scale + xOffset;
                float yCoord = (float) y / levelHeight * scale + yOffset;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                if (sample < 0.2)
                    terrainMap[x, y] = "Water";
                else if (sample < 0.5)
                    terrainMap[x, y] = "Grass";
                else if (sample < 0.9)
                    terrainMap[x, y] = "Dirt";
                else
                    terrainMap[x, y] = "Mountain";
            }
        }

        spriteManager = sm;

        RenderMap(tm, cm, trigm);
    }

    public string[,] GetTerrainMap()
    {
        return terrainMap;
    }


    public void RenderMap(Tilemap tilemap, Tilemap colliderMap, Tilemap triggerMap)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < terrainMap.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < terrainMap.GetUpperBound(1); y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                Tile t = new Tile();
                SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, x, y);
                t = ti.tile;
                t = RotateTile(t, ti.rotation);
                if (terrainMap[x, y] == "Mountain") { colliderMap.SetTile(pos, t); }
                else if (terrainMap[x, y] == "Water") { triggerMap.SetTile(pos, t); }
                else { tilemap.SetTile(pos, t); }
            }
        }

        // Borders around the level
        for (int i = 0; i < levelWidth; i++)
        {
            Vector3Int pos = new Vector3Int(i, -1, 0);
            Tile t = new Tile();
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }

        for (int i = 0; i < levelHeight; i++)
        {
            Vector3Int pos = new Vector3Int(levelWidth-1, i, 0);
            Tile t = new Tile();
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }

        for (int i = 0; i < levelWidth; i++)
        {
            Vector3Int pos = new Vector3Int(i, levelHeight-1, 0);
            Tile t = new Tile();
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }

        for (int i = 0; i < levelHeight; i++)
        {
            Vector3Int pos = new Vector3Int(0, i, 0);
            Tile t = new Tile();
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }
    }


    private Tile RotateTile(Tile t, float rotation){
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotation), Vector3.one);
        t.transform = matrix;
        return t;
    }

    public List<GameObject> SpawnPlayers(GameObject p, LevelManager lm, int numPlayers){

        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < numPlayers; i++){
            Vector3 pos = GetSpawnPos(center + spawnPos[i]);
            GameObject player = Instantiate(p, pos, Quaternion.identity); // Spawn player object
            player.AddComponent<PlayerInfo>(); // Add PlayerInfo script
            player.GetComponent<PlayerInfo>().InitPlayerInfo(lm, i+1); // Initialize player info script
            GameObject playerGun = player.transform.Find("Fairies").gameObject; // Get a reference to the player's gun object
            playerGun.AddComponent<VacuumController>(); // Add vacuum controller
            playerGun.GetComponent<VacuumController>().SetVacuum(player.GetComponent<PlayerInfo>().GetVacuum(), lm);
            playerGun.AddComponent<ProjectileSpawner>(); // Add a projectile spawner script to the player gun
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.InitPlayerController(playerGun.GetComponent<VacuumController>());
            result.Add(player); //Add player to our player list
			player.name = string.Format("Player{0}",i+1); // needs to be +1 since you add 1
        }
        return result;
    }


    public void SpawnResources(ResourceManager rm){
        SpawnTrees(rm);
        SpawnRocks(rm);
        SpawnFire(rm);
    }


    private void SpawnFire(ResourceManager rm)
    {
        for (int i = 0; i < 35; i++)
        {
            int[] spawnPoint = { UnityEngine.Random.Range(0, terrainMap.GetUpperBound(0)), 
                                 UnityEngine.Random.Range(0, terrainMap.GetUpperBound(1))};
            GameObject f = Instantiate(rm.GetResourceGameObject("Fire"), new Vector3(spawnPoint[0], spawnPoint[1], 0.3f), Quaternion.identity);
            f.GetComponent<Resource>().InitResource(5);
        }
    }


    private void SpawnRocks(ResourceManager rm)
    {
        List<int[]> rockSpawnPoints = GetRockSpawnPoints();

        for (int j = 0; j < 25; j++)
        {
            int i = UnityEngine.Random.Range(0, rockSpawnPoints.Count);
            int[] spawnPoint = rockSpawnPoints[i];
            GameObject r = Instantiate(rm.GetResourceGameObject("Rock"), new Vector3(spawnPoint[0] - 0.05f, spawnPoint[1] + 0.25f, 0f), Quaternion.identity);
            r.GetComponent<Resource>().InitResource(2);
        }
    }


        private void SpawnTrees(ResourceManager rm)
    {
        List<int[]> treeSpawnPoints = GetTreeSpawnPoints();

        for (int j = 0; j < 5; j++)
        {
            int i = UnityEngine.Random.Range(0, treeSpawnPoints.Count);
            int[] spawnPoint = treeSpawnPoints[i];
            Instantiate(rm.GetResourceGameObject("Tree"), new Vector3(spawnPoint[0], spawnPoint[1], 0f), Quaternion.identity);
        }
    }


    private List<int[]> GetRockSpawnPoints()
    {
        List<int[]> result = new List<int[]>();
        for (int x = 0; x < terrainMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < terrainMap.GetUpperBound(1); y++)
            {
                if (terrainMap[x, y] == "Dirt") { int[] temp = { x, y }; result.Add(temp); }
            }
        }
        return result;
    }


    private List<int[]> GetTreeSpawnPoints()
    {
        List<int[]> result = new List<int[]>();
        for (int x = 0; x < terrainMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < terrainMap.GetUpperBound(1); y++)
            {
                try
                {
                    string[] positions = { terrainMap[x    , y    ],
                                           terrainMap[x + 1, y    ],
                                           terrainMap[x + 2, y    ],
                                           terrainMap[x + 3, y    ],
                                           terrainMap[x    , y + 1],
                                           terrainMap[x + 1, y + 1],
                                           terrainMap[x + 2, y + 1],
                                           terrainMap[x + 3, y + 1]};

                    if (IsAllSame(positions, "Grass")) { int[] temp = { x, y }; result.Add(temp); }
                }
                catch (Exception e) {} ///Means one of our positions was invalid, and can't spawn
            }
        }
        return result;
    }


    private bool IsAllSame(string[] s, string type)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != type) { return false; }
        }

        return true;
    }


    public Vector3 GetSpawnPos(Vector3 startPos){
        //If spawn pos is invalid, search for nearest valid position
        int x = (int)startPos.x;
        int y = (int)startPos.y;

        if (!IsValidPosition(x, y))
        {
            int radius = 1;
            int tempX  = 0;
            int tempY  = 0;

            do
            {
                for (int t = 0; t < 4; t++)
                {
                    for (int i = 0; i < radius * 2 + 1; i++)
                    {
                        if (t == 0) { tempX = x - i;      tempY = y - radius; } //check top row
                        if (t == 1) { tempX = x + radius; tempY = y - i;      } //check right col
                        if (t == 2) { tempX = x - radius; tempY = y - i;      } //check left col
                        if (t == 3) { tempX = x - i;      tempY = y + radius; } //check bottom row

                        if (IsValidPosition(tempX, tempY)) { return new Vector3(tempX, tempY, 0); }
                    }
                }
                radius += 1;
            } 
            while (radius < levelWidth/2f);
        }
        return startPos;
    }


    private bool IsValidPosition(int x, int y){
        return IsInBounds(x    , y)     && terrainMap[x    , y]     != "Mountain" &&
               IsInBounds(x    , y - 1) && terrainMap[x    , y - 1] != "Mountain" &&
               IsInBounds(x - 1, y)     && terrainMap[x - 1, y]     != "Mountain" &&
               IsInBounds(x - 1, y - 1) && terrainMap[x - 1, y - 1] != "Mountain";
    }


    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < levelWidth && y >= 0 && y < levelHeight;
    }


    private string GetBlockUnderPlayer(Vector3 pos){
        return terrainMap[(int)pos.x, (int)pos.y];
    }


	// Use this for initialization
	void Start () {
        //GenerateLevel();
        //SpawnPlayers();
	}
}
