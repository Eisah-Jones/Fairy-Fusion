using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The Vacuum class is used to represent all of the information stored in a player's vacuum
public class Fairies : MonoBehaviour{

    //Chamber represents one slot in the vacuum
    public class Fairy
    {
      
        // #######  INVENTORY PAIR CODE  #######

        //InventoryPair only used in the chamber, keeps track of element occurence in a vacuum chamber
        public class InventoryInfo
        {
            private string elementName;
            private int elementID;
            private int count;
            private int maxCount;

            //Constructor
            public InventoryInfo(string n, int id, int c)
            {
                elementName = n;
                elementID = id;
                count = c;
            }

            //Increases count by amount i
            public void IncreaseCount(int i)
            {
                if (count < maxCount)
                {
                    count += i;
                }
               
        
            }

            //Decreases count by amount i
            public void DecreaseCount(int i)
            {
                if (count != 0)
                {
                    count -= i;
                }
            }

            //Returns the elementID
            public int GetElementID()
            {
                return elementID;
            }

            //Returns the count
            public int GetCount()
            {
                return count;
            }

            public string GetElementName()
            {
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



        // #######  FAIRY CODE  #######


        LevelManager levelManager;

        //declare chamber variable
        private List<InventoryInfo> chamber;
        ChamberInteractionModel interactionModel;


        //Constructor
        public Fairy(LevelManager lm)
        {
            levelManager = lm;
            interactionModel = lm.chamberInteractionModel;

            //Assign chamber variable
            chamber = new List<InventoryInfo>();
        }
     
        

        //Adds given element to the chamber
        public Fairies.Fairy Add(string name, int elemID)
        {
            //Check to see if the element is already in the chamber
            if (this.ContainsElement(name))
            {

                chamber = interactionModel.IncreaseElement(chamber, elemID);
                
                return this;
            }
            //If not in chamber, then we will add it, assuming it can be added
            chamber = interactionModel.AddToChamber(chamber, name, elemID);
            return this;
        }


        //Removes an amount of element from a chamber, given that it is in the chamber
        public int Remove(string elemName, int num){
            //Return 2 if element is successfully removed and chamber is now empty
            //Return 1 if element is successfully removed
            //Return 0 if not

            //Check to see if the element is in the chamber, if not return 0
            if (ContainsElement(elemName)){
                chamber = interactionModel.RemoveFromChamber(chamber, num);
                //If removal makes chamber empty, return 2
                //Else return 1
                return 1;
            }

            return 0;
        }


        //Returns whether or not the chamber contains an element
        private bool ContainsElement(string name)
        {
            if (chamber.Count <= 0)
                return false;
            return chamber[0].GetElementName() == name;
        }


        //Get number of elements in a chamber
        public int GetNumElements(){
            return chamber.Count;
        }


        //Get the ID of an element given its index
        public int GetElementIDByIndex(int i){
            if (chamber.Count <= i) { return -1; }
            return chamber[i].GetElementID();
        }


        public int GetAmountByIndex(int i){
            if (chamber.Count <= i) { return -1; }
            return chamber[i].GetCount();
        }


        public string GetElementNameByIndex(int i){
            if (chamber.Count <= i) { return "NONE"; }
            return chamber[i].GetElementName();
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



    // #######  Fairies CODE  #######

    // NOTE: CIM MAY NOT BE NECESSARY IN VACUUM, JUST IN CHAMBER

    //declare vacuum chamber array
    LevelManager levelManager;
    ChamberInteractionModel cim;

    private Fairy[] chambers;
    private elementData[] combinationChambers;
    private int currentChamber;
    private bool vacuumOn1; //True = ON, False = OFF
    private bool vacuumOn2; // Tells whether or not we are sucking
    private bool shootingLeft;
    private bool shootingRight;
    private bool isCombiningElements; //True = combinationChambers, False = normalChambers
    private ProjectileSpawner spawner;
    bool removingDoubleSize = true;

    // Vacuum constructor
    public Fairies(LevelManager lm)
    {
        levelManager = lm;
   
        cim = levelManager.chamberInteractionModel;

        //assign and populate chamber array
        chambers = new Fairy[3];
        combinationChambers = new elementData[3];
        currentChamber = 0;
        vacuumOn1 = false;
        vacuumOn2 = false;
        isCombiningElements = true; //false for original control scheme

        for (int i = 0; i < 3; i++)
            chambers[i] = new Fairy(levelManager);
    }

    // Sets the combination chambers based on normal chambers
    public void SetCombinationChambers()
    {

        for (int i = 0; i < 3; i++)
        {
            elementData result = cim.GetElementCombination(chambers[i], chambers[(i + 1) % 3]);
            if (result != null)
            {
                combinationChambers[i] = result;
            }
        }
    }


    public void SetIsCombiningElements(bool b)
    {
        isCombiningElements = b;
    }


    //Switch for turning on and off vacuum
    public void SetVacuum(bool b1, bool b2)
    {
        vacuumOn1 = b1;
        vacuumOn2 = b2;
    }

    public Fairy[] GetChambers()
    {
        return chambers;
    }


    // Get a boolean determining whether or not the vacuum is set to on
    public bool GetVacuumOn()
    {
        return vacuumOn1 || vacuumOn2;
    }


    //Get the current element id from a chamber
	public int GetCurrentChamberElement()
    {
		return chambers [currentChamber].GetElementIDByIndex (0);
	}


    // Change the chamber based on direction
    public int changeChamber(int direction)
    {
        if (direction == 0) return currentChamber;
        currentChamber = (currentChamber + direction)%3;
        if (currentChamber == -1)
            currentChamber = 2;
        return currentChamber;
    }


    // Add an item to the chamber given id
    public int AddToChamber(string n, int id, bool left)
    {
        // If the item does not match what is in chamber, do nothing
        int selectedChamber;
        if (left) selectedChamber = currentChamber; 
        else selectedChamber = (currentChamber + 1) % 3;
        
        if (chambers[selectedChamber].GetElementIDByIndex(0) != id && chambers[selectedChamber].GetNumElements() != 0)
        {
            return -1;
        } 
       
        chambers[selectedChamber] = chambers[selectedChamber].Add(n, id);
        return 1;
    }

    public void SetShootingBools(bool left, bool right)
    {
        shootingLeft = left;
        shootingRight = right;
    }

    // Returns the current chamber
    public Fairy GetCurrentChamber(bool sucking)
    {
        int selectedChamber;
        bool b;
        if (sucking) { b = vacuumOn1; } else { b = shootingLeft; }
        if (b) selectedChamber = currentChamber;
        else selectedChamber = (currentChamber + 1) % 3;

        return chambers[selectedChamber];
    }


    public int GetCurrentChamberIndex()
    {
        return currentChamber;
    }


    // Returns the chamber given the index
    public Fairy GetChamberByIndex(int i)
    {
        return chambers[i];
    }


    public elementData GetCombinationByIndex(int i)
    {
        if (combinationChambers[i] == null) return new elementData();
        return combinationChambers[i];
    }


    public bool IsCurrentChamberEmpty(bool sucking)
    {
        int selectedChamber;
        bool b;
        if (sucking) { b = vacuumOn1; } else { b = shootingLeft; }
        if (b) selectedChamber = currentChamber;
        else selectedChamber = (currentChamber + 1) % 3;

        return chambers[selectedChamber].GetContents().Count == 0;
    }


    public bool IsCurrentChamberAtCapacity(bool sucking)
    {
        int selectedChamber;
        bool b;
        if (sucking) { b = vacuumOn1; } else { b = shootingLeft; }
        if (b) selectedChamber = currentChamber;
        else selectedChamber = (currentChamber + 1) % 3;

        if (chambers[selectedChamber].GetAmountByIndex(0) == -1)
            return false;

        return chambers[selectedChamber].GetAmountByIndex(0) == 
            levelManager.elementManager.GetCapacityByName(chambers[selectedChamber].GetElementNameByIndex(0));
    }


    public bool IsCombinationChamberEmpty()
    {
        return combinationChambers[currentChamber] == null;
    }


    public void RemoveFromCurrentChamber(string elemName, int i, bool shootingLeft, bool doubleSize)
    {

        if (elemName == "Air") return;
        if (IsCombo(elemName))
        {
            combinationRequirements cr = levelManager.elementManager.GetElementDataByName(elemName).combinationRequirements;
            chambers[currentChamber].Remove(cr.elem1, i);
            chambers[currentChamber].Remove(cr.elem2, i);
            chambers[(currentChamber + 1) % 3].Remove(cr.elem1, i);
            chambers[(currentChamber + 1) % 3].Remove(cr.elem2, i);
        }
        else
        {
            if (doubleSize)
            {
                //StartCoroutine("ResetDoubleRemoving");
                chambers[currentChamber].Remove(elemName, 2);
                chambers[currentChamber + 1].Remove(elemName, 2);
                Debug.Log("Removing Both");
            }
            else if (shootingLeft)
                chambers[currentChamber].Remove(elemName, i);
            else
            {
                Debug.Log("REMOVE ONE");
                chambers[currentChamber + 1].Remove(elemName, i);
            }
        }
    }


    public void CheckFluid(bool sL, bool sR, GameObject ps)
    {
        if (spawner == null)
            spawner = ps.GetComponent<ProjectileSpawner>();

        if (spawner.p == null) return;

        if (!sL && !sR)
            spawner.ResetFluidBool();
    }


    private bool IsCombo(string name)
    {
        return levelManager.elementManager.GetElementDataByName(name).combinationRequirements.elem1 == null;
    }


    private IEnumerator emptyChamberReset()
    {
        yield return new WaitForSeconds(1f);
    }


    private IEnumerator ResetDoubleRemoving()
    {
        removingDoubleSize = false;
        yield return new WaitForSeconds(0.1f);
        removingDoubleSize = true;
    }


    // Shooting scripts
    public Fairy.InventoryInfo Shoot(bool combo, int chamberNum)
    {
        if (combo && chambers[0].GetElementIDByIndex(0) == chambers[1].GetElementIDByIndex(0))
        {
            return null;
        }
        // Check to make sure that we can shoot
        // if the chamber is not empty; if there is nothing in the current chamber, 
        //  there can also be nothing in the current combination chamber
        // if there is something in the current chamber we must check to see if there is anything 
        //  in the combination chamber, if they are in combination chamber set
        if (IsCurrentChamberEmpty(false) || (IsCombinationChamberEmpty() && combo))
        {
            //Shoot air
            spawner.ResetFluidBool();
            Fairy.InventoryInfo air = new Fairy.InventoryInfo("Air", 5, 1);
            return air; //There is nothing to shoot! Return null to disapprove instantiating anything
        }


        // If we are combining elements, we need to take an element out num of elements from currentChamber and currentChamber+1
        if (combo)
        {
            int nextChamber = (currentChamber + 1) % 3;

            // Get the combinationRequirements of elements needed for each chamber
            elementData elemData = cim.GetElementCombination(chambers[currentChamber], chambers[nextChamber]);
            if (elemData == null) { return null; }
            combinationRequirements combReq = elemData.combinationRequirements;

            // Make sure we are taking away from the correct chambers
            if (chambers[currentChamber].GetContents()[0].GetElementName() == combReq.elem1)
            {
                int e1Num = combReq.elem1Num;
                int e2Num = combReq.elem2Num;

                chambers[currentChamber].Remove(combReq.elem1, e1Num);
                chambers[nextChamber].Remove(combReq.elem2, e2Num);
            }
            else
            {
                int e1Num = combReq.elem2Num;
                int e2Num = combReq.elem1Num;

                chambers[currentChamber].Remove(combReq.elem2, e1Num);
                chambers[nextChamber].Remove(combReq.elem1, e2Num);
            }

            elementData ed = combinationChambers[currentChamber];
            return new Fairy.InventoryInfo(ed.name, ed.ID, 1);
        }

        // If we are not combining elements
        // Get the elementID of element in the chamber
        int selectedChamber = (currentChamber + chamberNum) % 3;
        string name = chambers[selectedChamber].GetContents()[0].GetElementName();
        int id = chambers[selectedChamber].GetContents()[0].GetElementID();

        Fairy.InventoryInfo result = chambers[selectedChamber].GetContents()[0];

        // Remove that element from the chamber
        chambers[selectedChamber].Remove(result.GetElementName(), 1);
        
        
        // Returns what element was shot as InventoryInfo
        return result;

        //// NOTE: Element script on object will handle motion and collision of the element
    }

    // Used for debugging contents of a vacuum
    public void DebugVac(){
        string s = "";

        for (int i = 0; i < 3; i++){
            s += "Chamber " + i.ToString() + ":\n";
            foreach(Fairy.InventoryInfo elem in chambers[i].GetContents()){
                s += "  " + elem.GetElementName() + ": " + elem.GetCount().ToString() + "\n";
            }
        }

        Debug.Log(s);
    }

}
