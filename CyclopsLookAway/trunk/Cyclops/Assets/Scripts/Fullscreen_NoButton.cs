using UnityEngine;
using System.Collections;

public class Fullscreen_NoButton : MonoBehaviour {

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
		GameObject.Find ("GameManager").GetComponent<FullscreenMenu> ().LoadScene ("Menu");
	}

}
