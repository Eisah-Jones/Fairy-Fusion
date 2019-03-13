using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    public GameObject Controls;
    public GameObject Sticks;
    public GameObject Shoulders;
    public GameObject Combo;
    public GameObject UI;

    private List<GameObject> stages;
    private int index;


    public void InitTutorialMenu(UIManager uim, LevelManager lm)
    {
        stages = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            stages.Add(transform.GetChild(i).gameObject);
        }

        ResetMenu();
    }


    public int HandleInput(bool a, bool b)
    {
        int result = 0;
        if (a)
        {
            stages[index++].SetActive(false);
            if (index == 5) result = -1;
            else stages[index].SetActive(true);
        }
        else if (b)
        {
            stages[index--].SetActive(false);
            if (index == -1) result = -1;
            else stages[index].SetActive(true);
        }

        if (result == -1) ResetMenu();

        return result;
    }


    public void ResetMenu()
    {
        index = 0;
        stages[index].SetActive(true);
    }
}
