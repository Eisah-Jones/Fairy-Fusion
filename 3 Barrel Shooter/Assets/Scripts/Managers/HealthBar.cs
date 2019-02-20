using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public Transform bar;
    float resetTimer;
    public GameObject hb;
    // Use this for initialization
    //	private void Start () {
    //		bar = transform.Find ("Bar");
    //	}

    private void Start()
    {
        hb = GameObject.FindGameObjectWithTag("HealthBar");
        hb.gameObject.SetActive(false);

    }
    public void SetSize (float sizeNormalized) {
		bar.localScale = new Vector3 (sizeNormalized, 1f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ('R' == collision.tag[0] ||collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged") return;
        hb.gameObject.SetActive(true);
    }

    IEnumerator StartTimer()
    {

        yield return new WaitForSeconds(4);
        hb.gameObject.SetActive(false);
        StopCoroutine("StartTimer");


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged") return;
        StartCoroutine("StartTimer");
    }

}
