using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour {

    private int maxResource = 5;
    private int resourceAvailable = 5;
    private string resourceName;
    private bool canBeCollected = true;
    public ParticleSystem smoke;
    private LevelManager lm;
    public AudioSource audiosource = new AudioSource();
    private SpriteRenderer spriteRenderer;
    private bool isGenerating;


    public void InitResource(int max, string name)
    {
        lm = FindObjectOfType<LevelManager>();
        audiosource = gameObject.AddComponent<AudioSource>();
        maxResource = max;
        resourceAvailable = max;
        resourceName = name;
        if (resourceName != "Tree")
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        else
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        isGenerating = false;
    }

    public void DecrementResource()
    {
        if (resourceAvailable != 0)
        {
            resourceAvailable -= 1;

            if (resourceAvailable == 0) 
            {
                lm.soundManager.PlaySoundByName(audiosource, "ResourcePickup", false, 1f);
                lm.SpawnParticleEffectAtPosition(gameObject.transform.position, 4);

                if (resourceName != "Tree")
                    StartCoroutine("DestroyResource");
                else
                {
                    StartCoroutine("RegenerateResource");
                }
            }
            else { StartCoroutine("CollectionReset"); }
        }
        
        SetTreeSprite();
    }

    public void IncrementResource()
    {
        if (resourceAvailable != maxResource)
            resourceAvailable += 1;
        SetTreeSprite();
    }

    public void ResetResource()
    {
        resourceAvailable = maxResource;
    }

    public bool CanCollect()
    {
        return resourceAvailable > 0 && canBeCollected;
    }


    private void SetTreeSprite()
    {
        if (resourceName != "Tree") return;

        if (resourceAvailable < 1) spriteRenderer.sprite = lm.spriteManager.GetTreeSprite(0);
        else if (resourceAvailable < 3 ) spriteRenderer.sprite = lm.spriteManager.GetTreeSprite(1);
        else spriteRenderer.sprite = lm.spriteManager.GetTreeSprite(2);
    }


    private IEnumerator DestroyResource()
    {
        yield return new WaitForSeconds(.2f);//WaitForFixedUpdate();
        //this.gameObject.SetActive(false);
        Destroy(gameObject);
    }


    private IEnumerator CollectionReset()
    {
        canBeCollected = false;
        yield return new WaitForSeconds(0.25f);
        canBeCollected = true;
    }


    private IEnumerator RegenerateResource()
    {
        if (!isGenerating)
        {
            isGenerating = true;
            for (int i = 0; i < 6; i++)
            {
                float t = Random.Range(4.0f, 10.0f);
                yield return new WaitForSeconds(t);
                IncrementResource();
            }
            isGenerating = false;
        }
    }
}
