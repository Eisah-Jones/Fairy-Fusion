using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines the actions for interactions of elements inside a vacuum chamber
//This is where all the functions for handling chamber interactions will be programmed and mapped
public class ChamberInteractionModel
{
    //Establish elementManager for the model
    ElementManager em = new ElementManager();

    //Used to store elementID for interactions
    int ID1;
    int ID2;
    string functionCall;


    public List<Vacuum.Chamber.InventoryPair> IncreaseElement(List<Vacuum.Chamber.InventoryPair> c, int id){

        List<Vacuum.Chamber.InventoryPair> result = c;

        //Unit value will vary per element, how much ammo is added per 1 element unit?
        int unitValue = 1;
        result[0].SetCount(result[0].GetCount() + unitValue);

        return result;
    }



    public List<Vacuum.Chamber.InventoryPair> AddToChamber(List<Vacuum.Chamber.InventoryPair> c, int id){
        List<Vacuum.Chamber.InventoryPair> result = c;

        //Unit value will vary per element, how much ammo is added per 1 element unit?
        int unitValue = 1;
        result.Add(new Vacuum.Chamber.InventoryPair(id, unitValue));

        return result;
    }


    //General function for combining elements in chamber
    //Assuming always 2 objects in the chamber
    public List<Vacuum.Chamber.InventoryPair> NormalizeChamber(List<Vacuum.Chamber.InventoryPair> c){
        Debug.Log("Normalizing Chamber");
        List<Vacuum.Chamber.InventoryPair> result = c;
        if (c.Count == 2)
        {
            ID1 = Mathf.Min(result[0].GetElementID(), result[1].GetElementID());
            ID2 = Mathf.Max(result[0].GetElementID(), result[1].GetElementID());
            functionCall = "Interaction" + ID1.ToString() + ID2.ToString();
            Debug.Log("Running " + functionCall);
            List<Vacuum.Chamber.InventoryPair> tempList = PerformInteraction(ID1, ID2, result);
            result = tempList;
        }
        return result;
    }


    private List<Vacuum.Chamber.InventoryPair> PerformInteraction(int id1, int id2, List<Vacuum.Chamber.InventoryPair> c)
    {
        if(id1 == 1 && id2 == 2){
            return Interaction12(c);
        }

        return c;
    }





    //Fire & Rock interaction: +5 fire ammo
    private List<Vacuum.Chamber.InventoryPair> Interaction12(List<Vacuum.Chamber.InventoryPair> c)
    {
        List<Vacuum.Chamber.InventoryPair> result = c;

        //Max out amount, but do not let go over capacity
        if (em.GetCapacityByID(1) < result[0].GetCount() + 5){
            result[0].SetCount(em.GetCapacityByID(1));
        } else {
            result[0].SetCount(result[0].GetCount() + 5);
        }

        //New element should always be fire; reassign ID and remove rock
        result[0].SetElementID(1);
        result.RemoveAt(result.Count - 1);

        return result;
    }

    //Fire & Water interaction

    //Fire & Wood interaction

    //Rock & Rock interaction
}
