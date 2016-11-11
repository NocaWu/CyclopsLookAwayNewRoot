using UnityEngine;
using System.Collections;

public class EyeHit : MonoBehaviour {

	public bool changeSize;
	public AnimationCurve sizeCurve;
	float scale;

	public bool changeColor;
	public Color colorHit;
	public float colorTime;
	private Material mat;
	private Color orig;

	float timeBegin;

	public bool changeTransparency;
	int maxHits;
	int curHits;

	// Use this for initialization
	void Start () {
		scale = transform.localScale.x;
		if (changeColor) {
			mat = GetComponent<MeshRenderer> ().material;
			orig = mat.GetColor ("_Color");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void HitByBee(){
		timeBegin = Time.time;

		if(changeSize) StartCoroutine (sizeAnim ());
		if (changeColor) StartCoroutine (colorAnim ());
	}
		

	IEnumerator sizeAnim(){

		for (float t = 0; t <= 1f; t += Time.deltaTime){
			Vector3 target = (Vector3.one * scale) * sizeCurve.Evaluate (Time.time - timeBegin);

			transform.localScale = target;


			yield return null;
		}
	}

	IEnumerator colorAnim(){
		


		for (float t = 0; t <= colorTime/2f; t += Time.deltaTime){
			mat.color = Color.Lerp (orig, colorHit, t / colorTime/2f);
			yield return null;
		}
		for (float t = 0; t <= colorTime/2f; t += Time.deltaTime){
			mat.color = Color.Lerp (colorHit, orig, t / colorTime/2f);
			yield return null;
		}
		mat.color = orig;


	}
}
