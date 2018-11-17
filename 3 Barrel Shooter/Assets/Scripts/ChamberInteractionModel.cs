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
            string n1 = c[0].GetElementName();
            string n2 = c[1].GetElementName();

            string result = em.GetChamberInteractions(n1, n2);

            //This will be a mapped function for normalization conversion
            int count = 1;

            newChamber = new List<Vacuum.Chamber.InventoryInfo> { new Vacuum.Chamber.InventoryInfo(result, em.GetElementIDByName(result), count) };
        }
        return newChamber;
    }


    ////Runs interaction function between two elements and returns the correct chamber
    //private List<Vacuum.Chamber.InventoryPair> HandleInteraction(int id1, int id2, List<Vacuum.Chamber.InventoryPair> c)
    //{
    //    if(id1 == 1 && id2 == 2){
    //        return Interaction1_2(c);
    //    }

    //    if(id1 == 3 && id2 == 4){
    //        return Interaction3_4(c);
    //    }

    //    return c;
    //}


    //// ## ENCODE INTERACTION FUNCTIONS BELOW ##
    //// When complete, make sure to add mapping to the HandleInteraction function


    ////Fire - 1 & Rock - 2 interaction: +5 fire ammo
    //private List<Vacuum.Chamber.InventoryPair> Interaction1_2(List<Vacuum.Chamber.InventoryPair> c)
    //{
    //    List<Vacuum.Chamber.InventoryPair> result = c;

    //    //Max out amount, but do not let go over capacity
    //    if (em.GetCapacityByID(1) < result[0].GetCount() + 5){
    //        result[0].SetCount(em.GetCapacityByID(1));
    //    } else {
    //        result[0].SetCount(result[0].GetCount() + 5);
    //    }

    //    //New element should always be fire; reassign ID and remove rock
    //    result[0].SetElementID(1);
    //    result.RemoveAt(result.Count - 1);

    //    return result;
    //}


    ////Fire - 1 & Water - 3: Creates Steam



    ////Fire - 1 & Wood - 4: Increase fire range



    ////Rock - 2 & Water - 3: Mud ball



    ////Rock - 2 & Wood - 4: Birdshot



    ////Water - 3 & Wood - 4: Creates Stakes
    //private List<Vacuum.Chamber.InventoryPair> Interaction3_4(List<Vacuum.Chamber.InventoryPair> c)
    //{
    //    List<Vacuum.Chamber.InventoryPair> result = new List<Vacuum.Chamber.InventoryPair>();

    //    result.Add(new Vacuum.Chamber.InventoryPair(9, 5));

    //    return result;
    //}
}
