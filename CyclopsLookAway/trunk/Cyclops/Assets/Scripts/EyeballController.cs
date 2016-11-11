using UnityEngine;
using System.Collections;

public class EyeballController : MonoBehaviour {

	//a reference to the cursor object is set using the inspector

	public bool inGame;
	public bool isDizzy = false;

	EyeballRotationTracker rotatrack;
	Animator dizzyAnim;

	public Texture eyeballNormal;
	public Texture eyeballDizzy;

	public Vector3 direc;

	public Transform cursor;
	//these variables are not public thus won't show up in the inspector or be accessible to other classes
	public GameManager game;
	bool gameStarted;

	public AudioSource dangerAudio;
	
	SpriteRenderer sprite;

	public GameObject mouth;
	Animator mouthAnim;

	public AudioSource hurtNoise;
	public AudioClip[] gruntNoises;


	// Use this for initialization
	void Start () {
		//initialize a reference to the sprite renderer
		if (mouth != null) {
			mouthAnim = mouth.GetComponent<Animator> ();
		}
		sprite = GetComponent<SpriteRenderer>();



		if (inGame) {
			rotatrack = GetComponent<EyeballDizzyController> ().rotatracker;
			dizzyAnim = GameObject.Find ("DizzyContainer").GetComponent<Animator> ();
		}

		//move to the center of the world at the start of the game.
		//transform.position = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if(!inGame){
		transform.LookAt (cursor);
		}

		if (inGame) {
			isDizzy = rotatrack.isDizzy;

			if (isDizzy) {
				dizzyAnim.SetBool ("IsDizzy", true);
				GetComponent<Renderer> ().materials [0].SetTexture ("_MainTex", eyeballDizzy);
			} else {
				dizzyAnim.SetBool ("IsDizzy", false);
				GetComponent<Renderer> ().materials [0].SetTexture ("_MainTex", eyeballNormal);
			}

		}

//		transform.forward = Vector3.SlerpUnclamped (transform.forward, cursor.position, 50f * Time.deltaTime);



		//GetComponent<Rigidbody> ().AddTorque (transform.forward - cursor.position);


	}



		

	void HitByBee(){
		mouthAnim.SetTrigger ("Hurt");

		hurtNoise.clip = gruntNoises [Random.Range (0, gruntNoises.Length)];
		hurtNoise.pitch = Random.Range (0.9f, 1.1f);
		hurtNoise.PlayOneShot (hurtNoise.clip);

		game.Stung ();
	}


	void OnDrawGizmos(){
		Gizmos.color = Color.red;

		direc = transform.forward;
		direc.z = 0f;
		direc *= 10;

		Gizmos.DrawRay (transform.position, direc);
	}



}
