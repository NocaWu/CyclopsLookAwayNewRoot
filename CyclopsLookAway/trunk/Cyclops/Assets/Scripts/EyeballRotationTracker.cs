using UnityEngine;
using System.Collections;

public class EyeballRotationTracker : MonoBehaviour {

	public float minDizzyForce, maxDizzyForce, minRotationstoDizzy, maxRotationstoDizzy;
	public bool isDizzy;


	public Transform cursor;
	public float dizzyFactor = 1f;
	private float totalRotation = 0;
	public int nbrOfRotations {
		get { 
			return ((int) totalRotation) / 90;
		}
	}
	private Vector3 lastPoint;

	// Use this for initialization
	void Start () {
		lastPoint = transform.forward;
		lastPoint.y = 0;
		InvokeRepeating ("reduceRotation", 0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (cursor);

		Vector3 facing = transform.forward;
		facing.y = 0;
		float angle = Vector3.Angle (lastPoint, facing);
		//if(Vector3.Cross(lastPoint, facing)
		totalRotation += (angle * (1f-(dizzyFactor/maxDizzyForce)));
		lastPoint = facing;

		float newdizfactor;
		newdizfactor = ((((float)nbrOfRotations - minRotationstoDizzy) * (maxDizzyForce - minDizzyForce)) / (maxRotationstoDizzy - minRotationstoDizzy));
		newdizfactor = Mathf.Clamp (newdizfactor, minDizzyForce, maxDizzyForce);
		dizzyFactor = newdizfactor;

		if ((dizzyFactor / maxDizzyForce) > 0.2f) {
			isDizzy = true;
		} else {
			isDizzy = false;
		}

	}

	void reduceRotation(){
		totalRotation *= 0.8f;
		if (totalRotation < 0) totalRotation = 0;
	}
}
