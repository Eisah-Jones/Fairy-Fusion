using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines the actions for interactions of elements inside a vacuum chamber
//This is where all the functions for handling chamber interactions will be programmed and mapped
public class ChamberInteractionModel
{
    // Establish elementManager for the model
    ElementManager em;

    public ChamberInteractionModel(ElementManager elemMan){
        em = elemMan;
    }

    // Increases the amount of an element in a chamber
    public List<Vacuum.Chamber.InventoryInfo> IncreaseElement(List<Vacuum.Chamber.InventoryInfo> c, int id){

        List<Vacuum.Chamber.InventoryInfo> result = c;

        // Unit value will vary per element, how much ammo is added per 1 element unit?
        int unitValue = 1;
        result[0].SetCount(result[0].GetCount() + unitValue);

        return result;
    }


    // Adds an element to a chamber
    public List<Vacuum.Chamber.InventoryInfo> AddToChamber(List<Vacuum.Chamber.InventoryInfo> c, string name, int id)
    {
        List<Vacuum.Chamber.InventoryInfo> result = c;

        // Unit value will vary per element, how much ammo is added per 1 element unit?
        int unitValue = 1;
        result.Add(new Vacuum.Chamber.InventoryInfo(name, id, unitValue));

        return result;
    }


    // Removes an element from the chamber
    public List<Vacuum.Chamber.InventoryInfo> RemoveFromChamber(List<Vacuum.Chamber.InventoryInfo> c, int id){
        List<Vacuum.Chamber.InventoryInfo> result = c;

        result[0].DecreaseCount(1);

        if(result[0].GetCount() <= 0){
            result.RemoveAt(0);
        }

        return result;
    }



    //General function for combining elements in chamber
    //Assuming always 2 objects in the chamber
    public List<Vacuum.Chamber.InventoryInfo> NormalizeChamber(List<Vacuum.Chamber.InventoryInfo> c){
        List<Vacuum.Chamber.InventoryInfo> newChamber = c;
        if (c.Count == 2)
        {
            int n1 = c[0].GetElementID();
            string n2 = c[1].GetElementName();

            string result = em.GetElementDataByID(n1).chamberInteractions.GetChamberResult(n2);

            if (result == "") // There was no reaction with the added element, remove and return
                return new List<Vacuum.Chamber.InventoryInfo>(){c[0]};

            //This will be a mapped function for normalization conversion
            int count = 1;
            newChamber = new List<Vacuum.Chamber.InventoryInfo> { new Vacuum.Chamber.InventoryInfo(result, em.GetElementIDByName(result), count) };
        }
        return newChamber;
    }
}
