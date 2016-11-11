using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour {

	public float shake = 0f;
	public float shakeAmount = 0.5f;
	private float decreaseFactor = 1f;
	private Transform[] cams;

	// Use this for initialization
	void Start () {
		cams = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			cams [i] = transform.GetChild (i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (shake > 0) {
			foreach (Transform child in cams) {
				child.localPosition = Random.insideUnitSphere * shakeAmount;
				child.localPosition = new Vector3 (child.localPosition.x, child.localPosition.y, -20f);
			}
			shake -= Time.deltaTime * decreaseFactor;
		} else {
			shake = 0f;
			foreach (Transform child in cams) {
				child.localPosition = new Vector3 (0, 0, -20f);
			}
		}
	}

	public void addShake(float amt){
		shake += amt;
	}
}
