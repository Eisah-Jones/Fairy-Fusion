using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: ELEMENTS MUST BE ORDERED BY ID

[System.Serializable]
public class ChamberInteractions
{
    public string Fire;
    public string Rock;
    public string Water;
    public string Leaf;
    public string Air;
    public string Steam;
    public string Fireball;
    public string Mud;
    public string Sniper;
    public string SpikeShot;
    public string Laser;

    public string GetChamberResult(string name){

        if (name == "Fire") return Fire;

        if (name == "Rock") return Rock;

        if (name == "Water") return Water;

        if (name == "Leaf") return Leaf;

        if (name == "Air") return Air;

        if (name == "Steam") return Steam;

        if (name == "Fireball") return Fireball;

        if (name == "Mud") return Mud;

        if (name == "Sniper") return Sniper;

        if (name == "SpikeShot") return SpikeShot;

        if (name == "Laser") return Laser;

        return "None";
    }
}


[System.Serializable]
public class combinationRequirements
{
    public string elem1;
    public int elem1Num;
    public string elem2;
    public int elem2Num;
}

[System.Serializable]
public class collisionPair
{
    public string c1;
    public string c2;
}

[System.Serializable]
public class ElementCollisions
{
    public collisionPair Fire;
    public collisionPair Rock;
    public collisionPair Water;
    public collisionPair Leaf;
    public collisionPair Air;
    public collisionPair Steam;
    public collisionPair Fireball;
    public collisionPair Mud;
    public collisionPair Sniper;
    public collisionPair SpikeShot;
    public collisionPair Laser;

    public collisionPair GetCollisionResult(string name){
        if (name == "Fire") return Fire;

        if (name == "Rock") return Rock;

        if (name == "Water") return Water;

        if (name == "Leaf") return Leaf;

        if (name == "Air") return Air;

        if (name == "Steam") return Steam;

        if (name == "Fireball") return Fireball;

        if (name == "Mud") return Mud;

        if (name == "Sniper") return Sniper;

        if (name == "SpikeShot") return SpikeShot;

        if (name == "Laser") return Laser;

        return new collisionPair();
    }
}

[System.Serializable]
public class elementData
{
    public string name;
    public int ID;
    public float damage;
    public float fireRate;
    public string projectileType;
    public int projectileLife;
    public int chamberCapacity;
    public float speed;

    public List<string> playerCollisionEffects;
    public ChamberInteractions chamberInteractions;
    public ElementCollisions elementCollisions;
    public combinationRequirements combinationRequirements;
}

//Stores the base information for elements in the game
[System.Serializable]
public class ElementInfo
{
    public elementData Fire;
    public elementData Rock;
    public elementData Water;
    public elementData Leaf;
    public elementData Air;
    public elementData Steam;
    public elementData Fireball;
    public elementData Mud;
    public elementData Sniper;
    public elementData SpikeShot;
    public elementData Laser;
}
