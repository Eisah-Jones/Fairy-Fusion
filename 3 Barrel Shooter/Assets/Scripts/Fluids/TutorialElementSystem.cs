using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialElementSystem : MonoBehaviour
{
    private GameObject particle;
    private Transform spawnPos;
    public bool isEmitting = false;

    public void InitTutorialElementSystem(Transform sp)
    {
        spawnPos = sp;
        particle = Resources.Load<GameObject>("Fluids/Tutorial Particle");
        particle.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Fluids/Fluid Sprites/Fire");
        StartCoroutine("SpawnParticles");
    }

    private IEnumerator SpawnParticles()
    {
        float emissionDelay;
        while (!isEmitting)
        {
            yield return new WaitForFixedUpdate();
        }
        while (isEmitting)
        {
            emissionDelay = Random.Range(0.001f, 0.023f);
            yield return new WaitForSeconds(emissionDelay);
            GameObject p = Instantiate(particle, transform.position, Quaternion.identity);
            p.GetComponent<TutorialParticle>().Move(spawnPos);
        }
    }
}
