﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementParticleSystem : MonoBehaviour {


    int particleCount;
    GameObject particle;
    ParticleManager particleManager;
    bool isEmitting;
    Sprite particleSprite;

    [Range(0f, 2f)]
    public float particleLife;

    [Range(0f, 5000f)]
    public float particleForce;

    [Range(0.0f, 0.01f)]
    public float emissionRangeStart;

    [Range(0.0f, 1.00f)]
    public float emissionRange;


    public void InitElementParticleSystem(LevelManager lm, int id)
    {
        isEmitting = true;
        particleCount = 50;
        particleLife = 1f;
        particleForce = 500f;
        emissionRangeStart = 0.001f;
        emissionRange = 0.001f;
        particleSprite = lm.spriteManager.GetElementParticleSpriteByID(id);
        particle = Resources.Load<GameObject>("Fluids/Fluid Particle");
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
            GameObject p = Instantiate(particle, transform.position, Quaternion.identity);
            p.transform.SetParent(transform);
            p.GetComponent<SpriteRenderer>().sprite = particleSprite;
            p.GetComponent<ElementParticle>().InitElementParticle(particleManager, 1, particleLife, particleForce);
        }
    }
}