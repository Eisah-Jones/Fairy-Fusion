using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementParticleSystem : MonoBehaviour {


    int particleCount;
    GameObject particle;
    ParticleManager particleManager;
    bool infinite;

    [Range(0f, 2f)]
    public float particleLife;

    [Range(0f, 5000f)]
    public float particleForce;

    [Range(0.0f, 0.01f)]
    public float emissionRangeStart;

    [Range(0.0f, 1.00f)]
    public float emissionRange;


	// Use this for initialization
	void Start () 
    {

        InitElementParticleSystem(new ParticleManager(), 1);
	}


    public void InitElementParticleSystem(ParticleManager pm, int id)
    {
        infinite = true;
        particleCount = 50;
        particleManager = pm;
        particleLife = 1f;
        particleForce = 500f;
        emissionRangeStart = 0.001f;
        emissionRange = 0.001f;
        particle = pm.GetParticleByID(id);
        StartCoroutine("SpawnParticles");
    }

    private IEnumerator SpawnParticles()
    {
        float emissionDelay;
        for (int i = 0; i < particleCount;)
        {
            emissionDelay = Random.Range(emissionRangeStart, emissionRangeStart + emissionRange);
            yield return new WaitForSeconds(emissionDelay);
            GameObject p = Instantiate(particle, transform.position, Quaternion.identity);
            p.transform.SetParent(transform);
            Debug.Log(particleLife);
            p.GetComponent<ElementParticle>().InitElementParticle(particleManager, 1, particleLife, particleForce);
            if (!infinite) { i++; }
        }
    }
}
