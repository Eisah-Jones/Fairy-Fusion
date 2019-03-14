using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public Transform bar;
    float resetTimer;
    public GameObject hb;
    PlayerInfo pi;
    string playerName;
    // Use this for initialization
    //	private void Start () {
    //		bar = transform.Find ("Bar");
    //	}
    
    private void Start()
    {
       
        pi = GetComponentInParent<PlayerInfo>();
        if (pi != null)
            playerName = pi.GetPlayerName();
        StartCoroutine("LateStart", 1);

    }
   
     

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hb = GameObject.FindGameObjectWithTag("HealthBar");
        hb.gameObject.SetActive(false);
    }

    public void SetSize (float sizeNormalized)
    {
		bar.localScale = new Vector3 (sizeNormalized, 1f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ('R' == collision.tag[0] ||collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "5-Air" ||  collision.tag == "TilemapTrigger") return;
        if (collision.tag == "6-Steam" && collision.GetComponent<ElementParticle>().GetOwner() == playerName) return;
        hb.gameObject.SetActive(true);
    }

    IEnumerator StartTimer()
    {

        yield return new WaitForSeconds(3);
        hb.gameObject.SetActive(false);
        StopCoroutine("StartTimer");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Walls" || collision.tag == "Player" || collision.tag == "Untagged"|| collision.tag == "5-Air" ||  collision.tag == "TilemapTrigger") return;
        StartCoroutine("StartTimer");
    }

}
