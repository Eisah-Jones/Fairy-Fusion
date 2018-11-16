using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stores the base information for all Elements in the game
public class ElementInfo{

    private int ID;
    private float damage;
    private float fireRate;
    private float life;
    private float speed;
    private int resourceMax;
    private int capacity;
    private char ammoType;

    // Constructor
    public ElementInfo(int id, float dam, float fr, float l, int cap, char at){
        ID = id;
        damage = dam;
        fireRate = fr;
        life = l;
        capacity = cap;
        ammoType = at;
    }

    // Debug function for an element
    public void debugElement(){
        string s = "ID: " + ID.ToString() + "\nAmmoType: " + ammoType.ToString();
        Debug.Log(s);
    }


    public int GetID(){
        return ID;
    }


    public float GetDamage(){
        return damage;
    }


    public float GetFireRate(){
        return fireRate;
    }


    public float GetLife(){
        return life;
    }


    public float GetSpeed(){
        return speed;
    }


    public int GetResourceMax(){
        return resourceMax;
    }


    // Returns the capacity of an element
    public int GetCapacity(){
        return capacity;
    }

}
