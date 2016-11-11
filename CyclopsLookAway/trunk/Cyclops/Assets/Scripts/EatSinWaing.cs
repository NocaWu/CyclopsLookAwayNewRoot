using UnityEngine;
using System.Collections;

public class EatSinWaing : MonoBehaviour {
	Vector3 originalscale;
	// Use this for initialization
	void Start () {
		originalscale = transform.localScale;
	}

	// Update is called once per frame
	void Update () {
		transform.localScale = originalscale * (Mathf.Abs(Mathf.Sin (4*Time.time))* 0.1f+1);
	
	}
}
