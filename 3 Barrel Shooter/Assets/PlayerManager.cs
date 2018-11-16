using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public GameObject[] playerList;
    public GameObject player1;
    public GameObject player2;

    void Start () {
        playerList[0] = player1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
