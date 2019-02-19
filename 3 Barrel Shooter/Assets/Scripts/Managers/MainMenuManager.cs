using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    int mainIndex;
    int optionsIndex;
    float inputDelayTime = 0.2f;
    float buttonDeadZone = 0.65f;

    ControllerManager controllerManager = new ControllerManager();

    struct OptionUI
    {
        public Button back;
        public Slider volume;
    }

    RectTransform[] menus = new RectTransform[3];
    Button[] mainButtons = new Button[4];
    OptionUI options;
    Button tutorialBack;
    string currentMenu;
    Button currentButton;
    Slider currentSlider;

    bool isDetecingVerticalInput;
    bool isDetecingHorizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        controllerManager.InitControllerManagerMenus(1);

        foreach (Transform child in transform)
        {
            if (child.name == "MainMenu")
            {
                int i = 0;
                foreach (Transform c in child)
                {
                    mainButtons[i] = c.GetComponent<Button>();
                    i++;
                }
            }

            else if (child.name == "OptionsMenu")
            {
                options.back = child.Find("BackButton").GetComponent<Button>();
                options.volume = child.Find("Slider").GetComponent<Slider>();
                options.volume.normalizedValue = 0.5f;
            }

            else if (child.name == "TutorialMenu")
            {
                tutorialBack = child.Find("BackButton").GetComponent<Button>();
            }
        }

        currentMenu = "Main";
        mainButtons[0].Select();
        isDetecingVerticalInput = true;
        isDetecingHorizontalInput = true;
    }

    // Update is called once per frame
    void Update()
    {

        // Check for controller input, check what stage of the menus they are in
        List<ControllerInputs> ci = controllerManager.GetControllerInputs();

        if (isDetecingVerticalInput)
        {
            if (currentMenu == "Main")
            {
                int dir = GetLeftStickVertical(ci);
                mainIndex = (mainIndex + dir) % 4;
                if (mainIndex < 0) { mainIndex = 3; }
                currentButton = mainButtons[mainIndex];

                if (dir != 0)
                {
                    isDetecingVerticalInput = false;
                    StartCoroutine("DelayInputVertical");
                }
            }
            else if (currentMenu == "OptionsButton")
            {
                int dir = GetLeftStickVertical(ci);
                optionsIndex = (optionsIndex + dir) % 2;
                if (optionsIndex < 0) { optionsIndex = 1; }
                else if (optionsIndex == 0) { currentButton = null; currentSlider = options.volume; }
                else if (optionsIndex == 1) { currentButton = options.back; currentSlider = null; }
            }
            else if (currentMenu == "TutorialButton")
            {
                currentButton = tutorialBack;
            }

            if (currentButton != null)
                currentButton.Select();

            if (currentSlider != null)
                currentSlider.Select();
        }

        if (isDetecingHorizontalInput)
        {
            if (currentSlider != null)
            {
                float leftHorizontal = GetLeftStickHorizontal(ci);
                if (leftHorizontal != 0)
                {
                    isDetecingHorizontalInput = false;
                    StartCoroutine("DelayInputHorizontal");
                }

                if (leftHorizontal > 0)
                {
                    currentSlider.normalizedValue = currentSlider.normalizedValue + 0.01f;
                }
                else if (leftHorizontal < 0)
                {
                    currentSlider.normalizedValue = currentSlider.normalizedValue - 0.01f;
                }
            }
        }

        bool A_buttonPressed = ci[0].A_Button;
        if (A_buttonPressed && currentButton != null)
        {
            currentButton.onClick.Invoke();
            currentMenu = currentButton.name;
            if (currentMenu == "BackButton") { currentMenu = "Main"; currentSlider = null; }
        }
    }


    public int GetLeftStickVertical(List<ControllerInputs> ci)
    {
        int dir = 0;
        float leftStickVertical = ci[0].Left_Stick_Vertical;

        if (leftStickVertical < -buttonDeadZone) dir = -1;
        else if (leftStickVertical > buttonDeadZone) dir = 1;
        return dir;
    }


    public int GetLeftStickHorizontal(List<ControllerInputs> ci)
    {
        int dir = 0;
        float leftStickHorizontal = ci[0].Left_Stick_Horizontal;

        if (leftStickHorizontal < -buttonDeadZone) dir = -1;
        else if (leftStickHorizontal > buttonDeadZone) dir = 1;
        return dir;
    }


    public IEnumerator DelayInputVertical()
    {
        yield return new WaitForSeconds(inputDelayTime);
        isDetecingVerticalInput = true;
    }

    public IEnumerator DelayInputHorizontal()
    {
        yield return new WaitForSeconds(inputDelayTime/16);
        isDetecingHorizontalInput = true;
    }
}
