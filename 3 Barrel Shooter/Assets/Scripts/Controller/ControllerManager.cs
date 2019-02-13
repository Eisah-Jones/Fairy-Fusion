using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    LevelManager levelManager;

    private List<Controller> controllers;

    public void InitControllerManager(LevelManager lm)
    {
        controllers = new List<Controller>();
        for (int i = 1; i < 4; i++)
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
        foreach (Controller c in controllers)
        {
            result.Add(c.GetInputs());
            i++;
        }

        return result;
    }
}
