using UnityEngine;
using System.Collections;

public class BackgroundSmallMoving : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//	gameObject.transform.Rotate = new Vector3(0, 0, Mathf.Abs (Mathf.Sin(Time.time)));
	
		foreach (Transform child in transform) 
		{
			child.Rotate(0, 0, Mathf.Sin(Time.time)* 0.02f);
		}
	}
}
