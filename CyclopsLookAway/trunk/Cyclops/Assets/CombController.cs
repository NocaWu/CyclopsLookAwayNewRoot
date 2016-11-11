using UnityEngine;
using System.Collections;

public class CombController : MonoBehaviour {

	public GameObject cursor;
	public GameManager gm;
	public GameObject comb;
	public int bitesPerAnim;
	int currentBites = 0;
	int totalBites = 0;

	LiftToMouth liftScript;
	public GameObject mouth;
	Animator mouthAnim;
	AudioSource mouthAud;

	ParticleSystem clickParticle;

	// Use this for initialization
	void Start () {
		mouthAnim = mouth.GetComponent<Animator> ();
		mouthAud = mouth.GetComponent<AudioSource> ();
		liftScript = GetComponent<LiftToMouth> ();
		mouthAud.volume = 0;
		mouthAud.Play ();

		clickParticle = cursor.GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){

		if (!gm.hasEnded) {
			mouthAnim.SetTrigger ("Bite");

			Animator anim = GetComponent<Animator> ();
			if (liftScript.currentstate == "Resting" || liftScript.currentstate == "Lowering") {
				liftScript.Action ("Lift");
			} else if (liftScript.currentstate == "Eating" || liftScript.currentstate == "Lifting") {
				liftScript.Action ("Eat");
			}



			totalBites++;
			currentBites++;
			gm.bitesTaken = totalBites;

			if (currentBites > bitesPerAnim) {
				currentBites = 0;
				advanceBite ();
			}
				
			CancelInvoke ("lowerVol");
			mouthAud.volume = 1f;
			Invoke ("lowerVol", 0.5f);

			clickParticle.Play ();
		}
	}

	void lowerVol(){
		mouthAud.volume = 0f;
	}

	void advanceBite(){
		Animator combanim = comb.GetComponent<Animator> ();
		RuntimeAnimatorController ac = combanim.runtimeAnimatorController;

		combanim.speed = 1f;
		combanim.Play ("Honeycomb", 0, (1 / 8f) * (totalBites / bitesPerAnim));
		liftScript.liftScale = 2f + (2f * (totalBites / (8f * bitesPerAnim)));

		combanim.speed = 0f;
	}



}
