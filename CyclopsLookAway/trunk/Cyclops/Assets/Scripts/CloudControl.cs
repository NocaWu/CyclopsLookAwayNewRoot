using UnityEngine;
using System.Collections;

public class CloudControl : MonoBehaviour {
	float cloudX;

	// Use this for initialization
	void Start () {
		cloudX = transform.position.x;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (cloudX > -17) {
			cloudX -= 0.05f;
		} else {
			cloudX = 17;
		}

		transform.position = new Vector3 (cloudX, transform.position.y, transform.position.z);

	
	}
}
