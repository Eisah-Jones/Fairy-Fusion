using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{

    int maxAvailable;
    int currentAvailable;

    bool isResetting;
    bool canHarvest;

    // Start is called before the first frame update
    void Start()
    {
        maxAvailable = 5;
        currentAvailable = 5;
        isResetting = false;
        canHarvest = true;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Fairies")
        {
            PlayerInfo player = collision.gameObject.GetComponentInParent<PlayerInfo>();
            if (currentAvailable > 0 && canHarvest && player.health < 100f)
            {
                player.health += 5f;
                currentAvailable -= 1;
                StartCoroutine("ResetHarvest");
            }
            else if (!isResetting)
            {
                isResetting = true;
                StartCoroutine("ResetWell");
            }
        }
        Debug.Log(currentAvailable);
    }


    private IEnumerator ResetWell()
    {
        while (maxAvailable < currentAvailable)
        {
            yield return new WaitForSeconds(10f);
            currentAvailable += 1;
        }
        isResetting = false;
    }


    private IEnumerator ResetHarvest()
    {
        canHarvest = false;
        yield return new WaitForSeconds(1f);
        canHarvest = true;
    }
}
