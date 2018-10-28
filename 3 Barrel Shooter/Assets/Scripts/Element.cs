using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element{

    private int ID;
    private float damage;
    private float fireRate;
    private float range;
    private int capacity;
    private char ammoType;

    //Constructor
    public Element(int id, float dam, float fr, float r, int cap, char at){
        ID = id;
        damage = dam;
        fireRate = fr;
        range = r;
        capacity = cap;
        ammoType = at;
    }

    public void debugElement(){
        string s = "ID: " + ID.ToString() + "\nAmmoType: " + ammoType.ToString();
        Debug.Log(s);
    }

    public int GetCapacity(){
        return capacity;
    }
}
