using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonHandler : MonoBehaviour {


	public void StartGame(){
		SceneManager.LoadScene ("Level");
	}

	public void Quit(){
		Application.Quit ();
	}
}
