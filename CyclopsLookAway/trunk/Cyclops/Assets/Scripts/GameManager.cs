using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

	public ParticleSystem cursorPart;

	public AudioClip[] stingnoises;
	public AudioClip[] winGrunts;
	public AudioClip[] loseGrunts;

	public GameObject woundPrefab;
	public Texture[] woundSteps;

	public int stingsBeforeLose;
	public float stingCooldown;
	float timeStung;
	public int stings;

	public CombController comb;
	public int bitesTaken { 
		get { return bites; } 
		set { 
			bites = value; /*print (bites);*/ 
			if (bites > (8 * comb.bitesPerAnim)) {
				if (!hasEnded) {
					WinGame ();
				}
			}
		} 
	}
	private int bites;
	public int bitesPerAnim;
	bool halfwaydone = false;

	private const char next = '\n';
	private const char delim = '|';

	StreamWriter swGameRecorder;
	StreamReader srPlayerCount;

	float playTimeCounter = 0.0f;
	float endingTest;

	public string gameEnding;
	private string PATH;

	public bool hasEnded = false;

	int playerNum;

	private BeeMaker beemakr;

	public GameObject eyeball;

	float timeGameEnded;
	AsyncOperation async;
	// Use this for initialization

	string message = "";

	void Start () {
		cursorPart.Play ();
		StopAllCoroutines ();
		//SceneManager.UnloadScene ("Menu");
		bitesPerAnim = comb.bitesPerAnim;
		beemakr = GameObject.Find ("BeeHive").GetComponent<BeeMaker> ();

		MusicManagerNew.instance.SongFadeIn ("Music_Layer_01", 1f);
		MusicManagerNew.instance.PlaySong ("Music_Layer_02");
		MusicManagerNew.instance.SetSongVolume ("Music_Layer_02", 0);

		//eyeball.GetComponent<MeshRenderer> ().materials [1].SetFloat ("_Cutoff", 1f- ((float)stings / stingsBeforeLose) + 0.01f);
		eyeball.GetComponent<MeshRenderer> ().materials [1].SetTexture ("_MainTex", woundSteps[0]);

		StartCoroutine("loadMenuScreen");
	}



	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		playTimeCounter += Time.deltaTime;
		if(!halfwaydone){
			if ((float)bites / (8f * (float)comb.bitesPerAnim) > 0.45f) {
				halfwaydone = true;
				MusicManagerNew.instance.SongFadeInVolOnly ("Music_Layer_02", 1f);
			}
		}


//		if (hasEnded && Time.time - timeGameEnded > 2f) {
//			if (Input.GetMouseButton (0)) {
//				async.allowSceneActivation = true;
//			}
//		}

	}

	IEnumerator loadMenuScreen() {
		async = Application.LoadLevelAsync("Menu");
		async.allowSceneActivation = false;
		yield return async;
	}

	public void ReturnTomenu(){
		
		async.allowSceneActivation = true;
		//SceneManager.LoadScene ("Menu");
	}

//	void OnGUI()
//	{
//			GUI.Box (new Rect (0, 0, 200, 100), message);
//
//
//	}

	void WinGame(){
		gameEnding = "win";
		EndMusic ();



		if (!hasEnded) {
			transform.Find ("ScoreShow").gameObject.SetActive (true);
//
//		GameObject.Find ("ScoreShow").SetActive (true);
			transform.Find ("EatTime").gameObject.SetActive (true);
			transform.Find ("ScoreShow").gameObject.GetComponent<TextMesh> ().text = Mathf.RoundToInt (playTimeCounter).ToString () + " seconds!";
			//playTimeCounter

			GameObject.Find ("WinText").GetComponent<Animator> ().SetBool ("hasWon", true);

			RecordTime ();
		}

		GameEndProcess (true);

		//Playsound?
//		if (hasEnded == false) {
//			
//			RecordTime ();
//			hasEnded = true;
//			SceneManager.LoadSceneAsync ("Winning");
//		}

	}

	void LoseGame(){
		gameEnding = "lose";
		EndMusic ();
		GameEndProcess (false);
		RecordTime ();
		transform.Find ("tearsParticleLeft").gameObject.SetActive (true);
		transform.Find ("tearsParticleRight").gameObject.SetActive (true);

		GameObject.Find ("LoseText").GetComponent<Animator> ().SetBool ("hasLose", true);
//		if (hasEnded == false) {
//			
//			RecordTime ();
//			hasEnded = true;
//			SceneManager.LoadSceneAsync ("Losing");
//		}
		//
	}

	void EndMusic(){
		MusicManagerNew.instance.SongFadeOut ("Music_Layer_01", 1f);
		MusicManagerNew.instance.SongFadeOut ("Music_Layer_02", 1f);
		if (gameEnding == "lose") {
			MusicManagerNew.instance.SongFadeIn ("Music_GameOver", 1f);

		} else {
			MusicManagerNew.instance.SongFadeIn ("Music_Victory", 1f);
		}
	}


	void GameEndProcess(bool gameWon){



		//GameObject.Find ("MusicManager").GetComponent<MusicManager> ().GameOver ();


		hasEnded = true;
		GameObject.Find ("BeeHive").GetComponent<BeeAttackManager> ().endGame ();
		List<GameObject> bees;
		bees = beemakr.bees;
		foreach (GameObject bee in bees) {
			bee.GetComponent<BeeController> ().AttackMode (false);
			if (gameWon) {
				bee.transform.Find ("Emoji").gameObject.SetActive (true);
			} else {
				bee.transform.Find ("Emoji Sad").gameObject.SetActive (true);
			}
		}
		timeGameEnded = Time.time;
		Invoke("playEndNoise",1.3f);

		GameObject.Find ("AgainHolder").GetComponent<Animator> ().SetTrigger ("hasEnd");
		// pos x 616

	}

	void playEndNoise(){
		string gamestate = gameEnding;

		AudioSource host;
		if (gamestate == "win") {
			host = transform.Find ("WinSound").GetComponent<AudioSource> ();
			host.clip =  winGrunts[Random.Range (0, winGrunts.Length)];
		} else {
			host = transform.Find ("LoseSound").GetComponent<AudioSource> ();
			host.clip =  loseGrunts[Random.Range (0, loseGrunts.Length)];
		}
		host.Play ();


	}


	public void Stung(){
		if (!hasEnded) {
			stings++;
			GameObject.Find ("Cameras").GetComponent<Screenshake> ().addShake (0.35f * ((float)stings / stingsBeforeLose));

			GameObject.Find ("Head").GetComponent<HitShaker> ().addShake (0.2f);
			GameObject.Find ("Body").GetComponent<HitShaker> ().addShake (0.2f);
			GameObject.Find ("MouthAni").GetComponent<HitShaker> ().addShake (0.2f);

			//eyeball.GetComponent<MeshRenderer> ().materials [1].SetFloat ("_Cutoff", 1f - ((float)stings / stingsBeforeLose) + 0.01f);
			eyeball.GetComponent<MeshRenderer> ().materials [1].SetTexture ("_MainTex", woundSteps[stings]);
			if (stings >= stingsBeforeLose) {
				LoseGame ();
			}
			Invoke ("removeSting", stingCooldown);


			AudioSource eyesting = transform.Find ("EyeballStab").GetComponent<AudioSource> ();
			eyesting.clip = stingnoises [Random.Range (0, stingnoises.Length)];
			eyesting.pitch = Random.Range (0.9f, 1.1f);
			eyesting.PlayOneShot (eyesting.clip);

		}
	}

	void removeSting(){
		if (!hasEnded) {
			stings--;
			//eyeball.GetComponent<MeshRenderer> ().materials [1].SetFloat ("_Cutoff", 1f - ((float)stings / stingsBeforeLose) + 0.01f);
			eyeball.GetComponent<MeshRenderer> ().materials [1].SetTexture ("_MainTex", woundSteps[stings]);
		}
	}

	public void addWoundToEye(Vector3 woundLoc){
		GameObject woundTemp;
		woundTemp = (GameObject)GameObject.Instantiate (woundPrefab);
		woundTemp.transform.parent = eyeball.transform;
		woundTemp.transform.position = woundLoc;
	}


	void beeSpeedUp(){
		
	}

	void RecordTime(){
//		StreamWriter record = new StreamWriter(PATH + "/GameRecord.txt", true);
//		record.WriteLine ("Player"+ playerNum + delim 
//						+ Mathf.RoundToInt(playTimeCounter).ToString() + delim 
//						+ bitesTaken.ToString() + delim 
//						+ gameEnding);
//		record.Close ();



		Analytics.CustomEvent("gameOver", new Dictionary<string, object>
			{
				{ "playTime",  Mathf.RoundToInt(playTimeCounter)},
				{ "bitesTaken", bitesTaken.ToString() },
				{ "gameState", gameEnding }
			});
		playTimeCounter = 0.0f;
	}

	public void playButtonNoise(){
		MusicManagerNew.instance.PlaySound ("Button");
	}
}
