using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    List<int> barrel1 = new List<int>();
    List<int> barrel2 = new List<int>();
    List<int> barrel3 = new List<int>();

    List<int> currentBarrel;

    [SerializeField]
    private SpriteRenderer gunHUD;
    [SerializeField]
    private SpriteRenderer b1HUD;
    [SerializeField]
    private SpriteRenderer b2HUD;
    [SerializeField]
    private SpriteRenderer b3HUD;

    bool isVacuumOn;

    BoxCollider2D gunArea;

    // Use this for initialization
    void Start () {
        isVacuumOn = false;
        gunArea = GetComponent<BoxCollider2D>();
        gunArea.enabled = false;
        currentBarrel = barrel1;
    }
	
	// Update is called once per frame
	void Update () {
        GetInput();
        updateGUI();
	}

    private void Switch(int direction)
    {
        if (direction == -1 && currentBarrel == barrel1)
            currentBarrel = barrel3;
        else if (direction == 1 && currentBarrel == barrel1)
            currentBarrel = barrel2;
        else if (direction == -1 && currentBarrel == barrel2)
            currentBarrel = barrel1;
        else if (direction == 1 && currentBarrel == barrel2)
            currentBarrel = barrel3;
        else if (direction == -1 && currentBarrel == barrel3)
            currentBarrel = barrel2;
        else if (direction == 1 && currentBarrel == barrel3)
            currentBarrel = barrel1;

        if (currentBarrel == barrel1){
            gunHUD.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        else if (currentBarrel == barrel2)
        {
            gunHUD.transform.eulerAngles = new Vector3(0, 0, -117);
        }

        else if (currentBarrel == barrel3)
        {
            gunHUD.transform.eulerAngles = new Vector3(0, 0, 117);
        }

    }

    private void GetInput(){

        if (Input.GetKey(KeyCode.Space))
            Vacuum(true);
        else
            Vacuum(false);

        if (Input.GetKeyDown(KeyCode.Q) && !isVacuumOn)
        {
            Switch(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isVacuumOn)
        {
            Switch(1);
        }
    }

    private void Vacuum(bool state){
        isVacuumOn = state;
        gunArea.enabled = state;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Rock")
        {
            currentBarrel.Add(0);
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "Water")
        {
            currentBarrel.Add(1);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Fire")
        {
            currentBarrel.Add(2);
            Destroy(other.gameObject);
        }
    }

    private void updateGUI(){
        if (barrel1.Count != 0){
            if (barrel1[0] == 0 && b1HUD.GetComponent<SpriteRenderer>().color != new Color(89/255f, 85f / 255f, 44f / 255f, 1f)){
                b1HUD.GetComponent<SpriteRenderer>().color = new Color(89f / 255f, 85f / 255f, 44f / 255f, 1f);
            } else if (barrel1[0] == 1 && b1HUD.GetComponent<SpriteRenderer>().color != new Color(42f / 255f, 230f / 255f, 255f / 255f, 1f))
            {
                b1HUD.GetComponent<SpriteRenderer>().color = new Color(42f / 255f, 230f / 255f, 255f / 255f, 1f);
            }
            else if (barrel1[0] == 2 && b1HUD.GetComponent<SpriteRenderer>().color != new Color(255f / 255f, 16f / 255f, 0f / 255f, 1f))
            {
                b1HUD.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 16f / 255f, 0f / 255f, 1f);
            }
        } else if (barrel1.Count == 0 && b1HUD.GetComponent<SpriteRenderer>().color != Color.white) {
            b1HUD.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (barrel2.Count != 0)
        {
            if (barrel2[0] == 0 && b2HUD.GetComponent<SpriteRenderer>().color != new Color(89 / 255f, 85f / 255f, 44f / 255f, 1f))
            {
                b2HUD.GetComponent<SpriteRenderer>().color = new Color(89f / 255f, 85f / 255f, 44f / 255f, 1f);
            }
            else if (barrel2[0] == 1 && b1HUD.GetComponent<SpriteRenderer>().color != new Color(42f / 255f, 230f / 255f, 255f / 255f, 1f))
            {
                b2HUD.GetComponent<SpriteRenderer>().color = new Color(42f / 255f, 230f / 255f, 255f / 255f, 1f);
            }
            else if (barrel2[0] == 2 && b1HUD.GetComponent<SpriteRenderer>().color != new Color(255f / 255f, 16f / 255f, 0f / 255f, 1f))
            {
                b2HUD.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 16f / 255f, 0f / 255f, 1f);
            }
        }
        else if (barrel2.Count == 0 && b2HUD.GetComponent<SpriteRenderer>().color != Color.white)
        {
            b2HUD.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (barrel3.Count != 0)
        {
            if (barrel3[0] == 0 && b3HUD.GetComponent<SpriteRenderer>().color != new Color(89 / 255f, 85f / 255f, 44f / 255f, 1f))
            {
                b3HUD.GetComponent<SpriteRenderer>().color = new Color(89f / 255f, 85f / 255f, 44f / 255f, 1f);
            }
            else if (barrel3[0] == 1 && b1HUD.GetComponent<SpriteRenderer>().color != new Color(42f / 255f, 230f / 255f, 255f / 255f, 1f))
            {
                b3HUD.GetComponent<SpriteRenderer>().color = new Color(42f / 255f, 230f / 255f, 255f / 255f, 1f);
            }
            else if (barrel3[0] == 2 && b1HUD.GetComponent<SpriteRenderer>().color != new Color(255f / 255f, 16f / 255f, 0f / 255f, 1f))
            {
                b3HUD.GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 16f / 255f, 0f / 255f, 1f);
            }
        }
        else if (barrel3.Count == 0 && b3HUD.GetComponent<SpriteRenderer>().color != Color.white)
        {
            b3HUD.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
