using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for Element GameObject and storage of updated element statistics
public class ElementObject : MonoBehaviour {

    LevelManager levelManager;
    ElementCollisionModel elementCollisionModel;


    private string owner; //player who shoots the object
    private int ID; //The id of the element
    private float damage; //The damage that the object does to a player
    private float life; //The time that an object remains a projectile
    private float speed; //The speed of the elementObject
    private bool isProjectile; //Dictates whether or not an element does damage to a player
    private Vector2 direction;
    private string particleOwner;

    public void initElement(LevelManager lm, elementData e, bool isP, string o)
    {
        levelManager = lm;
        elementCollisionModel = lm.elementCollisionModel;

        ID = e.ID;
        name = e.name;
        damage = e.damage;
        life = e.projectileLife;
        speed = 12.5f;
        isProjectile = isP;
        owner = o;
        direction = transform.right;
    }

    private void Update()
    {
        // Check the life of the projectile, if <= 0, set isProjectile false
        if (life <= 0){
            isProjectile = false;
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            owner = null;
            direction = transform.right;
            if (name == "SpikeShot") { StartCoroutine("Explode"); }
            else Destroy(gameObject);
        }
        
        // If the element is a projectile, then we move it and decrease its life
        if (isProjectile && life > 0)
        {
            life -= 1;
            transform.GetComponent<Rigidbody2D>().velocity = direction * speed; //Replace number with speed when ready
        }
    }

    // Gets the element ID
    public int GetID()
    {
        return ID;
    }

    public string GetName()
    {
        return name;
    }

    public string GetOwner()
    {
        return owner;
    }

    // Gets the projectile state of the element
    public bool GetIsProjectile(){
        return isProjectile;
    }


    // When an element collides with something else
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We don't care if we collide with these objects
        if (collision == null || collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Walls") return;
        if (collision.gameObject.tag == "4-Leaf")
        {
            if (collision.gameObject.GetComponent<ElementObject>().owner == owner) return;
        }
        if (collision.gameObject.tag == "Player")
        {
        
            if (collision.GetComponent<PlayerInfo>().GetPlayerName() == owner) return;
            
            else if (name == "SpikeShot") StartCoroutine("Explode");
            else Destroy(gameObject);
        }

        ElementObject other = collision.gameObject.GetComponent<ElementObject>();
        //if (other != null && other.GetOwner() == owner) return;

        // This happens when objects are not spawned by the levelGenerator
        if (elementCollisionModel == null) return;

        // Get the ID of the element we collided with
        int ID2;
        if (!int.TryParse(collision.tag[0].ToString(), out ID2))
        {
            return;
        }

        ElementCollisionModel.CollisionResult cr = elementCollisionModel.HandleInteraction(ID, ID2);

        int i = 0;
        foreach( string result in cr.elementResults )
        {
            if (result == "Destroy"){
                // Get environmental effect and then run it
                // Destroy gameobject
                if (i == 0) // We are evaluating outcome of THIS element object
                    Destroy(this.gameObject);
                else // We are evaluating outcome of COLLISION element object
                    Destroy(collision.gameObject);
            }
            else if (result == "Stop")
            {
                direction = Vector2.zero;
            }
            else if (result == "Reflect")
            {
                direction = -transform.right;
                owner = null;
            }
            i++;
        }
    }


    //For the SpikeShot
    private IEnumerator Explode()
    {
        // Spawn leaves travelling in different directions
        Quaternion currentRotation = transform.rotation;
        string spawnElement = "Leaf";
        int spawnID = levelManager.elementManager.GetElementDataByName(spawnElement).ID;
        GameObject spawnObject = levelManager.elemPrefabs[spawnID - 1];
        for (int i = 0; i < 16; i++)
        {
            GameObject o = Instantiate(spawnObject, transform.position, transform.rotation);
            o.transform.rotation *= Quaternion.Euler(0, 0, 22.5f*i);
            o.GetComponent<ElementObject>().initElement(levelManager, levelManager.elementManager.GetElementDataByID(spawnID), true, owner);
        }
        Destroy(gameObject);
        yield return new WaitForEndOfFrame();
    }
}
