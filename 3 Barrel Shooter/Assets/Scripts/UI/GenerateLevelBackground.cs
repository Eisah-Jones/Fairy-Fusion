using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateLevelBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelGenerator levelGen = new LevelGenerator();
        Tilemap tilemap = transform.GetChild(0).GetComponent<Tilemap>();
        SpriteManager spriteManager = new SpriteManager();
        ResourceManager resourceManager = new ResourceManager();
        levelGen.GenerateBackgroundLevel(tilemap, spriteManager);
        levelGen.SpawnMainMenuResources(resourceManager);
    }
}
