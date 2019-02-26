using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject cameraPrefab;
	private GameObject playerUIPrefab;
    private GameObject[] cameras;

    private int numCameras;
    private int currentCamIndex;

    private float zPos;

    public void InitCameraManager(int numPlayers)
    {
        cameraPrefab = Resources.Load<GameObject>("Camera/GameCam");
		playerUIPrefab = Resources.Load<GameObject> ("UI/PlayerUI");
        cameras = new GameObject[numPlayers];

        numCameras = numPlayers;
        currentCamIndex = 0;

        zPos = -12f;
    }

    public void SetCameraRatio()
    {
        if (numCameras == 2)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
        }
        else if (numCameras == 3)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0.25f, 0.0f, 0.5f, 0.5f);
        }
        else if (numCameras == 4)
        {
            cameras[0].GetComponent<Camera>().rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
            cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            cameras[2].GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
            cameras[3].GetComponent<Camera>().rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
        }
    }

    public void UpdateCameraPosition(List<GameObject> players)
    {
        for (int i = 0; i < numCameras; i++)
        {
            GameObject c = cameras[i];
            Vector3 playerPos = players[i].transform.position;
            c.transform.position = new Vector3(playerPos.x, playerPos.y, zPos);
        }
    }

    public void AddCamera()
    {
        GameObject cameraObject = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
		GameObject canvasObject = Instantiate (playerUIPrefab, Vector3.zero, Quaternion.identity);
		Canvas canvas = canvasObject.GetComponent<Canvas> ();
        cameraObject.name = "Cam" + (currentCamIndex + 1).ToString();
		canvasObject.name = "PlayerUI" + (currentCamIndex + 1).ToString ();
        cameras[currentCamIndex++] = cameraObject;
		canvas.worldCamera = cameraObject.GetComponent<Camera> ();
    }

    
    public GameObject[] GetCameraArray()
    {
        return cameras;
    }
}
