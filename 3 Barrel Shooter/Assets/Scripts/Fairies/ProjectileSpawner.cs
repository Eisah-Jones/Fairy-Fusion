using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

    private SoundManager sm;
    private bool isShootingFluid;
    public GameObject p;
    public LevelManager lm;
    public AudioSource audioSource;

    private LineRenderer lineRenderer;


    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        lm = FindObjectOfType<LevelManager>();
        sm = lm.soundManager;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;
    }


    public void ShootProjectile(int eID, LevelManager lm, string playerName, Transform spawn)
    {
        GameObject prefab = lm.elemPrefabs[eID - 1];
        GameObject e = Instantiate(prefab, spawn.position, transform.rotation);
        e.GetComponent<ElementObject>().initElement(lm, lm.elementManager.GetElementDataByID(eID), true, playerName);
        //Debug.Log("Starting Projectile Sound: EID: " + eID);
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


    public int ShootFluid(int eID, LevelManager lm, string playerName, Transform spawnPos, string owner)
    {
        if (isShootingFluid) return -1;

        isShootingFluid = true;

        //instantiates flamethrower
        //Debug.Log("Starting Flame Sound: EID: " + eID);
        sm.PlaySoundsByID(audioSource, 6);

        GameObject prefab = lm.fluidManager.GetFluidByID(eID);
        p = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
        p.transform.SetParent(transform);
        p.GetComponent<ElementParticleSystem>().InitElementParticleSystem(lm, eID, spawnPos, owner);

        p.transform.parent = spawnPos;
        StartCoroutine(fluidReset(p));
        return 1;
    }


    public GameObject ShootLaser(string playerName, Transform spawnPos)
    {
        if (lineRenderer == null) return null;
        lineRenderer.enabled = true;
        RaycastHit2D hit = Physics2D.Raycast(spawnPos.position, transform.right);
        lineRenderer.SetPosition(0, spawnPos.position);
        lineRenderer.SetPosition(1, hit.point);
        StartCoroutine("laserReset");
        return hit.transform.gameObject;
    }

    private IEnumerator laserReset()
    {
        yield return new WaitForSeconds(0.5f);
        lineRenderer.enabled = false;
    }

    private IEnumerator fluidReset(GameObject ps){
        yield return new WaitForSeconds(1);
        isShootingFluid = false;
        sm.StopSound(audioSource);
        yield return new WaitForSeconds(1);
        ps.GetComponent<ElementParticleSystem>().DestroyParticleSystem();
    }
}
