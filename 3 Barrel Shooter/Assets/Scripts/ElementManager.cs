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

    private void InitElementDataList(){

        elementDataList = new List<elementData>();

        // Must be added in order by ID!
        elementDataList.Add(loadedElementInfo.Fire);
        elementDataList.Add(loadedElementInfo.Rock);
        elementDataList.Add(loadedElementInfo.Water);
        elementDataList.Add(loadedElementInfo.Wood);
        elementDataList.Add(loadedElementInfo.Air);
    }



    // TODO: FIX THESE FUNCTIONS BELOW!!!

    // Doesn't work
    //Gets the capacity of an element given its ID
    public int GetCapacityByID(string name)
    {
        // loadedElementInfo.Element.chamberCapacity
        return ExpressionEvaluator.Evaluate<int>(string.Format("loadedElementInfo.{0}.chamberCapacity", name));
    }

    // Doesn't work
    public float GetDamageByID(string name)
    {
        // loadedElementInfo.Fire.damage
        return ExpressionEvaluator.Evaluate<float>(string.Format("loadedElementInfo.{0}.damage", name));
    }

    // This works!
    public elementData GetElementDataByID(int id){
        return elementDataList[id - 1];
    }

    // Doesn't work
    public int GetElementIDByName(string n){
        foreach (elementData eD in elementDataList){
            if (eD.name == n){
                return eD.ID;
            }
        }

        //If no element is found
        return -1;
    }
}