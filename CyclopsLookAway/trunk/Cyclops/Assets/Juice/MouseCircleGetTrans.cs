using UnityEngine;
using System.Collections;

public class MouseCircleGetTrans : MonoBehaviour {
	
	public GameObject mouseCircling;
	public GameObject eyeball;
	public float adjusting;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = eyeball.transform.position + (mouseCircling.transform.position 
			- eyeball.transform.position) * adjusting ;
	
	}
}
