using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyArrows : MonoBehaviour
{
    private LevelManager levelManager;
    private Transform[] arrows;

    private float maxTravelX;
    private float maxTravelY;

    private int numPlayers;


    public void InitEnemyArrows(LevelManager lm)
    {
        levelManager = lm;
        numPlayers = levelManager.GetNumPlayers();

        Transform arrowContainer = levelManager.uiManager.canvas.transform.GetChild(9);
        arrows = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            arrows[i] = arrowContainer.GetChild(i);
            // Set arrow sprites to match player color and disable unused arrows
            for (int j = 0; j < 3; j++)
            {
                if (j >= numPlayers - 1 || i >= numPlayers)
                {
                    arrows[i].GetChild(j).gameObject.SetActive(false);
              
                }
            }
        }

        if (levelManager.GetNumPlayers() == 2)
        {
            maxTravelX = Screen.width * 0.2f;
            maxTravelY = Screen.height * 0.4f;
        }
    }
    

    public void UpdateArrowPosition()
    {
        List<GameObject> playerList = levelManager.GetPlayerList();
        GameObject[] cameraArray = levelManager.cameraManager.GetCameraArray();
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject player = playerList[i];
            float x;
            float y;
            int arrowIndex = 0;
            for (int j = 0; j < numPlayers; j++)
            {
                if (i == j) continue;
                GameObject otherPlayer = playerList[j];
                SetArrowTransform(player, otherPlayer, i, arrowIndex);
                arrowIndex++;

            }
        }
    }


    private bool SetArrowActive(GameObject p1, GameObject p2, int index, int arrowIndex)
    {
        GameObject arrow = arrows[index].GetChild(arrowIndex).gameObject;
        if (Vector3.Distance(p1.transform.position, p2.transform.position) < 10 && arrow.activeSelf)
        {

            arrow.SetActive(false);
            return true;
        }
        else if (Vector3.Distance(p1.transform.position, p2.transform.position) >= 10 && !arrow.activeSelf)
        {
            arrow.SetActive(true);
        }
        return false;
    }


    private void SetArrowTransform(GameObject p1, GameObject p2, int index, int arrowIndex)
    {
        if (SetArrowActive(p1, p2, index, arrowIndex)) return;
        SetArrowRotation(p1, p2, index, arrowIndex);
    }


    private void SetArrowRotation(GameObject p1, GameObject p2, int index, int arrowIndex)
    {
        Vector2 heading = p2.transform.position - p1.transform.position;
        heading = heading.normalized;
        float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        arrows[index].GetChild(arrowIndex).transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        SetArrowPosition(index, arrowIndex, heading, angle);
    }


    private void SetArrowPosition(int index, int arrowIndex, Vector2 heading, float angle)
    {
        float x = 0;
        float y = 0;
        GameObject camera = levelManager.cameraManager.GetCameraArray()[index];
        Rect cameraRect = camera.GetComponent<Camera>().rect;
        if (numPlayers == 2)
        {
            //Debug.Log(index + ": " + cameraRect);

            x = (Screen.width * (cameraRect.x + 0.25f)) + (maxTravelX * heading.x * 1.25f);
            y = (Screen.height * (cameraRect.y + 0.5f)) + (maxTravelY * heading.y * 1.25f);

            float xMod = 1;
            if (angle > 110f || angle < -110f)
                xMod = -1f;

            if (xMod * x > xMod * (Screen.width * (cameraRect.x + 0.25f)) + maxTravelX)
                x = (Screen.width * (cameraRect.x + 0.25f)) + maxTravelX * xMod;

            float yMod = 1;
            if (angle < -22f && angle > -153f)
                yMod = -1f;

            if (yMod * y > yMod * (Screen.height * (cameraRect.y + 0.5f)) + maxTravelY)
                y = (Screen.height * (cameraRect.y + 0.5f)) + maxTravelY * yMod;
        }
        arrows[index].GetChild(arrowIndex).transform.position = new Vector3(x, y, 0f);
    }
}
