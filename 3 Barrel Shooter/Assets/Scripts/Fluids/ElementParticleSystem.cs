using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementParticleSystem : MonoBehaviour {


    int particleCount;
    GameObject particle;
    int particleID;
    string particleName;
    ParticleManager particleManager;
    bool isEmitting;
    Sprite particleSprite;
    LevelManager levelManager;
    string owner;

    Transform rotation;

    [Range(0f, 2f)]
    public float particleLife;

    [Range(0f, 5000f)]
    public float particleForce;

    [Range(0.0f, 0.01f)]
    public float emissionRangeStart;

    [Range(0.0f, 1.00f)]
    public float emissionRange;


    //private void Start()
    //{
    //    InitElementParticleSystem(new LevelManager(), 1, new GameObject());
    //}

    public void InitElementParticleSystem(LevelManager lm, int id, Transform t, string o)
    {
        levelManager = lm;
        owner = o;
        isEmitting = true;
        particleID = id;
        particleCount = 50;
        particleLife = 1f;
        particleForce = 500f;
        emissionRangeStart = 0.001f;
        emissionRange = 0.001f;
        particleSprite = lm.spriteManager.GetElementParticleSpriteByID(id);
        particleName = particleSprite.name;
        particle = Resources.Load<GameObject>("Fluids/Fluid Particle");
        particle.GetComponent<SpriteRenderer>().sprite = particleSprite;
        rotation = t;
        StartCoroutine("SpawnParticles");
    }


    public void SetEmissionState(bool state)
    {
        isEmitting = state;
    }


    public void DestroyParticleSystem()
    {
        Destroy(gameObject);
    }


    private IEnumerator SpawnParticles()
    {
        float emissionDelay;
        while (isEmitting)
        {
            emissionDelay = Random.Range(emissionRangeStart, emissionRangeStart + emissionRange);
            yield return new WaitForSeconds(emissionDelay);
            GameObject p = Instantiate(particle, transform.position, rotation.rotation);
            p.GetComponent<ElementParticle>().InitElementParticle(particleManager, particleID, particleLife, particleForce, rotation, owner, levelManager, particleName);
        }
    }
}
