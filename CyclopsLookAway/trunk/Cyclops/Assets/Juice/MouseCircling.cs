using UnityEngine;
using System.Collections;

public class MouseCircling : MonoBehaviour {

	public GameObject eyeball;

	public Vector3 axis = Vector3.forward;

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(eyeball.transform.position, axis, Time.deltaTime * speed);

	}
}
