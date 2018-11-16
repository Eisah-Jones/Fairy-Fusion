using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

    public void shootProjectile(int eID, LevelManager lm){
        GameObject prefab = lm.elemPrefabs[eID - 1];
        GameObject e = Instantiate(prefab, transform.position, transform.rotation);
        e.GetComponent<ElementObject>().initElement(lm, lm.elementManager.GetElementByID(eID), true);
    }

}
