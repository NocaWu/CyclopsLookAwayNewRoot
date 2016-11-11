using UnityEngine;
using System.Collections;

public class FullscreenMenu : MonoBehaviour {
	
	AsyncOperation async;
	// Use this for initialization
	void Start () {
		StartCoroutine("loadMenuScreen");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void LoadScene(string sc){
		if (sc == "Menu") {
			async.allowSceneActivation = true;
		}
	}


	IEnumerator loadMenuScreen() {
		async = Application.LoadLevelAsync("Menu");
		async.allowSceneActivation = false;
		yield return async;
	}

}
