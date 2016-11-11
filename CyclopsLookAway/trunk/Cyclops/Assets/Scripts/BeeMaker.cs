using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeMaker : MonoBehaviour {

	public bool forMenu;

	public int audio_detail = 500;
	public float audio_minValue = 1.0f;
	public float audio_amplitude = 0.1f;
	public float audio_speedMult = 1f;

	public int maxbeeCount;
	public List<GameObject> bees;
	public Vector2 swarmBounds;//= new Vector2(300f, 300f);
	public float swarmRadius = 100f;
	public GameObject prefab;
	public float beeDelay;

	private bool leftright = true;

	// Use this for initialization
	void Start () {
		bees = new List<GameObject> ();
		if (forMenu) {
			int i = 0;
			foreach (Transform beechild in transform) {
				BeeController bc = beechild.GetComponent<BeeController> ();
				bc.bees = this.bees;
				bc.beemaker = this;
				bc.canAttack = false;
				bc.currentNode = i;
				i += 3;
				beechild.GetComponent<BeeController> ().pathDirection = leftright ? "Left" : "Right";
				leftright = !leftright;
				bees.Add (beechild.gameObject);
			}
		} else {
			Vector3[] pathnodes;
			Path beepath = this.GetComponent<BeePath> ().beeroute;
			pathnodes = beepath.getNodes ();

			for (int j = 0; j < pathnodes.Length; j += 4) {
				generateBee(pathnodes[j], j);
			}


			StartCoroutine (MakeBee ());
			StartCoroutine (SpeedUpBeesOverTime());
		}
	}


	// Update is called once per frame
	void Update () {
		float[] info = new float[audio_detail];
		AudioListener.GetOutputData(info, 0); 
		float packagedData = 0;

		for(int x = 0; x < info.Length; x++)
		{
			packagedData += System.Math.Abs(info[x]);   
		}
		audio_speedMult = (packagedData * audio_amplitude);
	}

	IEnumerator SpeedUpBeesOverTime(){
		beeDelay *= 0.96f;
		beeDelay = Mathf.Clamp (beeDelay, 0.1f, 10f);
		yield return new WaitForSeconds (2f);
	}
		
	IEnumerator MakeBee(){
		// instantiate the drones

		while (true) {

			if (this.bees.Count < maxbeeCount) {
				generateBee( new Vector2 (transform.position.x, transform.position.y + 10f) + Random.insideUnitCircle * 1f);
			}

			yield return new WaitForSeconds (beeDelay);

		}
	}

	void generateBee(Vector3 pos, int node = default(int)){
		GameObject droneTemp;

		droneTemp = (GameObject)GameObject.Instantiate (prefab);
		BeeController bc = droneTemp.GetComponent<BeeController> ();
		bc.bees = this.bees;
		bc.beemaker = this;
		bc.attackmngr = GetComponent<BeeAttackManager> ();
		bc.currentNode = node;

		droneTemp.transform.position = new Vector3 (pos.x, pos.y, 0);
		droneTemp.transform.parent = transform;
		droneTemp.transform.localScale *= Random.Range (0.9f, 1.1f);

		droneTemp.GetComponent<BeeController> ().pathDirection = leftright ? "Left" : "Right";
		leftright = !leftright;

		bees.Add (droneTemp);
	}



	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(swarmBounds.x, swarmBounds.y, 0));
		Gizmos.DrawWireSphere(transform.position, swarmRadius);
	}
}
