using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ResourceManager {

    GameObject[] resourceObjects;

    // Use this for initialization
    public ResourceManager() {
        LoadResourceObjects();
	}


    public GameObject GetResourceGameObject(string name)
    {
        if (name == "Fire") { return resourceObjects[0]; }
        if (name == "Rock") { return resourceObjects[1]; }
        if (name == "Tree") { return resourceObjects[2]; }
        return null;
    }


    private void LoadResourceObjects()
    {
        TextAsset txt = (TextAsset)Resources.Load("Sprites/loadResourceSprites", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        resourceObjects = new GameObject[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string filePath = "Sprites/" + lines[i];
            resourceObjects[i] = Resources.Load<GameObject>(filePath);
        }
    }
}
