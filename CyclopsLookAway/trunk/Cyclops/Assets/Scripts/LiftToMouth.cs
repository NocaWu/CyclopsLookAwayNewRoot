using UnityEngine;
using System.Collections;

public class LiftToMouth : MonoBehaviour {
	
	public float liftScale;
	public AnimationCurve liftCurve;
	public AnimationCurve eatCurve;
	public AnimationCurve downCurve;
	[HideInInspector] public AnimationCurve activeCurve;
	float timeBegin;
	private Vector3 originalpos;
	public string currentstate = "Resting";
	public float lowerDelay = 0.2f;
	float timeToLower;

	// Use this for initialization
	void Start () {
		currentstate = "Resting";
		originalpos = transform.position;
		timeToLower = lowerDelay;
	}
	
	// Update is called once per frame
	void Update () {

		if (currentstate != "Resting") {
			timeToLower -= Time.deltaTime;
			if (timeToLower <= 0) {
				Action ("Down");
			}
		}
	}

	public void Action(string curv_nm){
		if (curv_nm == "Lift") {
			if (currentstate == "Resting") {
				activeCurve = liftCurve;
				currentstate = "Lifting";
				timeToLower = lowerDelay;
				AnimAct();
			}
		} else if (curv_nm == "Eat") {
			if (currentstate == "Eating" || currentstate == "Lifting") {
				activeCurve = eatCurve;
				currentstate = "Eating";
				timeToLower = lowerDelay;
				AnimAct();
			}
		} else if (curv_nm == "Down") {
			if (currentstate != "Lowering") {
				activeCurve = downCurve;
				currentstate = "Resting";
				AnimAct();
			}
		}
	}

	void AnimAct(){
		timeBegin = Time.time;

		//if (currentstate == "Resting") {
			StopAllCoroutines ();
			StartCoroutine (sizeAnim ());
		//}

	}

	IEnumerator sizeAnim(){

		for (float t = 0; t <= 1f; t += Time.deltaTime){
			Vector3 target = (Vector3.up * liftScale) * activeCurve.Evaluate (Time.time - timeBegin);

			this.transform.position = originalpos + target;

		
			if (currentstate == "Lowering" && t >= 0.98f) {
				currentstate = "Resting";
			}


			yield return null;
		}

//		this.transform.position = originalpos;
	}
}
