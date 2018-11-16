using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for Element GameObject and storage of updated element statistics
public class ElementObject : MonoBehaviour {

    LevelManager levelManager;
    ElementCollisionModel elementCollisionModel;


    private int ID;
    private float damage;
    private float life;
    private float speed;
    private bool isProjectile;

    public void initElement(LevelManager lm, ElementInfo e, bool isP){
        levelManager = lm;
        elementCollisionModel = lm.elementCollisionModel;

        ID = e.GetID();
        damage = e.GetDamage();
        life = e.GetLife();
        speed = e.GetSpeed();
        isProjectile = isP;
    }

    private void Update()
    {
        //Check the life of the projectile, if <= 0, set isProjectile false
        if (life <= 0){
            isProjectile = false;
            life = 100;
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }


        //If the element is a projectile, then we move it and decrease its life
        if (isProjectile && life > 0){
            life -= 1;
            transform.GetComponent<Rigidbody2D>().velocity = transform.right * 10.0f;
        }
    }

    public int GetID(){
        return ID;
    }

    public bool GetIsProjectile(){
        return isProjectile;
    }

    // When an element collides with something else
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Untagged") return;

        Debug.Log(tag);
        Debug.Log(collision.tag);

        int ID2 = int.Parse(collision.tag[0].ToString());
        ElementCollisionModel.CollisionResult cr = elementCollisionModel.HandleInteraction(ID, ID2);

        // This is needed since scripts will call on both projectiles on collision
        // This allows us to call it on only one object in the collision

        Debug.Log("State1 " + cr.state1.ToString());
        Debug.Log(cr.state2);

        if (cr.state2 == -1){
            if (cr.environmentalEffect2 == 1){ //Steam explosion
                //Run steam explosion code
            } else if (cr.environmentalEffect2 == 2){
                //Run ground fire code
            }
            Destroy(collision.gameObject);
        }

        if (cr.state1 == -1){
            if (cr.environmentalEffect1 == 1)
            { //Steam explosion
                //Run steam explosion code
            }
            else if (cr.environmentalEffect1 == 2)
            {
                //Run ground fire code
            }
            Destroy(this.gameObject);
            return;
        }
    }
}
