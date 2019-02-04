using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementParticle : MonoBehaviour {

    int particleID;
    public float particleLife;
    Rigidbody2D body;


    public void InitElementParticle(ParticleManager pm, int id, float pLife, float pForce)
    {
        particleID = id;
        particleLife = pLife;

        gameObject.transform.rotation = new Quaternion(Quaternion.identity.x, Quaternion.identity.y, Random.Range(-180f, 180f), 100f);

        body = GetComponent<Rigidbody2D>();
        Vector2 dir = new Vector2(pForce, 0);
        body.AddForce(dir);
        body.AddTorque(100);

        StartCoroutine("DestroyParticle");
    }

    public int GetParticleID()
    {
        return particleID;
    }


    public IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(particleLife);
        Destroy(gameObject);
    }
}
