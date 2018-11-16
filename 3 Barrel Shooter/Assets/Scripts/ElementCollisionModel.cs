using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For when an projectile hits another projectile or a resource in the world
//Returns a CollisionResult with necessary informationH (state of elem1, state of elem2)
//Will depend on what types of interactions we have between elements
public class ElementCollisionModel{

    // Data structure for storing information from collision
    public class CollisionResult{

        // The elemN_Result is the respective resulting ID from collision 
        public int elem1_ResultID;
        public int elem2_ResultID;

        // The stateN is the respective resulting state code from collision
        public int state1;
        public int state2;

        // The envEffN is the respective envEff code from collision
        public int environmentalEffect1;
        public int environmentalEffect2;

        // Constructor
        public CollisionResult(int e1 = 0, int e2 = 0, int s1 = 0, int s2 = 0, int eff1 = 0, int eff2 = 0){
            elem1_ResultID = e1;
            elem2_ResultID = e2;

            state1 = s1;
            state2 = s2;

            environmentalEffect1 = eff1;
            environmentalEffect2 = eff2;
        }

        // Set the resulting element result IDs
        public void SetElems(int elem1, int elem2){
            elem1_ResultID = elem1;
            elem2_ResultID = elem2;
        }

        //Set the resulting states for each element in collision
        public void SetStates(int s1, int s2){
            state1 = s1;
            state2 = s2;
        }


        //Set the resulting environmental effects for each element in collision
        public void SetEnvironmentalEffect(int e1, int e2){
            environmentalEffect1 = e1;
            environmentalEffect2 = e2;
        }
    }


    //Declare elementManager for the model
    ElementManager em;

    public ElementCollisionModel(ElementManager elemMan){
        em = elemMan;
    }

    //ID mapping function for interaction functions
    public CollisionResult HandleInteraction(int id1, int id2){

        // Interactions where nothing happens will just return a newly 
        //   constructed result and do not need a function written

        CollisionResult result = new CollisionResult();

        Debug.Log("Collision Interaction");
        Debug.Log(id1);
        Debug.Log(id2);


        if (id1 == 2 && id2 == 2){
            return Interaction2_2();
        } 

        if (id1 == 1 && id2 == 3){
            return Interaction1_3();
        }

        return result;
    }


    // ## WRITE ELEMENT INTERACTION FUNCTIONS BELOW ##

    // Don't forget to map function in HandleInteraction!!

    // Rock - 2 v. Rock - 2: Both get destroyed
    private CollisionResult Interaction2_2(){
        return new CollisionResult(0, 0, -1, -1, 0, 0);
    }


    //Fire - 1 v. Water - 3: Fire destroyed, water survives
    private CollisionResult Interaction1_3(){
        return new CollisionResult(0, 3, -1, 0, 0, 0);
    }
}
