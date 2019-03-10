using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialParticle : MonoBehaviour
{
    public void Move(Transform sp)
    {
        gameObject.transform.rotation = sp.rotation;
        gameObject.transform.Rotate(new Vector3(0f, 0f, Random.Range(-10, 10)));


        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.AddForce(transform.right * 1050);

        StartCoroutine("DestroyParticle");
    }


    private IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
