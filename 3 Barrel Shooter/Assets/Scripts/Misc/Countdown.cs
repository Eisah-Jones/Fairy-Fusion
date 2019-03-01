using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Countdown : MonoBehaviour
{
    // Start is called before the first frame update
    public int roundEndLength = 3;
    public int startTime = 60; 
    private int timerLeft = 5;
    public int initialfontSize = 20;
    public int endofRoundfontSize = 32;
    private bool isPaused = false;
    private bool preTimer = true;
    public int preGameCounter = 3;
    private int roundNum = 1;
    public Transform LoadingBar;
    public Text TextIndicator;
    [SerializeField] private float currentTime;
    [SerializeField] private float speed = 1;
    LevelManager lm;
    AudioSource asource;
    private bool isFlashing;

    // Start is called before the first frame update
    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
        TextIndicator.fontSize = initialfontSize;
        currentTime = startTime;
        isFlashing = false;
       
        //lm.soundManager.StartBGMusic();
    }

    public void incrementRound()
    {
        roundNum++;
    }

    public void togglePause()
    {
        isPaused = !isPaused;
    }
    public void resetTimer()
    {
        currentTime = startTime;
    }
    public void setTimer(int n)
    {
        currentTime = n;
    }
    public IEnumerator FlashText()
    {
        while (currentTime < 11)
        { //keep looping while no gold

            TextIndicator.enabled = !TextIndicator.enabled; //flip the active state of goldText
            yield return new WaitForSeconds(.5f);// wait .5 seconds
        }
        TextIndicator.enabled = true; // Don't forget to flip it back on incase it was off when exiting the loop!
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPaused && currentTime > 0)
        {
            currentTime -= speed * Time.deltaTime;
        
            if (!isFlashing && currentTime < 11)
            {
                isFlashing = true;
                TextIndicator.color = Color.red;
                StartCoroutine("FlashText");
            }
            TextIndicator.text = ((int)currentTime).ToString();
        }
       
        else
        {
            //TextIndicator.gameObject.transform.position = new Vector3(TextIndicator.gameObject.transform.position.x, TextIndicator.gameObject.transform.position.y + 180, TextIndicator.gameObject.transform.position.z); ;
            TextIndicator.fontSize = endofRoundfontSize;
            // play explosion and add sound effects
            LoadingBar.gameObject.SetActive(false);
            LoadingBar.GetChild(0).gameObject.SetActive(false);
            TextIndicator.color = Color.white;
            TextIndicator.text = "Round " + roundNum +" over!";
            TextIndicator.text = "Game over!";
            List<string> winner = lm.GetKillCounter().GetWinner();
            if (winner.Count > 1) // if theres a tie
            {
                string winners = "";
                for (int i = 0; i < winner.Count; i++)
                {
                    winners += winner[i] + " -";
                }
                TextIndicator.fontSize = 13;
                TextIndicator.text = "It's a tie between " + winners;
            }
            else
            {
                TextIndicator.text = winner[0] + " is the WINNER!!";
            }
            lm.StopBGMusic();
            Time.timeScale = 0f;
        }
        LoadingBar.GetComponent<Image>().fillAmount = (currentTime) / (startTime);
   
    }
}
