using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Vacuum class is used to represent all of the information stored in a player's vacuum
public class Vacuum{

    //Chamber represents one slot in the vacuum
    public class Chamber
    {

        // #######  INVENTORY PAIR CODE  #######

        //InventoryPair only used in the chamber, keeps track of element occurence in a vacuum chamber
        public class InventoryInfo
        {
            private string elementName;
            private int elementID;
            private int count;

            //Constructor
            public InventoryInfo(string n, int id, int c)
            {
                elementName = n;
                elementID = id;
                count = c;
            }

            //Increases count by amount i
            public void IncreaseCount(int i){
                count += i;
            }

            //Decreases count by amount i
            public void DecreaseCount(int i){
                if (count != 0)
                {
                    count -= i;
                }
            }

            //Returns the elementID
            public int GetElementID(){
                return elementID;
            }

            //Returns the count
            public int GetCount(){
                return count;
            }

            public string GetElementName(){
                return elementName;
            }

            //Returns the count
            public void SetCount(int c)
            {
                count = c;
            }

            //Returns the count
            public void SetElementID(int id)
            {
                elementID = id;
            }

        }



        // #######  CHAMBER CODE  #######


        LevelManager levelManager;

        //declare chamber variable
        private List<InventoryInfo> chamber;
        ChamberInteractionModel interactionModel;


        //Constructor
        public Chamber(LevelManager lm)
        {
            levelManager = lm;
            interactionModel = lm.chamberInteractionModel;

            //Assign chamber variable
            chamber = new List<InventoryInfo>();
        }


        //Adds given element to the chamber
        public Vacuum.Chamber Add(string name, int elemID)
        {
            //Check to see if the element is already in the chamber
            if (this.ContainsElement(elemID)){
                // **RUN CHAMBER INTERACTION SCRIPT**
                chamber = interactionModel.IncreaseElement(chamber, elemID);
                return this;
            }

            //If not in chamber, then we will add it, assuming it can be added
            chamber = interactionModel.AddToChamber(chamber, name, elemID);
            chamber = interactionModel.NormalizeChamber(chamber);
            return this;
        }


        //Removes an amount of element from a chamber, given that it is in the chamber
        public int Remove(int elemID){
            //Return 2 if element is successfully removed and chamber is now empty
            //Return 1 if element is successfully removed
            //Return 0 if not

            //Check to see if the element is in the chamber, if not return 0
            if (this.ContainsElement(elemID)){
                chamber = interactionModel.RemoveFromChamber(chamber, elemID);
                //If removal makes chamber empty, return 2
                //Else return 1
                return 1;
            }

            return 0;
        }


        //Returns whether or not the chamber contains an element
        private bool ContainsElement(int eID)
        {
            if (chamber.Count <= 0)
                return false;
            return chamber[0].GetElementID() == eID;
        }


        //Get number of elements in a chamber
        public int GetNumElements(){
            return chamber.Count;
        }


        //Get the ID of an element given its index
        public int GetElementIDByIndex(int i){
            return chamber[i].GetElementID();
        }


        //Get the contents of a chamber
        public List<InventoryInfo> GetContents(){
            return chamber;
        }


        //Set the contents of a chamber
        public void SetContents(List<InventoryInfo> c)
        {
            chamber = c;
        }
    }



    // #######  VACUUM CODE  #######

    // NOTE: CIM MAY NOT BE NECESSARY IN VACUUM, JUST IN CHAMBER

    //declare vacuum chamber array
    LevelManager levelManager;
    ChamberInteractionModel cim;
    private Chamber[] chambers;
    private int currentChamber;
    private bool vacuumOn; //True = ON, False = OFF

    // Vacuum constructor
    public Vacuum(LevelManager lm){
        levelManager = lm;

        cim = levelManager.chamberInteractionModel;

        //assign and populate chamber array
        chambers = new Chamber[3];
        currentChamber = 0;
        vacuumOn = false;

        for (int i = 0; i < 3; i++)
            chambers[i] = new Chamber(levelManager);
    }

    //Switch for turning on and off vacuum
    public void SetVacuum(bool b){
        vacuumOn = b;
    }

    //Get a boolean determining whether or not the vacuum is set to on
    public bool GetVacuumOn(){
        return vacuumOn;
    }

	public int GetCurrentChamberElement(){
		return chambers [currentChamber].GetElementIDByIndex (0);
	}

    //Change the chamber based on direction
    public int changeChamber(int direction){
        currentChamber = (currentChamber + direction)%3;
        return currentChamber;
    }

    //Add an item to the chamber given id
    public int AddToChamber(string n, int id){
        chambers[currentChamber] = chambers[currentChamber].Add(n, id);
        return 1;
    }
     
    //Returns the current chamber
    public Chamber GetCurrentChamber(){
        return chambers[currentChamber];
    }

    public bool isCurrentChamberEmpty(){
        return chambers[currentChamber].GetContents().Count == 0;
    }

    //Shooting scripts
    public Chamber.InventoryInfo Shoot(){

        //Check to make sure that we can shoot
        if (isCurrentChamberEmpty()){
            return null; //There is nothing to shoot! Return -1 to disapprove instantiating anything
        }

        //Get the elementID of element in the chamber
        string name = chambers[currentChamber].GetContents()[0].GetElementName();
        int id = chambers[currentChamber].GetContents()[0].GetElementID();

        Debug.Log("Current Element: " + name);

        Chamber.InventoryInfo result = chambers[currentChamber].GetContents()[0];


        //Remove that element from the chamber
        chambers[currentChamber].Remove(result.GetElementID());

        //return eID to give approval for instantiating
        return result;

        //NOTE: Element script on object will handle motion and collision of the element
    }

    //Used for debugging contents of a vacuum
    public void DebugVac(){
        string s = "";

        for (int i = 0; i < 3; i++){
            s += "Chamber " + i.ToString() + ":\n";
            foreach(Chamber.InventoryInfo elem in chambers[i].GetContents()){
                s += "  " + elem.GetElementName() + ": " + elem.GetCount().ToString() + "\n";
            }
        }

        Debug.Log(s);
    }

}
