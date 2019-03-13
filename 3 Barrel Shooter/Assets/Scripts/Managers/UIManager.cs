using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // General UI Input
    private Menu activeMenu;
    private List<Menu> menus = new List<Menu>();

    private TutorialMenu tutorialMenu;

    public LevelManager levelManager;
    private ControllerManager controllerManager = new ControllerManager();

    private bool isDetectingVerticalInput;
    private bool isDetectingHorizontalInput;
    private bool isPaused = false;

    private float buttonDeadZone = 0.65f;
    private float inputDelayTimeVertical = 0.2f;
    private float inputDelayTimeHorizontal = 0.013f;

    private int startButtonIndex = -1;

    // Level UI Elements
    private EnemyArrows enemyArrows;
    public GameObject canvas;
    public GameObject win;
    public GameObject minimap;
    public GameObject cdown;
    public KillCounter killCounter;
    public KillTracker killTracker;
    public Image skullIcon;
    public GameObject endScreen;
    public Text winText;
    private Countdown timer;


    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
        }

        controllerManager.InitControllerManagerMenus(4);
        
        foreach (Transform m in transform)
        {
            if (m.tag == "Menu")
            {
                Menu menu = new Menu();
                menu.InitMenu(m.gameObject);
                menus.Add(menu);
                if (menu.GetName() == "TutorialMenu")
                {
                    tutorialMenu = menu.GetMenu().GetComponent<TutorialMenu>();
                    tutorialMenu.InitTutorialMenu(this, levelManager);
                }
            }
        }

        activeMenu = menus[0];
        activeMenu.SetActiveElement(0);

        isDetectingVerticalInput   = true;
        isDetectingHorizontalInput = true;

        if (SceneManager.GetActiveScene().name == "Level") // Load the level UI
        {
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            cdown = canvas.transform.GetChild(2).gameObject;
            canvas.AddComponent<KillCounter>();
            killCounter = canvas.GetComponent<KillCounter>();
            killTracker = canvas.GetComponent<KillTracker>();
            enemyArrows = new EnemyArrows();
            enemyArrows.InitEnemyArrows(this, levelManager.GetNumPlayers());
            timer = canvas.transform.GetComponent<Countdown>();
        }
    }


    public void InitUIManager()
    {
        Start();
    }


    void Update()
    {
        if (cdown != null && cdown.activeSelf != isPaused)
            cdown.SetActive(!isPaused);

        if (levelManager != null )//&& timer.isGameOver)
        {
            cdown.SetActive(false);
            //endScreen.SetActive(true);
            winText.text = string.Format("Player {0} Wins!", levelManager.GetWinner());
            levelManager.SetIsOver(true);
        }

        if (levelManager != null && levelManager.playersSpawned)
        {
            enemyArrows.UpdateArrowPosition();
        }

        List<ControllerInputs> ci = controllerManager.GetControllerInputs();

        if (!isPaused)
        {
            startButtonIndex = GetStartButton(ci);
        }

        if (isDetectingVerticalInput)
        {
            int v;
            if (startButtonIndex == -1)
            {
                v = GetLeftStickVertical(ci);
            }
            else 
            {
                v = GetLeftStickVertical(ci[startButtonIndex]);
            }
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

        if (GetStartButton(ci) != -1 && SceneManager.GetActiveScene().name == "Level")
        {
            menus[0].GetMenu().SetActive(!menus[0].GetMenu().activeSelf);
            isPaused = menus[0].GetMenu().activeSelf;
            SetActiveMenu();
        }
        else if (activeMenu.GetName() == "TutorialMenu")
        {
            int r = tutorialMenu.HandleInput(ci[0].A_Button, ci[0].B_Button);
            if (r == -1) ToMainMenu();
        }
        else if (GetAButton(ci) && activeMenu.GetName() != "PlayerSelect" && activeMenu.GetActiveElement() != null)
        {
            if (isPaused && GetAButton(ci[startButtonIndex]))
            {
                activeMenu.GetActiveElement().Interact(0);
                SetActiveMenu();
            }
            else if (!isPaused)
            {
                activeMenu.GetActiveElement().Interact(0);
                SetActiveMenu();
            }
        }
        else if (ci[0].B_Button && activeMenu.GetName() == "OptionsMenu")
        {
            ToMainMenu();

        }
        else if (activeMenu.GetName() == "PlayerSelect")
        {
            if (activeMenu.GetPlayerSelectHandler().GetBack())
            {
                activeMenu.ResetMenu();
                SetActiveMenu();
            }
            else
            {
                activeMenu.GetPlayerSelectHandler().HandleInput(ci);
            }
        }
    }


    public bool GetPaused()
    {
        return isPaused;
    }


    public void ToMainMenu()
    {
        activeMenu.ResetMenu();
        activeMenu.GetMenu().SetActive(false);
        activeMenu = menus[0];
        activeMenu.GetMenu().SetActive(true);
        SetActiveMenu();
    }


    public Countdown GetTimer()
    {
        return timer;
    }


    private void SetActiveMenu()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].GetMenu().activeSelf)
            {
                activeMenu.ResetMenu();
                activeMenu = menus[i];
                activeMenu.SetActiveElement(1);
                activeMenu.SetActiveElement(-1);
                return;
            }
        }

        if (SceneManager.GetActiveScene().name != "Level")
        {
            activeMenu.ResetMenu();
            activeMenu = menus[0];
            activeMenu.GetMenu().SetActive(true);
            activeMenu.SetActiveElement(1); // Unity is dumb
            activeMenu.SetActiveElement(-1);
        }
    }


    private bool GetBButton(List<ControllerInputs> ci)
    {
        foreach (ControllerInputs c in ci)
        {
            if (c.B_Button) return true;
        }
        return false;
    }


    private bool GetBButton(ControllerInputs c)
    {
        return c.B_Button;
    }


    private bool GetAButton(List<ControllerInputs> ci)
    {
        foreach (ControllerInputs c in ci)
        {
            if (c.A_Button) return true;
        }
        return false;
    }


    private bool GetAButton(ControllerInputs c)
    {
        return c.A_Button;
    }


    private int GetLeftStickVertical(List<ControllerInputs> ci)
    {
        int dir = 0;
        foreach (ControllerInputs c in ci)
        {
            float leftStickVertical = c.Left_Stick_Vertical;

            if (leftStickVertical < -buttonDeadZone) dir = -1;
            else if (leftStickVertical > buttonDeadZone) dir = 1;
        }

        return dir;
    }

    private int GetLeftStickVertical(ControllerInputs c)
    {
        int dir = 0;
        float leftStickVertical = c.Left_Stick_Vertical;

        if (leftStickVertical < -buttonDeadZone) dir = -1;
        else if (leftStickVertical > buttonDeadZone) dir = 1;

        return dir;
    }


    private int GetLeftStickHorizontal(List<ControllerInputs> ci)
    {
        int dir = 0;
        foreach (ControllerInputs c in ci)
        {
            float leftStickHorizontal = c.Left_Stick_Horizontal;

            if (leftStickHorizontal < -buttonDeadZone) dir = -1;
            else if (leftStickHorizontal > buttonDeadZone) dir = 1;
        }

        return dir;
    }

    
    private int GetStartButton(List<ControllerInputs> ci)
    {
        int j = 0;
        foreach (ControllerInputs i in ci)
        {
            if (i.Start_Button)
                return j;
            j++;
        }
        return -1;
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