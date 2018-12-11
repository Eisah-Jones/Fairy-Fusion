using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

    private bool isShootingFluid;
    public ParticleSystem p;

    public void ShootProjectile(int eID, LevelManager lm, string playerName){
        GameObject prefab = lm.elemPrefabs[eID - 1];
        GameObject e = Instantiate(prefab, transform.position, transform.rotation);
        e.GetComponent<ElementObject>().initElement(lm, lm.elementManager.GetElementDataByID(eID), true, playerName);
    }

    public int ShootFluid(int eID, LevelManager lm, string playerName, Transform spawnPos){
        if (isShootingFluid) return -1;
        isShootingFluid = true;
        if (eID == 1){
            //instantiates flamethrower
            ParticleSystem prefab = lm.particles[0];
            p = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
            p.transform.parent = spawnPos;
            p.Play();
            StartCoroutine(fluidReset(p));
            return 1;
        }
        else if (eID == 3)
        {
            //instantiates water blast
            ParticleSystem prefab = lm.particles[1];
            p = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
            p.transform.parent = spawnPos;
            p.Play();
            StartCoroutine(fluidReset(p));
            return 1;
        }
        else if (eID == 6)
        {
            //instantiates steam
            ParticleSystem prefab = lm.particles[2];
            p = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
            p.transform.parent = spawnPos;
            p.Play();
            StartCoroutine(fluidReset(p));
            return 1;
        }

        return -1;
    }

    private IEnumerator fluidReset(ParticleSystem p){
        yield return new WaitForSeconds(1);
        isShootingFluid = false;
        p.Stop();
        yield return new WaitForSeconds(1f);
        Destroy(p.gameObject);

    }

}
