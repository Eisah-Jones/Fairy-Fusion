using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//For when an projectile hits another projectile or a resource in the world
//Returns a CollisionResult with necessary informationH (state of elem1, state of elem2)
//Will depend on what types of interactions we have between elements
public class ElementCollisionModel{

    // Data structure for storing information from collision
    public class CollisionResult{

        public string elemResult1;
        public string elemResult2;

        public string[] elementResults;

        // Constructor
        public CollisionResult(collisionPair cp){
            elemResult1 = cp.c1;
            elemResult2 = cp.c2;

            elementResults = new string[2] { elemResult1, elemResult2 };
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

        elementData elem1 = em.GetElementDataByID(id1);
        elementData elem2 = em.GetElementDataByID(id2);

        collisionPair cp = elem1.elementCollisions.GetCollisionResult(elem2.name);

        CollisionResult result = new CollisionResult(cp);

        return result;
    }
}
