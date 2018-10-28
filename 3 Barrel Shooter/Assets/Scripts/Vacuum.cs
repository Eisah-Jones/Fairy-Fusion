using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Vacuum class is used to represent all of the information stored in a player's vacuum
public class Vacuum{

    //Chamber represents one slot in the vacuum
    public class Chamber
    {
        //InventoryPair only used in the chamber, keeps track of element occurence in a vacuum chamber
        public class InventoryPair
        {

            private int elementID;
            private int count;

            //Constructor
            public InventoryPair(int id, int c)
            {
                elementID = id;
                count = c;
            }

            //Increases count by amount i
            public void IncreaseCount(int i){
                count += i;
            }

            //Decreases count by amount i
            public void DecreaseCount(int i){
                count -= i;
            }

            //Returns the elementID
            public int GetElementID(){
                return elementID;
            }

            //Returns the count
            public int GetCount(){
                return count;
            }

            //Returns the count
            public void SetCount(int c)
            {
                count = c;
            }

            //Returns the count
            public void SetElementID(int eID)
            {
                elementID = eID;
            }

        }


        // #######  CHAMBER CODE  #######


        //declare chamber variable
        private List<InventoryPair> chamber;
        ChamberInteractionModel interactionModel = new ChamberInteractionModel();


        //Constructor
        public Chamber()
        {
            //Assign chamber variable
            chamber = new List<InventoryPair>();
        }

        //Adds given element to the chamber
        public Vacuum.Chamber Add(int elemID){
            //Check to see if the element is already in the chamber
            if (this.ContainsElement(elemID)){
                // **RUN CHAMBER INTERACTION SCRIPT**
                chamber = interactionModel.IncreaseElement(chamber, elemID);
                return this;
            }

            //If not in chamber, then we will add it, assuming it can be added
            chamber = interactionModel.AddToChamber(chamber, elemID);
            chamber = interactionModel.NormalizeChamber(chamber);
            return this;
        }

        private bool ContainsElement(int eID){
            if (chamber.Count <= 0)
                return false;
            return chamber[0].GetElementID() == eID;
        }


        public int Remove(int elemID){
            //Return 2 if element is successfully removed and chamber is now empty
            //Return 1 if element is successfully removed
            //Return 0 if not

            //Check to see if the element is in the chamber, if not return 0
            foreach(InventoryPair elem in chamber){
                if (elem.GetElementID() == elemID){
                    // **RUN CHAMBER INTERACTION SCRIPT**

                    //If removal makes chamber empty, return 2
                    //Else return 1
                    return 1;
                }
            }

            return 0;
        }

        public int GetNumElements(){
            return chamber.Count;
        }

        public int GetElementIDByIndex(int i){
            return chamber[i].GetElementID();
        }

        public List<InventoryPair> GetContents(){
            return chamber;
        }

        public void SetContents(List<InventoryPair> c)
        {
            chamber = c;
        }
    }


    // #######  VACUUM CODE  #######

    //declare vacuum chamber array
    ChamberInteractionModel cim = new ChamberInteractionModel();
    private Chamber[] chambers;
    private int currentChamber;

    public Vacuum(){

        //assign and populate chamber array
        chambers = new Chamber[3];
        currentChamber = 0;

        chambers[0] = new Chamber();
        chambers[1] = new Chamber();
        chambers[2] = new Chamber();

    }

    //Change the chamber based on direction
    public int changeChamber(int direction){
        currentChamber = (currentChamber + direction)%3;
        return currentChamber;
    }

    public int AddToChamber(int id){
        chambers[currentChamber] = chambers[currentChamber].Add(id);
        return 1;
    }

    public void DebugVac(){
        string s = "CHAMBER 0:\n";

        foreach (Chamber.InventoryPair e in chambers[0].GetContents()){
            s += e.GetElementID().ToString() + ": " + e.GetCount().ToString() + "\n";
        }

        Debug.Log(s);
    }

}
