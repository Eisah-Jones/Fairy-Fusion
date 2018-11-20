using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for Element GameObject and storage of updated element statistics
public class ElementObject : MonoBehaviour {

    LevelManager levelManager;
    ElementCollisionModel elementCollisionModel;


    private int ID; //The id of the element
    private float damage; //The damage that the object does to a player
    private float life; //The time that an object remains a projectile
    private float speed; //The speed of the elementObject
    private bool isProjectile; //Dictates whether or not an element does damage to a player

    public void initElement(LevelManager lm, elementData e, bool isP){
        levelManager = lm;
        elementCollisionModel = lm.elementCollisionModel;

        ID = e.ID;
        name = e.name;
        damage = e.damage;
        life = 20;
        speed = 10.0f;
        isProjectile = isP;
    }

    private void Update()
    {
        // Check the life of the projectile, if <= 0, set isProjectile false
        if (life <= 0){
            isProjectile = false;
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }


        // If the element is a projectile, then we move it and decrease its life
        if (isProjectile && life > 0){
            life -= 1;
            transform.GetComponent<Rigidbody2D>().velocity = transform.right * speed; //Replace number with speed when ready
        }
    }

    // Gets the element ID
    public int GetID(){
        return ID;
    }

    public string GetName(){
        return name;
    }

    // Gets the projectile state of the element
    public bool GetIsProjectile(){
        return isProjectile;
    }

    // When an element collides with something else
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We don't care if we collide with these objects
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Untagged") return;

        Debug.Log(tag + " : " + collision.tag);

        // Get the ID of the element we collided with
        int ID2 = int.Parse(collision.tag[0].ToString());

        ElementCollisionModel.CollisionResult cr = elementCollisionModel.HandleInteraction(ID, ID2);

        Debug.Log("Collision Results : " + cr.elementResults[0].ToString());

        int i = 0;
        foreach( string result in cr.elementResults ){
            if (result == "Destroy"){
                // Get environmental effect and then run it
                // Destroy gameobject
                if (i == 0) // We are evaluating outcome of THIS element object
                    Destroy(this.gameObject);
                else // We are evaluating outcome of COLLISION element object
                    Destroy(collision.gameObject);
            }


            i++;
        }
    }
}
