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
            killCounter.InitKillCounter(levelManager, canvas);
            enemyArrows = new EnemyArrows();
            enemyArrows.InitEnemyArrows(this, levelManager.GetNumPlayers());
        }
    }


    public void InitUIManager()
    {
        Start();
    }


    void Update()
    {
        if (cdown != null && cdown.activeSelf != !isPaused)
            cdown.SetActive(!isPaused);

        if (levelManager != null && levelManager.GetIsGameOver())
        {
            cdown.SetActive(false);
            minimap.SetActive(false);
            endScreen.SetActive(true);
            winText.text = string.Format("Player {0} Wins!", levelManager.GetWinner());
            levelManager.SetIsOver(true);
        }

        if (levelManager != null)
        {
            enemyArrows.UpdateArrowPosition();
        }

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

        if (GetStartButton(ci) && SceneManager.GetActiveScene().name == "Level")
        {
            menus[0].GetMenu().SetActive(!menus[0].GetMenu().activeSelf);
            isPaused = menus[0].GetMenu().activeSelf;
            SetActiveMenu();
        }
        else if (activeMenu.GetName() == "TutorialMenu")
        {
            tutorialMenu.HandleInput(ci[0]);
        }
        else if (ci[0].A_Button && activeMenu.GetName() != "PlayerSelect" && activeMenu.GetActiveElement() != null)
        {
            activeMenu.GetActiveElement().Interact(0);
            SetActiveMenu();
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

    
    private bool GetStartButton(List<ControllerInputs> ci)
    {
        foreach (ControllerInputs i in ci)
        {
            if (i.Start_Button)
                return true;
        }
        return false;
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