using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DizzyAnimation : MonoBehaviour {

	public Texture[] frames;
	int framenum = 0;

	// Use this for initialization
	void Start () {
		StartCoroutine (eyetest ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator eyetest(){
		while (true){
			if (framenum == 0) {
				GetComponent<Renderer> ().material.SetTexture ("_MainTex", frames [framenum]);
				framenum++;
			} else {
				GetComponent<Renderer> ().material.SetTexture ("_MainTex", frames [framenum]);
				framenum = 0;
			}
			yield return new WaitForSeconds(1);
		}

	}
}
