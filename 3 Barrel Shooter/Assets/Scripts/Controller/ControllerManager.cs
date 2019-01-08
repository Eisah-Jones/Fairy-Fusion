using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    LevelManager lm;

    private List<Controller> controllers;

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
