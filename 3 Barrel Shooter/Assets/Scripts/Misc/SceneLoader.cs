using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneLoader : MonoBehaviour {
	
	private int dot_count = 1;
	private bool loading = false;

	[SerializeField]
	private int scene;
	[SerializeField]
	private TextMeshProUGUI loadingText;
	[SerializeField]
	private int frame_count_to = 30;
	private int frame_count = 0;


	// Updates once per frame
	void Update() {
		//Starts loading scene if not already loading
		if (loading == false) {
			Debug.Log ("Loading Game");
			StartCoroutine (LoadNewScene ());
			loading = true;
		}

		//Adds dots to the end of text after a set amount of frames to let user know computer is working. After 3 it resets to 1
//		if (frame_count == frame_count_to) {
//			if (dot_count < 3) {
//				loadingText.text += ".";
//				dot_count += 1;
//			} else {
//				loadingText.text = "Loading.";
//				dot_count = 1;
//			}
//		} else if( frame_count > frame_count_to){
//			frame_count = 0;
//		}
//		frame_count += 1;
		

	}


	// The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
	IEnumerator LoadNewScene() {
		//Simulates a load, meant for debugging purposes
		//yield return new WaitForSeconds(3);

		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		AsyncOperation async = Application.LoadLevelAsync(scene);

		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}

	}

}

