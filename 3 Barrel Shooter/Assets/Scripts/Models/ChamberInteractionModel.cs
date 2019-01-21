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
    public List<Vacuum.Chamber.InventoryInfo> RemoveFromChamber(List<Vacuum.Chamber.InventoryInfo> c, int num){
        List<Vacuum.Chamber.InventoryInfo> result = c;

        result[0].DecreaseCount(num);

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


    public elementData GetElementCombination(Vacuum.Chamber c1, Vacuum.Chamber c2){
        elementData elem1 = em.GetElementDataByID(c1.GetElementIDByIndex(0));
        elementData elem2 = em.GetElementDataByID(c2.GetElementIDByIndex(0));

        if (elem1 == null || elem2 == null){
            return null;
        }

        //This returns the resulting name of the element of the chamber interaction
        string resultName = elem1.chamberInteractions.GetChamberResult(c2.GetElementNameByIndex(0));

        //Filter out results, call function to perform other actions
        if (resultName == "Ammo") return null;


        //If the number of elem1 and elem2 are enough to create an element return that element's data, else return null
        elementData e = em.GetElementDataByName(resultName);
        if (e == null) { return null; }

        combinationRequirements combReq = e.combinationRequirements;

        string elem1ReqName = combReq.elem1;
        int elem1ReqNum = combReq.elem1Num;

        string elem2ReqName = combReq.elem1;
        int elem2ReqNum = em.GetElementDataByName(resultName).combinationRequirements.elem2Num;

        // If there is enough of element 1 in the first chamber
        if (combReq.elem1 == elem1.name && combReq.elem1Num <= c1.GetAmountByIndex(0))
        {
            // If there is enough of element 2 in the second chamber
            if (combReq.elem2 == elem2.name && combReq.elem2Num <= c2.GetAmountByIndex(0))
                return em.GetElementDataByName(resultName);
        }
        // Check to see if elements are in opposite order
        else if (combReq.elem1 == elem2.name && combReq.elem1Num <= c2.GetAmountByIndex(0))
        {
            if (combReq.elem2 == elem1.name && combReq.elem2Num <= c1.GetAmountByIndex(0))
                return em.GetElementDataByName(resultName);
        }

        Debug.Log("NOT ENOUGH");
        return null;
    }
}
