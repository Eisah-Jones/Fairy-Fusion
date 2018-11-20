using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    LevelManager lm;

    private List<Controller> controllers;

    float horizontal;
    float vertical;
    bool suck;

    // Use this for initialization
    void Start () 
    {
        lm = GetComponent<LevelManager>();
        controllers = new List<Controller>();

        for (int i = 1; i < 3; i++)
        {
            Controller c = new Controller();
            c.initController(i);
            controllers.Add(c);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");
        //suck = Input.GetButton("VacuumButton_P1");
        ////Debug.Log(horizontal + ", " + vertical);
        //Debug.Log("P1: " + suck);
        //Debug.Log("P2: " + Input.GetButton("VacuumButton_P2"));
    }

    public List<ControllerInputs> GetControllerInputs()
    {
        List<ControllerInputs> result = new List<ControllerInputs>();

        int i = 0;
        foreach (Controller c in controllers){
            result.Add(c.GetInputs());
            i++;
        }

        return result;
    }
}
