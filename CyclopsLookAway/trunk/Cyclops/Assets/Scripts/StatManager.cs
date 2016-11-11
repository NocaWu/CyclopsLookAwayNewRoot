using UnityEngine;
using System.Collections;
using System.IO;

public class StatManager : MonoBehaviour {

//	private const char next = '\n';
//
//	StreamWriter swPlayTime;
//
//	float playTimeCounter = 0.0f;
//
//	bool gameStarted;
//	bool hasWon;
//	bool hasLost;
//
//	public GameObject cursor;
//	public GameObject gm;
//
//	GameLogic glGM;
//	GameLOgic glGL;
//
//	Vector3 posCursorOri;
//	Vector3 posCursorNow;
//
	//float distCursorMove;

	// Use this for initialization
	void Start () {
		// having trouble putting it into a folder
//		if (File.Exists ("swPlayTime.txt") == false) {
//			swPlayTime = new StreamWriter("PlayTime", true);
//			swPlayTime.Write ("The time-counts of each game are:" + next);
//			swPlayTime.Close ();
//		} 
//
//		posCursorOri = cursor.transform.position;
//
//		glGM = gm.GetComponent <GameManager>();
//
	}
	
//	// Update is called once per frame
//	void Update () {
//
//		// tell unity what's the condition of "game start"
//		posCursorNow = cursor.transform.position;
//		if (gameStarted == false) {
//			distCursorMove = Vector3.Distance (posCursorOri, posCursorNow);
//		}
//
//		// as soon as the game start, startCounter
//		if (distCursorMove > 0.0f) {
//			gameStarted = true;
//		}
//
//		// when any other scene is called, record playTimeCounter and then reset it
//
//	}

	// function that count how long player is in the
//	void timeCount(){
//		playTimeCounter += Time.deltaTime;
//	}
}
