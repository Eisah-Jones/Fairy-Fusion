using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Menu activeMenu;
    private List<Menu> menus = new List<Menu>();

    private ControllerManager controllerManager = new ControllerManager();

    private bool isDetectingVerticalInput;
    private bool isDetectingHorizontalInput;

    private float buttonDeadZone = 0.65f;
    private float inputDelayTimeVertical = 0.2f;
    private float inputDelayTimeHorizontal = 0.013f;


    void Start()
    {
        controllerManager.InitControllerManagerMenus(4);
        
        foreach (Transform m in transform)
        {
            if (m.tag == "Menu")
            {
                Menu menu = new Menu();
                menu.InitMenu(m.gameObject);
                menus.Add(menu);
            }
        }

        activeMenu = menus[0];
        activeMenu.SetActiveElement(0);

        isDetectingVerticalInput   = true;
        isDetectingHorizontalInput = true;
    }


    void Update()
    {
        List<ControllerInputs> ci = controllerManager.GetControllerInputs();

        if (isDetectingVerticalInput)
        {
            int v = GetLeftStickVertical(ci);
            if (v != 0)
            {
                activeMenu.SetActiveElement(v);
                isDetectingVerticalInput = false;
                StartCoroutine("DelayInputVertical");
            }
        }

        if (isDetectingHorizontalInput)
        {
            if (activeMenu.GetActiveElement() != null && activeMenu.GetActiveElement().GetElementType() == "Slider")
            {
                int v = GetLeftStickHorizontal(ci);
                if (v != 0)
                {
                    activeMenu.GetActiveElement().Interact(v);
                    isDetectingHorizontalInput = false;
                    StartCoroutine("DelayInputHorizontal");
                }
            }
        }
        
        if (ci[0].A_Button && activeMenu.GetName() != "PlayerSelect")
        {
            activeMenu.GetActiveElement().Interact(0);
            SetActiveMenu();
        }
        else if (activeMenu.GetName() == "PlayerSelect")
        {
            if (activeMenu.GetPlayerSelectHandler().GetBack())
            {
                activeMenu.ResetMenu();
                SetActiveMenu();
                Debug.Log("BACK");
            }
            else
            {
                activeMenu.GetPlayerSelectHandler().HandleInput(ci);
            }
        }
    }


    private void SetActiveMenu()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].GetMenu().activeSelf)
            {
                activeMenu.ResetMenu();
                activeMenu = menus[i];
                activeMenu.SetActiveElement(0);
                return;
            }
        }
        activeMenu.ResetMenu();
        activeMenu = menus[0];
        activeMenu.GetMenu().SetActive(true);
        activeMenu.SetActiveElement(1); // Unity is dumb
        activeMenu.SetActiveElement(-1);
    }


    private int GetLeftStickVertical(List<ControllerInputs> ci)
    {
        int dir = 0;
        float leftStickVertical = ci[0].Left_Stick_Vertical;

        if (leftStickVertical < -buttonDeadZone) dir = -1;
        else if (leftStickVertical > buttonDeadZone) dir = 1;
        return dir;
    }


    private int GetLeftStickHorizontal(List<ControllerInputs> ci)
    {
        int dir = 0;
        float leftStickHorizontal = ci[0].Left_Stick_Horizontal;

        if (leftStickHorizontal < -buttonDeadZone) dir = -1;
        else if (leftStickHorizontal > buttonDeadZone) dir = 1;
        return dir;
    }



    private IEnumerator DelayInputVertical()
    {
        yield return new WaitForSeconds(inputDelayTimeVertical);
        isDetectingVerticalInput = true;
    }


    private IEnumerator DelayInputHorizontal()
    {
        yield return new WaitForSeconds(inputDelayTimeHorizontal);
        isDetectingHorizontalInput = true;
    }
}