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
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        TextIndicator.fontSize = initialfontSize;
        currentTime = startTime;
    }
    public void incrementRound()
    {
        roundNum++;
    }


    public void InitStart()
    {
        //countText = GameObject.FindGameObjectWithTag("CountText").GetComponent<Text>();
    }


    public void startCountDown()
    {

    }


    public void startPreCountDown()
    {

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










    // Update is called once per frame
    void Update()
    {
        if (!isPaused && currentTime > 0)
        {
            currentTime -= speed * Time.deltaTime;
            if (currentTime < 11)
            {
                TextIndicator.fontSize = endofRoundfontSize;
            }
            TextIndicator.text = ((int)currentTime).ToString();
        }
        else
        {
            //TextIndicator.gameObject.transform.position = new Vector3(TextIndicator.gameObject.transform.position.x, TextIndicator.gameObject.transform.position.y + 180, TextIndicator.gameObject.transform.position.z); ;
            TextIndicator.fontSize = endofRoundfontSize;
            LoadingBar.gameObject.SetActive(false);
            TextIndicator.color = Color.white;
            TextIndicator.text = "Round " + roundNum +" over!";
        }
        LoadingBar.GetComponent<Image>().fillAmount = (currentTime) / (startTime);
    //    if (roundNum >= 5)
    //    {
    //        countText.text = "Gameover!!";
    //        StopAllCoroutines();
    //    }
    //    else if (timerLeft <= 0)
    //    {
    //        countText.text = "Round " + roundNum + " Finish!";
    //        // call respawn code here to respawn all players?


    //        StopCoroutine("LoseTime");
    //        StartCoroutine("EndRound");
         


    //        // Call function to initialize the next round here

    //    }
    //    else if (canTime){
    //        countText.text = ("" + timerLeft);
    //    }
    //    else if (preTimer)
    //    {
    //        countText.text = ("" + preGameCounter);
    //    }
    }
}
