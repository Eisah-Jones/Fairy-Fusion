using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager
{

    LevelManager levelManager;

    private List<Controller> controllers;

    public void InitControllerManager(LevelManager lm)
    {
        controllers = new List<Controller>();
        for (int i = 0; i < lm.GetNumPlayers(); i++)
        {
            Controller c = new Controller();
            c.initController(i+1);
            controllers.Add(c);
        }
    }


    public void InitControllerManagerMenus(int numPlayers)
    {
        controllers = new List<Controller>();
        for (int i = 0; i < numPlayers; i++)
        {
            Controller c = new Controller();
            c.initController(i + 1);
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
