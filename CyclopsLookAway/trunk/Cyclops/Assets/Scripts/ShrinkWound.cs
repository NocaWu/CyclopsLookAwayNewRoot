using UnityEngine;
using System.Collections;

public class ShrinkWound : MonoBehaviour {

	GameManager gm;

	float startval;
	float endval;
	float starttime;
	// Use this for initialization
	void Start () {
		startval = transform.localScale.x;
		endval = 0;
		starttime = Time.time + 1.5f;
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gm.hasEnded) {

			float time = (Time.time - starttime) / 6f;
			float newval = Mathf.Lerp (startval, endval, time);
			transform.localScale = new Vector3 (newval, newval, newval);
			if (newval == 0) {
				transform.gameObject.SetActive (false);
			}
		}
	}
}
