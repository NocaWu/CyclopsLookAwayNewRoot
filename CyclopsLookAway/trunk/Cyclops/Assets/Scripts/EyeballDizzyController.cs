using UnityEngine;
using System.Collections;

public class EyeballDizzyController : MonoBehaviour {

	Transform cursor;
	public EyeballRotationTracker rotatracker;
	private float dizzyFactor;

	void Start () {
		cursor = GetComponent<EyeballController> ().cursor;
	}
	
	// Update is called once per frame
	void Update () {

		dizzyFactor = rotatracker.dizzyFactor;


		Vector3 x = Vector3.Cross (transform.forward, cursor.position.normalized);
		float theta = Mathf.Asin (x.magnitude);
		Vector3 w = x.normalized * theta / Time.fixedDeltaTime / 2f;
		Quaternion q = transform.rotation * GetComponent<Rigidbody> ().inertiaTensorRotation;
		Vector3 T = q * Vector3.Scale (GetComponent<Rigidbody> ().inertiaTensor, (Quaternion.Inverse (q) * w));
		GetComponent<Rigidbody> ().AddTorque (T, ForceMode.Impulse);
		GetComponent<Rigidbody> ().AddTorque (GetComponent<Rigidbody> ().angularVelocity*dizzyFactor, ForceMode.Acceleration);
		//make 200f above into the dizzy variable!


	}


}
