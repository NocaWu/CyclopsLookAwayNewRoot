using UnityEngine;
using System.Collections;

public class HitShaker : MonoBehaviour {

	public float shake = 0f;
	public float shakeAmount;
	private float decreaseFactor =1f;
	private Vector3 originalPos;

	// Use this for initialization
	void Start () {
		originalPos = transform.localPosition;

	}

	// Update is called once per frame
	void Update () {
		if (shake > 0) {
			
			transform.localPosition = originalPos + (Random.insideUnitSphere * shakeAmount);
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, originalPos.z);

			shake -= Time.deltaTime * decreaseFactor;
		} else {
			shake = 0f;
			transform.localPosition = new Vector3 (originalPos.x, originalPos.y, originalPos.z);
		}
	}

	public void addShake(float amt){
		shake += amt;
	}
}
