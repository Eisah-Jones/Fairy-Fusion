using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFairies : MonoBehaviour
{

    private Transform fairy1;
    private Transform fairy2;
    private BoxCollider2D harnessArea;
    private GameObject fire;
    private TutorialElementSystem fireSystem;
    private GameObject fireball;
    private Transform spawnPos;
    private string harnessedElement = "";
    private bool harnessedLeft = false;
    private bool castedLeft = false;
    private bool harnessedRight = false;
    private bool hasCastedCombo = false;

    public void InitTutorialFairies()
    {
        fairy1 = transform.GetChild(1);
        fairy2 = transform.GetChild(2);
        harnessArea = GetComponent<BoxCollider2D>();
        spawnPos = transform.GetChild(0);
        GameObject tempFire = Resources.Load<GameObject>("Fluids/Fluid Systems/Tutorial_Fire");
        fire = Instantiate(tempFire, spawnPos.transform);
        fireSystem = fire.GetComponent<TutorialElementSystem>();
        fireSystem.InitTutorialElementSystem(spawnPos);
        fireball = Resources.Load<GameObject>("Elements/Tutorial_Fireball");
    }


    public void ShootFire(bool cl)
    {
        Debug.Log(cl);
        fireSystem.isEmitting = cl;
    }


    public void ShootCombo()
    {
        GameObject fb = Instantiate(fireball, spawnPos.position, spawnPos.rotation);
        fb.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;
        hasCastedCombo = true;
    }


    public void SetHarnessArea(bool b)
    {
        harnessArea.enabled = b;
        StartCoroutine("ResetHarnessArea");
    }


    public bool GetHarnessedLeft()
    {
        return harnessedLeft;
    }


    public bool GetHarnessedRight()
    {
        return harnessedRight;
    }


    public bool GetCastedLeft()
    {
        return castedLeft;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "R-1-Fire")
        {
            harnessedElement = "Fire";
            other.GetComponent<Shaker>().beingSucked = true;
            StartCoroutine("HandleElementShake", other);
        }
        else if (other.tag == "R-2-Rock")
        {
            harnessedElement = "Rock";
            other.GetComponent<Shaker>().beingSucked = true;
            StartCoroutine("HandleElementShake", other);
        }
    }


    private IEnumerator ResetHarnessArea()
    {
        yield return new WaitForSeconds(3f);
        harnessArea.enabled = false;
    }


    private IEnumerator HandleElementShake(Collider2D other)
    {
        yield return new WaitForSeconds(1f);
        if (other != null)
        {
            Destroy(other.gameObject);
            if (harnessedElement == "Fire" && !harnessedLeft && !harnessedRight)
            {
                harnessedElement = "";
                harnessedLeft = true;
                fairy1.GetChild(0).gameObject.SetActive(false);
                fairy1.GetChild(1).gameObject.SetActive(true);
            }
            else if (harnessedElement == "Rock" && harnessedLeft)
            {
                harnessedElement = "";
                harnessedRight = true;
                fairy2.GetChild(0).gameObject.SetActive(false);
                fairy2.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
