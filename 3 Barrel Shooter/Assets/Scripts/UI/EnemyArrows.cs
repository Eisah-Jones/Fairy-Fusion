using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyArrows
{
    private UIManager uiManager;
    private Transform[] arrows;

    private float maxTravelX;
    private float maxTravelY;

    private int numPlayers;


    public void InitEnemyArrows(UIManager um)
    {
        uiManager = um;
        numPlayers = um.levelManager.GetNumPlayers();

        Transform arrowContainer = uiManager.canvas.transform.GetChild(9);
        //Debug.Log(arrowContainer);
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

        if (numPlayers == 2)
        {
            maxTravelX = Screen.width * 0.2f;
            maxTravelY = Screen.height * 0.4f;
        }
        else
        {
            maxTravelX = Screen.width * 0.2f;
            maxTravelY = Screen.height * 0.2f;
        }
    }
    

    public void UpdateArrowPosition()
    {
        List<GameObject> playerList = uiManager.levelManager.GetPlayerList();
        Debug.Log(uiManager.levelManager.cameraManager);
        GameObject[] cameraArray = uiManager.levelManager.cameraManager.GetCameraArray();
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject player = playerList[i];
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

        SetArrowPosition(index, arrowIndex, heading);
    }


    private void SetArrowPosition(int index, int arrowIndex, Vector2 heading)
    {
        float x = 0;
        float y = 0;

        float centerX = 0.5f;
        float centerY = 0.5f;
        float bottomLeftX = 0f;
        float bottomLeftY = 0f;
        float topRightX = Screen.width;
        float topRightY = Screen.height;

        GameObject camera = uiManager.levelManager.cameraManager.GetCameraArray()[index];
        Rect cameraRect = camera.GetComponent<Camera>().rect;

        if (numPlayers == 2)
        {
            centerX = 0.25f;
            centerY = 0.5f;

            bottomLeftX = (Screen.width * (cameraRect.x + 0.04f));
            bottomLeftY = (Screen.height * (cameraRect.y + 0.0975f));

            topRightX = (Screen.width * (cameraRect.x + 0.46f));
            topRightY = (Screen.height * (cameraRect.y + 0.905f));
        }
        else
        {
            centerX = 0.25f;
            centerY = 0.25f;

            bottomLeftX = (Screen.width * (cameraRect.x + 0.044f));
            bottomLeftY = (Screen.height * (cameraRect.y + 0.0725f));

            topRightX = (Screen.width * (cameraRect.x + 0.45f));
            topRightY = (Screen.height * (cameraRect.y + 0.425f));
        }

        x = (Screen.width * (cameraRect.x + centerX)) + (maxTravelX * heading.x * 1.25f);
        y = (Screen.height * (cameraRect.y + centerY)) + (maxTravelY * heading.y * 1.25f);

        if (x < bottomLeftX) x = bottomLeftX;
        else if (x > topRightX) x = topRightX;

        if (y < bottomLeftY) y = bottomLeftY;
        else if (y > topRightY) y = topRightY;

        arrows[index].GetChild(arrowIndex).transform.position = new Vector3(x, y, 0f);
    }
}
