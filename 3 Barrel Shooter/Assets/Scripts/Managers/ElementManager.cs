using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ElementManager
{

    public ElementInfo loadedElementInfo;

    public List<elementData> elementDataList;

    private string rawJSON;

    public void initElementManager()
    {

        string filePath = Path.Combine(Application.streamingAssetsPath, "elements.json");

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            rawJSON = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            loadedElementInfo = JsonUtility.FromJson<ElementInfo>(rawJSON);
            //NEED TO PUT ALL ELEMENTS INTO AN ARRAY
            InitElementDataList();
        }
        else{ Debug.LogError("Cannot load element data!"); }
    }

    private void InitElementDataList()
    {

        elementDataList = new List<elementData>();
        // Must be added in order by ID!
        elementDataList.Add(loadedElementInfo.Fire);
        elementDataList.Add(loadedElementInfo.Rock);
        elementDataList.Add(loadedElementInfo.Water);
        elementDataList.Add(loadedElementInfo.Leaf);
        elementDataList.Add(loadedElementInfo.Air);
        elementDataList.Add(loadedElementInfo.Steam);
        elementDataList.Add(loadedElementInfo.Fireball);
        elementDataList.Add(loadedElementInfo.Mud);
        elementDataList.Add(loadedElementInfo.Sniper);
        elementDataList.Add(loadedElementInfo.SpikeShot);
        elementDataList.Add(loadedElementInfo.Laser);
    }

    public GameObject[] LoadElementPrefabs()
    {
        // Load Sprites from resources folders
        TextAsset txt = (TextAsset)Resources.Load("Elements/loadElements", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        GameObject[] result = new GameObject[lines.Length + 1];
        int j = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "")
                result[j++] = (GameObject)Resources.Load("Elements/" + lines[i]);
        }
        return result;
    }


    public string GetProjectileTypeByID(int id){
        if (id >= elementDataList.Capacity || id < 0) return "NONE";
        return elementDataList[id - 1].projectileType;
    }

    //Gets the capacity of an element given its ID
    public int GetCapacityByName(string name)
    {
        foreach(elementData eD in elementDataList)
        {
            if (eD.name == name)
                return eD.chamberCapacity;
        }
        return -1;
    }


    public float GetDamageByName(string name)
    {
        foreach (elementData eD in elementDataList)
        {
            if (eD.name == name)
                return eD.damage;
        }
        return -1;
    }


    public elementData GetElementDataByID(int id)
    {
        if (id == -1) { return null; }
        return elementDataList[id - 1];
    }


    public string GetElementNameByID(int i)
    {
        foreach(elementData eD in elementDataList)
        {
            if (eD.ID == i)
            {
                return eD.name;
            }

        }
        return "NONE";
    }


    public int GetElementIDByName(string n)
    {
        foreach (elementData eD in elementDataList)
        {
            if (eD.name == n)
            {
                return eD.ID;
            }

        }
        return -1;
    }


    public elementData GetElementDataByName(string n)
    {
        foreach (elementData eD in elementDataList)
        {
            if (eD.name == n)
            {
                return eD;
            }
        }
        return null;
    }

}