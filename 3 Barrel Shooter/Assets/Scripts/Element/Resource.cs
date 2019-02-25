using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    private int maxResource = 5;
    private int resourceAvailable = 5;
    private bool canBeCollected = true;
    public ParticleSystem smoke;
    private LevelManager lm;
    public AudioSource audiosource = new AudioSource();
    private void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }
    public void InitResource(int max){
        maxResource = max;
        resourceAvailable = max;
    }

    public void DecrementResource(){
        if (resourceAvailable != 0)
        {
            resourceAvailable -= 1;

            if (resourceAvailable == 0) 
            {

                lm.soundManager.PlaySoundByName(audiosource, "ResourcePickup");
                lm.SpawnParticleEffectAtPosition(gameObject.transform.position, 4);

                StartCoroutine("DestroyResource");
            }
            else { StartCoroutine("CollectionReset"); }
        }
    }

    public void IncrementResource(){
        if (resourceAvailable != maxResource)
            resourceAvailable += 1;
    }

    public void ResetResource(){
        resourceAvailable = maxResource;
    }

    public bool CanCollect(){
        return resourceAvailable > 0 && canBeCollected;
    }


    private IEnumerator DestroyResource()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }


    private IEnumerator CollectionReset(){
        canBeCollected = false;
        yield return new WaitForSeconds(0.25f);
        canBeCollected = true;
    }
}
