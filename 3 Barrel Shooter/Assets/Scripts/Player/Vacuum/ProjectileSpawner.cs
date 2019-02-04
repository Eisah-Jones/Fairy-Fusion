using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

    private SoundManager sm;
    private bool isShootingFluid;
    public GameObject p;
    public LevelManager lm;
    public AudioSource audioSource;
    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        lm = FindObjectOfType<LevelManager>();
        sm = lm.soundManager;
   
    }

    public void ShootProjectile(int eID, LevelManager lm, string playerName, Transform spawn)
    {
        GameObject prefab = lm.elemPrefabs[eID - 1];
        GameObject e = Instantiate(prefab, spawn.position, transform.rotation);
        //Debug.Log("e: " + e + eID);
        e.GetComponent<ElementObject>().initElement(lm, lm.elementManager.GetElementDataByID(eID), true, playerName);
        if (eID == 4)
        {
            sm.PlaySoundsByID(audioSource, 2); // plays wood chip sound
        }
        else if (eID == 2)
        {
            sm.PlaySoundsByID(audioSource, 4); // plays rock shot sound
        }
        else if (eID == 7)
        {
            sm.PlaySoundsByID(audioSource, 8); // plays fireball sound
        }
        else if (eID == 2)
        {
            sm.PlaySoundsByID(audioSource, 4); // plays rock shot sound
        }
    }

    public int ShootFluid(int eID, LevelManager lm, string playerName, Transform spawnPos)
    {
        if (isShootingFluid) return -1;

        isShootingFluid = true;

        if (eID == 1){
            //instantiates flamethrower

            Debug.Log("YO");
            sm.PlaySoundsByID(audioSource, 6);

            GameObject prefab = lm.fluidManager.GetFluidByID(eID);
            p = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
            p.GetComponent<ElementParticleSystem>().InitElementParticleSystem(lm, eID);

            p.transform.parent = spawnPos;
            StartCoroutine(fluidReset(p));
            return 1;
        }
        else if (eID == 3)
        {
            //instantiates water blast
            sm.PlaySoundsByID(audioSource, 0);

            GameObject prefab = lm.fluidManager.GetFluidByID(eID);
            p = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
            p.GetComponent<ElementParticleSystem>().InitElementParticleSystem(lm, eID);

            p.transform.parent = spawnPos;
            StartCoroutine(fluidReset(p));
            return 1;
        }
        else if (eID == 6)
        {
            //instantiates steam
            sm.PlaySoundsByID(audioSource, 7);

            GameObject prefab = lm.fluidManager.GetFluidByID(eID);
            p = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
            p.GetComponent<ElementParticleSystem>().InitElementParticleSystem(lm, eID);

            p.transform.parent = spawnPos;
            StartCoroutine(fluidReset(p));
            return 1;
        }

        return -1;
    }


    private IEnumerator fluidReset(GameObject ps){
        yield return new WaitForSeconds(1);
        isShootingFluid = false;
        sm.StopSound(audioSource);
        yield return new WaitForSeconds(1);
        ps.GetComponent<ElementParticleSystem>().DestroyParticleSystem();
    }

}
