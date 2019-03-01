using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAffector : MonoBehaviour
{
    private float speedMultiplier = 1f;
    private bool Burning = false;
    private GameObject playerBody;
    private PlayerController playerController;
    Color c;
    public void InitPlayerAffector(GameObject pb)
    {
        playerBody = pb;
        c = pb.GetComponent<SpriteRenderer>().material.color;
    }


    public void HandleEffects(List<string> effects, Transform t)
    {
        foreach (string e in effects)
        {
            if (e == "Knockback")
            {
                IEnumerator c = Knockback(t);
                StartCoroutine(c);
            }
            else if (e == "Burn" && !Burning)
            {
                Burning = true;
                StartCoroutine("Burn");
            }
            else if (e == "Pushback")
            {
                IEnumerator c = Pushback(t);
                StartCoroutine(c);
            }
            else if (e == "Slow")
            {
                StartCoroutine(e);
            }
        }
    }


    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }


    private IEnumerator ResetForces()
    {
        yield return new WaitForSeconds(0.05f);
        Vector2 v = playerBody.GetComponent<Rigidbody2D>().velocity;
        playerBody.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }


    private IEnumerator Knockback(Transform t)
    {
        Vector2 heading = playerBody.transform.position - t.position;
        heading = heading.normalized;

        playerBody.GetComponent<Rigidbody2D>().AddForce(heading * 15, ForceMode2D.Impulse);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.color = Color.grey;
        yield return new WaitForFixedUpdate();
        sr.material.color = c;
    }


    private IEnumerator Pushback(Transform t)
    {
        Vector2 heading = playerBody.transform.position - t.position;
        heading = heading.normalized;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.color = Color.grey;
        playerBody.GetComponent<Rigidbody2D>().AddForce(heading * 3, ForceMode2D.Impulse);
        yield return new WaitForFixedUpdate();
        sr.material.color = c;

    }


    private IEnumerator Burn()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.material.color = Color.red;
        int burnHits = Random.Range(1, 5);
        for (int i = 0; i < burnHits; i++)
        {
            int hitPoints = Random.Range(3, 6);
            if (gameObject.GetComponent<PlayerInfo>().health <= 0)
            {
                Burning = false;
            }
            else if (Burning)
            {
                gameObject.GetComponent<PlayerInfo>().RemovePlayerHealth(hitPoints);
                float waitTime = Random.Range(1.0f, 2.5f);
                yield return new WaitForSeconds(waitTime);
            }
        }
        Burning = false;
        yield return new WaitForFixedUpdate();
        sr.material.color = c;

    }


    private IEnumerator Slow()
    {

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.material.color = Color.gray;
        speedMultiplier = 0.5f;
        yield return new WaitForSeconds(3f);
        speedMultiplier = 1f;
        sr.material.color = c;
    }
}
