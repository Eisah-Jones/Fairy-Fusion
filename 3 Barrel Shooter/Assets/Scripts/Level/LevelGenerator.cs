using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


// Generates the level
// Procedurally generates terrain
// Spawns in resources and players
public class LevelGenerator : MonoBehaviour
{

    public List<Sprite> waterTiles = new List<Sprite>();

    private int levelWidth  = 45;
    private int levelHeight = 45;
    private Vector3 center;
    private List<Vector3> spawnPos;
    private string[,] terrainMap; //Array for storing level floor map
    private SpriteManager spriteManager;
    private List<GameObject> playerObjectList;
    private List<int[]> takenPositions = new List<int[]>();

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
                else if (sample < 0.75)
                    terrainMap[x, y] = "Dirt";
                else
                    terrainMap[x, y] = "Wall";
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
                Tile t = (Tile) ScriptableObject.CreateInstance("Tile");
                SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, x, y);
                t = ti.tile;
                t = RotateTile(t, ti.rotation);
                if (terrainMap[x, y] == "Wall") { colliderMap.SetTile(pos, t); }
                else if (terrainMap[x, y] == "Water") { triggerMap.SetTile(pos, t); }
                else { tilemap.SetTile(pos, t); }
            }
        }

        //Borders around the level
        for (int i = 0; i < levelWidth; i++)
        {
            Vector3Int pos = new Vector3Int(i, -1, 0);
            Tile t = (Tile)ScriptableObject.CreateInstance("Tile");
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }

        for (int i = 0; i < levelHeight; i++)
        {
            Vector3Int pos = new Vector3Int(levelWidth - 1, i, 0);
            Tile t = (Tile)ScriptableObject.CreateInstance("Tile");
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }

        for (int i = 0; i < levelWidth; i++)
        {
            Vector3Int pos = new Vector3Int(i, levelHeight - 1, 0);
            Tile t = (Tile)ScriptableObject.CreateInstance("Tile");
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }

        for (int i = 0; i < levelHeight; i++)
        {
            Vector3Int pos = new Vector3Int(-1, i, 0);
            Tile t = (Tile)ScriptableObject.CreateInstance("Tile");
            SpriteManager.TileMap.TileInfo ti = spriteManager.GetTileSprite(t, terrainMap, tilemap, 0, -1);
            t = ti.tile;
            t = RotateTile(t, ti.rotation);
            colliderMap.SetTile(pos, t);
        }
    }


    public void GenerateBackgroundLevel(Tilemap tilemap, SpriteManager sm)
    {
        int width = 20;
        int height = 20;

        center = new Vector3(width / 2, height / 2, 0);

        float scale = 5.0f;
        float xOffset = UnityEngine.Random.Range(0f, 99999f);
        float yOffset = UnityEngine.Random.Range(0f, 99999f);

        terrainMap = new string[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                float xCoord = (float)x / width * scale + xOffset;
                float yCoord = (float)y / height * scale + yOffset;

                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                if (sample < 0.3)
                    terrainMap[x, y] = "Water";
                else if (sample < 0.6)
                    terrainMap[x, y] = "Grass";
                else
                    terrainMap[x, y] = "Dirt";
            }
        }

        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < terrainMap.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < terrainMap.GetUpperBound(1); y++)
            {
                Vector3Int pos = new Vector3Int(x-10, y-10, 0);
                Tile t = (Tile)ScriptableObject.CreateInstance("Tile");
                SpriteManager.TileMap.TileInfo ti = sm.GetTileSprite(t, terrainMap, tilemap, x, y);
                t = ti.tile;
                t = RotateTile(t, ti.rotation);
                tilemap.SetTile(pos, t);
            }
        }
    }


    private Tile RotateTile(Tile t, float rotation){
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotation), Vector3.one);
        t.transform = matrix;
        return t;
    }

    public List<GameObject> SpawnPlayers(GameObject p, LevelManager lm, int numPlayers)
    {

        List<GameObject> result = new List<GameObject>();
        playerObjectList = new List<GameObject>();
        for (int i = 1; i < 5; i++)
        {
            playerObjectList.Add(Resources.Load<GameObject>("Player/player" + i));
        }

        for (int i = 0; i < numPlayers; i++)
        {
            Vector3 pos = GetSpawnPos(center + spawnPos[i]);
            GameObject player = Instantiate(playerObjectList[i], pos, Quaternion.identity); // Spawn player object
            player.AddComponent<PlayerInfo>(); // Add PlayerInfo script
            player.GetComponent<PlayerInfo>().InitPlayerInfo(lm, i+1); // Initialize player info script
            GameObject playerGun = player.transform.Find("Fairies").gameObject; // Get a reference to the player's gun object
            playerGun.AddComponent<FairyController>(); // Add vacuum controller
            playerGun.GetComponent<FairyController>().SetVacuum(player.GetComponent<PlayerInfo>().GetVacuum(), lm);
            playerGun.AddComponent<ProjectileSpawner>(); // Add a projectile spawner script to the player gun
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.InitPlayerController(playerGun.GetComponent<FairyController>());
            result.Add(player); //Add player to our player list
			player.name = string.Format("Player{0}",i+1); // needs to be +1 since you add 1
        }
        return result;
    }


    public void SpawnResources(ResourceManager rm)
    {
        SpawnTrees(rm);
        SpawnRocks(rm);
        SpawnFire(rm);
    }


    public void SpawnMainMenuResources(ResourceManager rm)
    {
        // Fire
        for (int i = 0; i < 10; i++)
        {
            int[] spawnPoint = { UnityEngine.Random.Range(-10, 10),
                                 UnityEngine.Random.Range(-10, 10)};
            GameObject f = Instantiate(rm.GetResourceGameObject("Fire"), new Vector3(spawnPoint[0], spawnPoint[1], 0.3f), Quaternion.identity);
            f.GetComponent<Resource>().InitResource(5, "Fire");
        }

        //Rocks
        List<int[]> rockSpawnPoints = GetRockSpawnPoints();

        for (int j = 0; j < 10; j++)
        {
            int i = UnityEngine.Random.Range(0, rockSpawnPoints.Count);
            int[] spawnPoint = rockSpawnPoints[i];
            GameObject r = Instantiate(rm.GetResourceGameObject("Rock"), new Vector3(spawnPoint[0] - 0.05f-10, spawnPoint[1] + 0.25f-10, 0f), Quaternion.identity);
            r.GetComponent<Resource>().InitResource(2, "Rock");
        }

        //Trees
        List<int[]> treeSpawnPoints = GetTreeSpawnPoints();

        for (int j = 0; j < 2; j++)
        {
            int i = UnityEngine.Random.Range(0, treeSpawnPoints.Count);
            int[] spawnPoint = treeSpawnPoints[i];
            GameObject t = Instantiate(rm.GetResourceGameObject("Tree"), new Vector3(spawnPoint[0]-10, spawnPoint[1]-10, 0f), Quaternion.identity);
            t.GetComponent<Resource>().InitResource(5, "Tree");
        }  
    }


    public List<int[]> GetWaterTiles(Tilemap tm)
    {
        List<int[]> result = new List<int[]>();

        for (int x = 0; x < terrainMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < terrainMap.GetUpperBound(1); y++)
            {
                int[] temp = { x, y };
                if (terrainMap[x, y] == "Water")
                    result.Add(temp);
            }
        }

        return result;
    }


    private void SpawnFire(ResourceManager rm)
    {
        for (int i = 0; i < 40;)
        {
            int[] spawnPoint = { UnityEngine.Random.Range(0, terrainMap.GetUpperBound(0)), 
                                 UnityEngine.Random.Range(0, terrainMap.GetUpperBound(1))};
            if ((terrainMap[spawnPoint[0], spawnPoint[1]] != "Water" || terrainMap[spawnPoint[0], spawnPoint[1]] != "Wall") && !takenPositions.Contains(spawnPoint))
            {
                i++;
                GameObject f = Instantiate(rm.GetResourceGameObject("Fire"), new Vector3(spawnPoint[0]+0.5f, spawnPoint[1]+0.5f, 0.3f), Quaternion.identity);
                f.GetComponent<Resource>().InitResource(5, "Fire");
            }
        }
    }


    private void SpawnRocks(ResourceManager rm)
    {
        List<int[]> rockSpawnPoints = GetRockSpawnPoints();

        for (int j = 0; j < 35; j++)
        {
            int i = UnityEngine.Random.Range(0, rockSpawnPoints.Count);
            int[] spawnPoint = rockSpawnPoints[i];
            takenPositions.Add(spawnPoint);
            GameObject r = Instantiate(rm.GetResourceGameObject("Rock"), new Vector3(spawnPoint[0] - 0.05f, spawnPoint[1] + 0.25f, 0f), Quaternion.identity);
            r.GetComponent<Resource>().InitResource(2, "Rock");
        }
    }


    private void SpawnTrees(ResourceManager rm)
    {
        List<int[]> treeSpawnPoints = GetTreeSpawnPoints();

        for (int j = 0; j < 10; j++)
        {
            int i = UnityEngine.Random.Range(0, treeSpawnPoints.Count);
            int[] spawnPoint = treeSpawnPoints[i];
            treeSpawnPoints.Remove(spawnPoint);
            takenPositions.Add(spawnPoint);
            takenPositions.Add(new int[] { spawnPoint[0]+1, spawnPoint[1]   });
            takenPositions.Add(new int[] { spawnPoint[0]+2, spawnPoint[1]   });
            takenPositions.Add(new int[] { spawnPoint[0]+3, spawnPoint[1]   });
            takenPositions.Add(new int[] { spawnPoint[0]  , spawnPoint[1]+1 });
            takenPositions.Add(new int[] { spawnPoint[0]+1, spawnPoint[1]+1 });
            takenPositions.Add(new int[] { spawnPoint[0]+2, spawnPoint[1]+1 });
            takenPositions.Add(new int[] { spawnPoint[0]+3, spawnPoint[1]+1 });
            treeSpawnPoints = GetTreeSpawnPoints();
            GameObject t = Instantiate(rm.GetResourceGameObject("Tree"), new Vector3(spawnPoint[0], spawnPoint[1], 0f), Quaternion.identity);
            t.GetComponent<Resource>().InitResource(5, "Tree");
        }
    }


    private List<int[]> GetRockSpawnPoints()
    {
        int[] check = new int[2];
        List<int[]> result = new List<int[]>();
        for (int x = 0; x < terrainMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < terrainMap.GetUpperBound(1); y++)
            {
                check[0] = x;
                check[1] = y;
                if (terrainMap[x, y] == "Dirt" && !takenPositions.Contains(check)) { int[] temp = { x, y }; result.Add(temp); }
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

                    if (IsAllSame(positions, "Grass") && ContainsNone(x, y)) { int[] temp = { x, y }; result.Add(temp); }
                }
                catch (Exception e) {} ///Means one of our positions was invalid, and can't spawn
            }
        }
        return result;
    }


    private bool ContainsNone(int x, int y)
    {
        int[] p1 = { x, y };
        int[] p2 = { x+1, y };
        int[] p3 = { x+2, y };
        int[] p4 = { x+3, y };
        int[] p5 = { x, y+1 };
        int[] p6 = { x+1, y+1 };
        int[] p7 = { x+2, y+1 };
        int[] p8 = { x+3, y+1 };

        return !(takenPositions.Contains(p1) ||
                 takenPositions.Contains(p2) ||
                 takenPositions.Contains(p3) ||
                 takenPositions.Contains(p4) ||
                 takenPositions.Contains(p5) ||
                 takenPositions.Contains(p6) ||
                 takenPositions.Contains(p7) ||
                 takenPositions.Contains(p8));
    }


    private bool IsAllSame(string[] s, string type)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] != type) { return false; }
        }

        return true;
    }


    public Vector3 GetSpawnPos(Vector3 startPos)
    {
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


    private bool IsValidPosition(int x, int y)
    {
        return IsInBounds(x    , y)     && terrainMap[x    , y]     != "Wall" &&
               IsInBounds(x    , y - 1) && terrainMap[x    , y - 1] != "Wall" &&
               IsInBounds(x - 1, y)     && terrainMap[x - 1, y]     != "Wall" &&
               IsInBounds(x - 1, y - 1) && terrainMap[x - 1, y - 1] != "Wall";
    }


    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < levelWidth && y >= 0 && y < levelHeight;
    }


    private string GetBlockUnderPlayer(Vector3 pos)
    {
        return terrainMap[(int)pos.x, (int)pos.y];
    }
}
