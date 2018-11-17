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
    }



    // TODO: FIX THESE FUNCTIONS BELOW!!!

    //Gets the capacity of an element given its ID
    public int GetCapacityByID(string name)
    {
        // loadedElementInfo.Element.chamberCapacity
        return ExpressionEvaluator.Evaluate<int>(string.Format("loadedElementInfo.{0}.chamberCapacity", name));
    }

    public float GetDamageByID(string name)
    {
        // loadedElementInfo.Fire.damage
        return ExpressionEvaluator.Evaluate<float>(string.Format("loadedElementInfo.{0}.damage", name));
    }

    public string GetChamberInteractions(string n1, string n2)
    {
        return ExpressionEvaluator.Evaluate<string>(string.Format("loadedElementInfo.{0}.chamberInteractions.{1}", n1, n2));
    }

    public elementData GetElementDataByID(int id){
        return elementDataList[id - 1];
    }

    public int GetElementIDByName(string name){
        return ExpressionEvaluator.Evaluate<int>(string.Format("loadedElementInfo.{0}.id", name));
    }
}