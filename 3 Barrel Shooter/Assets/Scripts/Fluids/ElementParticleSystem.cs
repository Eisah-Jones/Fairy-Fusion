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

    [Range(0f, 5f)]
    public float particleLife;

    [Range(0f, 2f)]
    public float lifeRange;

    [Range(0f, 5000f)]
    public float particleForce;

    [Range(0f, 5000f)]
    public float forceRange;

    [Range(0.0f, 0.01f)]
    public float emissionRangeStart;

    [Range(0.0f, 1.00f)]
    public float emissionRange;

    [Range(0f, 360f)]
    public float spread;


    //private void Start()
    //{
    //    StartCoroutine("TESTING");
    //}

    //private IEnumerator TESTING()
    //{
    //    yield return new WaitForSeconds(5);
    //    LevelManager lm = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
    //    InitElementParticleSystem(lm, 6, transform, "TEST");
    //}

    public void InitElementParticleSystem(LevelManager lm, int id, Transform t, string o)
    {
        levelManager = lm;
        owner = o;
        isEmitting = true;
        particleID = id;
        transform.position = transform.position + new Vector3(0f, 0f, -4f);
        particleSprite = lm.spriteManager.GetElementParticleSpriteByID(id);
        particleName = particleSprite.name;
        particle = Resources.Load<GameObject>("Fluids/Fluid Particle");
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
            p.GetComponent<ElementParticle>().InitElementParticle(particleManager, particleID, particleLife, particleForce, rotation, owner, levelManager, particleName, spread, lifeRange, forceRange, particleSprite);
        }
    }
}
