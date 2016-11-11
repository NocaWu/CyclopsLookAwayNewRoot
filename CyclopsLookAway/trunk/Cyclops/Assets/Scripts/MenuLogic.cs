using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MenuLogic : MonoBehaviour {
	AsyncOperation async;
	string gamescene;
	// Use this for initialization

	void Awake(){
//		if (Application.isWebPlayer) {
//			Screen.fullScreen = true;
//		}
	}

	void Start () {
		//			inGame = true;
		//			noAttackZone = GameObject.Find ("NoAttackZone").gameObject.GetComponent<Collider> ();
		gamescene = SceneManager.GetActiveScene ().name;

		if (gamescene == "Menu") {
			MusicManagerNew.instance.PlaySong ("BeeAmbience");
			MusicManagerNew.instance.SongFadeIn ("Music_Opening_Theme", 1f);
			StartCoroutine("loadControlScreen");
		} else if (gamescene == "Controls") {
			StartCoroutine("loadGameScreen");
		}

	}
		

	IEnumerator loadControlScreen() {
		async = Application.LoadLevelAsync("Controls");
		async.allowSceneActivation = false;
		yield return async;
	}
	IEnumerator loadGameScreen() {
		async = Application.LoadLevelAsync("Game");
		async.allowSceneActivation = false;
		yield return async;
	}



	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
//		if (gamescene == "Controls") {
//			if (Input.GetMouseButtonDown(0)) {
//				async.allowSceneActivation = true;
//			}
//		}
	}

	public void playButtonNoise(){
		MusicManagerNew.instance.PlaySound ("Button");
	}

	public void LoadScene(string sc){
		if (sc == "Controls") {
			
			async.allowSceneActivation = true;
		}
		if (sc == "Game") {
			MusicManagerNew.instance.SongFadeOut ("Music_Opening_Theme", 1f);
			async.allowSceneActivation = true;
		}
		//UTIL_SceneLoader.loadScene (sc);
	}




}
