using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeAttackManager : MonoBehaviour {


	bool gameIsOver = false;

	public float beeCheckUpperBound;
	public float beeCheckLowerBound;

	BeeMaker bm;
	List<GameObject> bees;
	GameManager gm;

	float combPercent;

	public int maxAttackBeeCount;
	int numAttackingBees = 0;

	float timer = 0;

	// Use this for initialization
	void Start () {
		bm = GetComponent<BeeMaker> ();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		bees = bm.bees;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.timeSinceLevelLoad > 2.25f || gm.bitesTaken > 0) && !gameIsOver) {
			timer += Time.deltaTime;
			combPercent = (gm.bitesTaken / (8f * gm.bitesPerAnim));
			if (numAttackingBees < Mathf.Clamp (combPercent * (float)maxAttackBeeCount, 1f, 5f)) {
				if (timer > 0) {
					bees [Random.Range (0, bees.Count - 1)].GetComponent<BeeController> ().AngerBee ();
					timer = 0 - Random.Range (0, beeCheckUpperBound - ((beeCheckUpperBound - beeCheckLowerBound) * combPercent));
				}
			}
		}
	}

	public void ReportInAttack(bool attack){
		if (attack) {
			numAttackingBees++;
		} else {
			numAttackingBees--;
		}
		numAttackingBees = Mathf.Clamp (numAttackingBees, 0, maxAttackBeeCount * 2);
	}

	public void endGame(){
		gameIsOver = true;
	}
}
