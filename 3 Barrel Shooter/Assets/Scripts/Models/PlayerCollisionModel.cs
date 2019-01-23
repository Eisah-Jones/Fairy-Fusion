using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// For when a player is hit by an element; functions should return (new player state, new element state)
public class PlayerCollisionModel {

    ElementManager em;

    // Data type for PlayerCollisionModel result; stores resulting health and player effect
    public class CollisionResult{

        public float health;
        //Made into a list to accomodate multiple playerEffects from one element
        public List<string> playerEffect;

        public CollisionResult(float h, List<string> e){
            health = h;
            playerEffect = e;
        }
    }

    // Constructor
    public  PlayerCollisionModel(ElementManager elemMan){
        em = elemMan;
    }

    public CollisionResult HandleCollision(float playerHealth, string name){
        float resultingHealth = playerHealth - em.GetDamageByName(name);
        //Will get actual effect list once implemented in ElementInfo and elementManager
        List<string> effectList = em.GetElementDataByName(name).playerCollisionEffects;
        Debug.Log(effectList);
        return new CollisionResult(resultingHealth, effectList);
    }
}
