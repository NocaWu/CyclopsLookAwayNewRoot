using UnityEngine;
using System.Collections;

public class ScaleSize : MonoBehaviour {

	Vector3 originsize;

	// Use this for initialization
	void Start () {
		originsize = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
//		print(
//			//Mathf.Sin (Time.time)
//			((Mathf.Sin (Time.time)+1f)/2f*(0.4f))+0.8f
//		);
		transform.localScale = originsize  * (((Mathf.Sin (Time.time*3.5f)+1f)/2f*(0.3f))+0.95f);
	}
}
