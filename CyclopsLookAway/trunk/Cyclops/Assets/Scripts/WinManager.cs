using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour {


//	public GameObject beehive;
//	public GameObject sadPrefab;

	// Use this for initialization
	void Start () {
//			int i = 0;
//			foreach (Transform beechild in beehive.transform) {
//				GameObject part = (GameObject)GameObject.Instantiate (sadPrefab);
//				part.transform.parent = beechild;
//			}

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > 3f) {
			if (Input.GetMouseButton (0)) {
				SceneManager.LoadScene ("Menu");
			}
		}
	}
}
