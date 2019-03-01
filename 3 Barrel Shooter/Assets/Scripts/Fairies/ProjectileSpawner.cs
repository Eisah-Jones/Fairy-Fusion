﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

    private SoundManager sm;
    private bool isShootingFluid;
    public GameObject p;
    public LevelManager lm;
    public AudioSource audioSourceProjectile;
    public AudioSource audioSourceFluid;
    public AudioSource backupAudio;



    private LineRenderer lineRenderer;
    private bool resetFluidShooting;


    public void Start()
    {
        audioSourceFluid = gameObject.AddComponent<AudioSource>();
        audioSourceProjectile = gameObject.AddComponent<AudioSource>();
        lm = FindObjectOfType<LevelManager>();
        sm = lm.soundManager;
        resetFluidShooting = false;

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
            sm.PlaySoundByName(audioSourceProjectile, "Leaf", false, .5f, 1.0f); // plays wood chip sound
        }
        else if (eID == 2)
        {
            sm.PlaySoundByName(audioSourceProjectile, "Rockshot"); // plays rock shot sound
        }
        else if (eID == 7)
        {
            sm.PlaySoundByName(audioSourceProjectile, "FireBall"); // plays fireball sound
        }
        else if (eID == 8)
        {
            sm.PlaySoundByName(audioSourceProjectile, "MudSplat"); // plays sound for mud
        }
        else if (eID == 9)
        {
            sm.PlaySoundByName(audioSourceProjectile, "Sniper");
        }
        else if (eID == 10)
        {
            sm.PlaySoundByName(audioSourceProjectile, "Spikeshot");
        }
   

    }


    public int ShootFluid(int eID, LevelManager lm, string playerName, Transform spawnPos, string owner)
    {
        if (isShootingFluid) return -1;

        isShootingFluid = true;

        //instantiates flamethrower
        //Debug.Log("Starting Flame Sound: EID: " + eID);
        if (eID == 1)
        {
            sm.PlaySoundByName(audioSourceFluid, "Flamethrower", backupAudio);
        }
        else if (eID == 3)
        {
            sm.PlaySoundByName(audioSourceFluid, "Water", backupAudio);
        }
        else if (eID == 6)
        {
            sm.PlaySoundByName(audioSourceFluid, "Steam", backupAudio);
        }
        //sm.PlaySoundsByID(audioSource, 0);

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
        sm.PlaySoundByName(audioSourceFluid, "Laser", backupAudio);
        lineRenderer.enabled = true;
        RaycastHit2D hit = Physics2D.Raycast(spawnPos.position, transform.right);
        lineRenderer.SetPosition(0, spawnPos.position);
        lineRenderer.SetPosition(1, hit.point);
        StartCoroutine("laserReset");
        return hit.transform.gameObject;
    }


    public void ResetFluidBool()
    {
        isShootingFluid = false;
    }


    private IEnumerator laserReset()
    {
        yield return new WaitForSeconds(0.5f);
        lineRenderer.enabled = false;
        sm.StopSound(audioSourceFluid);
    }

    private IEnumerator fluidReset(GameObject ps)
    {
        while (isShootingFluid)
        {
            yield return new WaitForEndOfFrame();
        }

        isShootingFluid = false;
        ps.GetComponent<ElementParticleSystem>().DestroyParticleSystem();
        sm.StopSound(audioSourceFluid);
    }
}
