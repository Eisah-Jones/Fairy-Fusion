using System.Collections;
using System.Collections.Generic;
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
        else
        {
            Debug.LogError("Cannot load element data!");
        }
    }

    private void InitElementDataList()
    {

        elementDataList = new List<elementData>();

        // Must be added in order by ID!
        elementDataList.Add(loadedElementInfo.Fire);
        elementDataList.Add(loadedElementInfo.Rock);
        elementDataList.Add(loadedElementInfo.Water);
        elementDataList.Add(loadedElementInfo.Wood);
        elementDataList.Add(loadedElementInfo.Air);
        elementDataList.Add(loadedElementInfo.Steam);
        elementDataList.Add(loadedElementInfo.Fireball);
        elementDataList.Add(loadedElementInfo.Mud);
        elementDataList.Add(loadedElementInfo.Stakes);
    }


    public string GetProjectileTypeByID(int id){
        return elementDataList[id - 1].projectileType;
    }


    // Does not work!
    //Gets the capacity of an element given its ID
    //public int GetCapacityByName(string name)
    //{
    //    // loadedElementInfo.Element.chamberCapacity
    //    return ExpressionEvaluator.Evaluate<int>(string.Format("loadedElementInfo.{0}.chamberCapacity", name));
    //}


    public float GetDamageByName(string n)
    {
        foreach (elementData eD in elementDataList)
        {
            if (eD == null) continue;
            if (eD.name == n)
            {
                return eD.damage;
            }
        }

        return -1;
    }


    public elementData GetElementDataByID(int id){
        if (id == -1) { return null; }
        return elementDataList[id - 1];
    }


    public int GetElementIDByName(string n){
        foreach (elementData eD in elementDataList){
            if (eD.name == n){
                return eD.ID;
            }
        }

        // If no element is found
        return -1;
    }


    public elementData GetElementDataByName(string n)
    {
        foreach (elementData eD in elementDataList)
        {
            // tempfix
            if (eD == null) { continue; }
            if (eD.name == n)
            {
                return eD;
            }
        }

        // No element found
        return null;
    }

}