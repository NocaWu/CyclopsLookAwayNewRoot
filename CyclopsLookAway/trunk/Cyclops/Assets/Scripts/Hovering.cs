using UnityEngine;
using System.Collections;

public class Hovering : MonoBehaviour {

	Vector3 originsize;

	// Use this for initialization
	void Start () {
		originsize = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		//		print(
		//			//Mathf.Sin (Time.time)
		//			((Mathf.Sin (Time.time)+1f)/2f*(0.4f))+0.8f
		//		);
		float sine = ((Mathf.Sin (Time.time*6f)+0f)/2f*(0.3f))+0.85f;
		transform.localPosition = originsize + new Vector3 (0, sine, 0);
	}
}
