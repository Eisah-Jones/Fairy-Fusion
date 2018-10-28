using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChamberInteraction : MonoBehaviour {


    Vacuum v;
    ChamberInteractionModel cim;

	// Use this for initialization
	void Start () {
        v = new Vacuum();
        //ADD FIRE TO THE CHAMBER
        v.AddToChamber(2);
        v.DebugVac();

        //ADD ROCK TO THE CHAMBER
        v.AddToChamber(1);
        v.DebugVac();

        v.AddToChamber(1);
        v.DebugVac();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
