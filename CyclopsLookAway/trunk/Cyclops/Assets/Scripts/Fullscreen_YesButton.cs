using UnityEngine;
using System.Collections;

public class Fullscreen_YesButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Invoke ("Gotime", 0f);
	}

	void Gotime(){

		Screen.fullScreen = true;

		GameObject.Find ("GameManager").GetComponent<FullscreenMenu> ().LoadScene ("Menu");
	}
}
