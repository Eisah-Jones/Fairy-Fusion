using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour {

    Vector2 startingPos;

    public int speed = 100;
    public float rangeAmount = .05f;
    public bool beingSucked = false;


    void Awake()
    {
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
        //Random.Range(0, rangeAmount);
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fairies")
        {
            beingSucked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (beingSucked)
        {
            this.transform.position = new Vector2(startingPos.x + Mathf.Sin(Time.time * speed) * Random.Range(0, rangeAmount), startingPos.y + (Mathf.Sin(Time.time * speed) * Random.Range(0, rangeAmount)));
        }
    }

}
