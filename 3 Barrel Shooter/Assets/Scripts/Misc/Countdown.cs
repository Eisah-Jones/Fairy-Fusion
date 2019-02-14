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
    public Text countText;
    private bool canTime;
    private bool preTimer = true;
    public int preGameCounter = 3;
    public int roundNum = 1;

    public void incrementRound()
    {
        roundNum++;
    }


    public void InitStart()
    {
        timerLeft = startTime;
        countText = GameObject.FindGameObjectWithTag("CountText").GetComponent<Text>();
    }


    public void startCountDown()
    {

       // Time.timeScale = 1f;
        canTime = true;
        StartCoroutine("LoseTime");
    }


    public void startPreCountDown()
    {
        //canTime = true;
        StartCoroutine("PreCount");
    }


    public void pause()
    {
        canTime = false;
    }
    public void resetTimer()
    {
        canTime = false;
        timerLeft = startTime;

    }
    public void setTimer(int n)
    {
        startTime = n;
        timerLeft = n;
    }


    IEnumerator LoseTime()
    {
      
        while (canTime)
        {
            yield return new WaitForSeconds(1);
            timerLeft--;
        }
    }


    IEnumerator PreCount()
    {

        while (true)
        {
          
            yield return new WaitForSeconds(1);
            preGameCounter--;
            if (preGameCounter <= 0)
            {
                
                countText.text = "FIGHT!";
                yield return new WaitForSeconds(1);
                StopCoroutine("PreCount");
                startCountDown();
            }
        }
    }


    IEnumerator EndRound()
    {
        while (true)
        {
            //Time.timeScale = 0;
            yield return new WaitForSeconds(roundEndLength);

            

            incrementRound();
            
            StopCoroutine("EndRound");
            resetTimer();
            startCountDown();


        }
    }


    // Update is called once per frame
    void Update()
    {
        if (roundNum >= 5)
        {
            countText.text = "Gameover!!";
            StopAllCoroutines();
        }
        else if (timerLeft <= 0)
        {
            countText.text = "Round " + roundNum + " Finish!";
            // call respawn code here to respawn all players?


            StopCoroutine("LoseTime");
            StartCoroutine("EndRound");
         


            // Call function to initialize the next round here

        }
        else if (canTime){
            countText.text = ("" + timerLeft);
        }
        else if (preTimer)
        {
            countText.text = ("" + preGameCounter);
        }
    }
}
