using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementParticle : MonoBehaviour {

    int particleID;
    string particleName;
    public float particleLife;
    string particleOwner;
    Rigidbody2D body;
    LevelManager levelManager;
    Light light;


    public void InitElementParticle(ParticleManager pm, int id, float pLife, float pForce, Transform t, string pO, LevelManager lm, string pName, float rotationRange, float lifeRange, float forceRange, Sprite s)
    {
        tag = id + "-" + pName;
        particleID = id;
        particleName = pName;
        float lR = Random.Range(-lifeRange, lifeRange);
        particleLife = pLife + lR;
        if (particleLife < 0) particleLife = 0.25f;

        GetComponent<SpriteRenderer>().sprite = s;

        gameObject.transform.rotation = t.rotation;
        gameObject.transform.Rotate(new Vector3(0f, 0f, Random.Range(-rotationRange, rotationRange)));

        body = GetComponent<Rigidbody2D>();
        float fR = Random.Range(-forceRange, forceRange);
        body.AddForce(transform.right * (pForce + fR));

        particleOwner = pO;
        levelManager = lm;

        light = transform.GetChild(0).GetComponent<Light>();
        SetLight();

        StartCoroutine("DestroyParticle");
    }


    private void SetLight()
    {
        int i = Random.Range(0, 2);
        if (particleName == "Steam") i = Random.Range(0, 500);
       
        if (i != 0)
        {
            light.enabled = false;
        }


        if (particleID == 3)
            light.color = Color.blue;
        else if (particleID == 6)
            light.color = Color.gray;

    }


    public int GetParticleID()
    {
        return particleID;
    }


    public string GetParticleName()
    {
        return particleName;
    }


    public IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(particleLife);
        Destroy(gameObject);
    }


    public string GetOwner()
    {
        return particleOwner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Tilemap_Collider")
        {
            Destroy(gameObject);
            return;
        }

        if (collision.tag == "Walls" || collision.tag == "Untagged" || collision.tag[0] == 'R' || collision.tag == "TEST")
        {
            return;
        }

        ElementParticle ep = collision.GetComponent<ElementParticle>();
        if (ep != null && ep.GetOwner() == GetOwner())
        {
            return;
        }


        PlayerInfo pi = null;
        string elemName = this.tag.Split('-')[1];
        if (collision.tag == "Player")
        {
            pi = collision.gameObject.GetComponent<PlayerInfo>();

        }

        if (pi == null) return;
        if (elemName == "Fire" || elemName == "Water" || elemName == "Steam") // checks if element is colliding with player and does damage if its enemy
        {
            if (particleOwner == ("Player" + pi.playerNum.ToString()))
            {
                return;
            }
            if (pi.health <= 0 )
            {
                //levelManager.GetKillCounter().addKill(pi.GetPlayerName(), particleOwner);
                pi.health = 0;
               
            }

            if (pi == null) return;

            PlayerCollisionModel.CollisionResult result = levelManager.playerCollisionModel.HandleCollision(pi.health, elemName);
            pi.health = result.health;
        }
    }
}
