using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BeeController : MonoBehaviour {

	bool inGame = false;
	private Collider noAttackZone;
	private float audioSpeedMult = 0f;

	// the overall speed of the simulation
	public float speed;
	// max speed any particular drone can move at
	public float maxSpeed;
	// maximum steering power
	public float maxSteer;
	public float maxSeeAhead;

	// weights: used to modify the drone's movement
	public float separationWeight;
	public float alignmentWeight;
	public float cohesionWeight;
	public float boundsWeight;
	public float objavoidWeight;
	public float pathWeight;
	public float attackWeight;

	public float neighborRadius;
	public float desiredSeparation ;

	// velocity influences
	public Vector3 _separation;
	public Vector3 _alignment;
	public Vector3 _cohesion;
	public Vector3 _bounds;
	public Vector3 _objavoidance;
	public Vector3 _pathfollow;
	public Vector3 _attackvector;

	// other members of my swarm
	public List<GameObject> bees;
	public BeeMaker beemaker;
	public BeeAttackManager attackmngr;

	public string pathDirection;
	public Path beepath;
	public int currentNode = 0;
	public Vector3[] pathnodes;

	Rigidbody rb;
	bool isAttacking = false;
	public bool canAttack = true;

	Vector3 lastvelocity;

	AudioSource audio;

	GameManager gm;
	AudioSource gmSound;

	SpriteRenderer rend;
	public Color angryColor;

	// Use this for initialization
	void Start () {


		if (SceneManager.GetActiveScene ().name == "Game") {
			inGame = true;
			noAttackZone = GameObject.Find ("NoAttackZone").gameObject.GetComponent<Collider> ();
			if(GameObject.Find("GameManager") != null){
				gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
				gmSound = gm.GetComponent<AudioSource> ();
			}
		}
			

		rend = GetComponent<SpriteRenderer> ();

		GetComponent<BoxCollider> ().enabled = false;
		rb = GetComponent<Rigidbody> ();
		beepath = this.transform.parent.GetComponent<BeePath> ().beeroute;
		pathnodes = beepath.getNodes ();
		audio = GetComponent <AudioSource> ();
		audio.volume = 0;
		audio.Play ();
//		if (canAttack) {
//			currentNode = pathDirection == "Left" ? 0 : pathnodes.Length - 1;
//		}
	}


	void FixedUpdate()
	{
		this.bees = beemaker.bees;
		lastvelocity = rb.velocity;

		AttackProcess ();



		audioSpeedMult = Mathf.Pow((1f + beemaker.audio_speedMult),1.2f);
		audioSpeedMult = Mathf.Clamp (audioSpeedMult, 1f, 30f);


		Flock();



		if (isAttacking) {
			Vector3 eyetarget = Vector3.zero;
			float volume = 1f - Mathf.Clamp ((Vector3.Distance (transform.position, eyetarget) / 8f), 0, 1f);
			audio.volume = volume;
		} else {
			audio.volume = 0f;
		}

	}

	private void AttackProcess(){
		if (isAttacking) {
			if (canAttack) {
				GameObject eyeball = GameObject.Find ("Eyeball");
				Vector3 direc;
				direc = eyeball.transform.forward;
				direc.z = 0;
				//print (Vector3.Dot ((transform.position - eyeball.transform.position).normalized, direc.normalized));

				if (!eyeball.GetComponent<EyeballController> ().isDizzy) {
					if (Vector3.Distance (transform.position, eyeball.transform.position) < 10f) {
						if (Vector3.Dot ((transform.position - eyeball.transform.position).normalized, direc.normalized) > 0.9f) {
							AttackMode (false);
							gmSound.PlayOneShot (gmSound.clip);
							GetComponent<BoxCollider> ().enabled = false;
							transform.FindChild ("BeingLooked").GetComponent<ParticleSystem> ().Play ();
						}
					}
				}
			}
		}
	}

	public bool AngerBee(){
		if (!isAttacking) {
			if (canAttack && inGame) {
				if (!noAttackZone.bounds.Contains (transform.position)) {
					AttackMode (true);
					return true;
				} else {return false;}
			} else {return false;}
		} else {return false;}
	}

	public void AttackMode(bool attackon){
		attackmngr.ReportInAttack (attackon);
		if (attackon) {
			isAttacking = true;
			maxSpeed *= 1.5f;
			maxSteer *= 1.5f;
			GetComponent<BoxCollider> ().enabled = true;
			rend.color = angryColor;
		} else {
			isAttacking = false;
			maxSpeed *= (1f/1.5f);
			maxSteer *= (1f/1.5f);
			GetComponent<BoxCollider> ().enabled = false;
			rend.color = Color.white;
		}
	}

	public virtual void Flock()
	{


		Vector3 newVelocity = Vector3.zero;

		CalculateVelocities();

		//transform.forward = _alignment;
		newVelocity += _separation * separationWeight;
		newVelocity += _objavoidance * objavoidWeight;
		if (!isAttacking) {
			newVelocity += _alignment * alignmentWeight;
			newVelocity += _cohesion * cohesionWeight;
			newVelocity += _bounds * boundsWeight;
			newVelocity += _pathfollow * pathWeight;
		} else {
			newVelocity = _attackvector * attackWeight;
		}
		newVelocity = newVelocity * speed * (audioSpeedMult);
		newVelocity = rb.velocity + newVelocity;
		newVelocity.z = 0f;

		rb.velocity = Limit(newVelocity, maxSpeed* (audioSpeedMult));
		//Make the bee point in direction its going:
		transform.up = Vector3.Lerp (transform.up, newVelocity.normalized, Time.fixedDeltaTime * 25f);

//		if ((lastvelocity - newVelocity).magnitude > 1f) {
//			print ((lastvelocity - newVelocity).magnitude);
//		}
	}

	/// <summary>
	/// Calculates the influence velocities for the drone. We do this in one big loop for efficiency.
	/// </summary>
	protected virtual void CalculateVelocities()
	{
		// the general procedure is that we add up velocities based on the neighbors in our radius for a particular influence (cohesion, separation, etc.) 
		// and divide the sum by the total number of bees in our neighbor radius
		// this produces an evened-out velocity that is aligned with its neighbors to apply to the target drone		
		Vector3 separationSum = Vector3.zero;
		Vector3 alignmentSum = Vector3.zero;
		Vector3 cohesionSum = Vector3.zero;
		Vector3 boundsSum = Vector3.zero;
		Vector3 ahead = Vector3.zero;
		Vector3 ahead2 = Vector3.zero;
		Vector3 eyetarget = new Vector3 (0, 0, 0);

		int separationCount = 0;
		int alignmentCount = 0;
		int cohesionCount = 0;
		int boundsCount = 0;

		for (int i = 0; i < this.bees.Count; i++)
		{
			if (bees[i] == null) continue;

			float distance = Vector3.Distance(transform.position, bees[i].transform.position);

			// separation
			// calculate separation influence velocity for this drone, based on its preference to keep distance between itself and neighboring bees
			if (distance > 0 && distance < desiredSeparation)
			{
				// calculate vector headed away from myself
				Vector3 direction = transform.position - bees[i].transform.position;	
				direction.Normalize();
				direction = direction / distance; // weight by distance
				separationSum += direction;
				separationCount++;
			}

			// alignment & cohesion
			// calculate alignment influence vector for this drone, based on its preference to be aligned with neighboring bees
			// calculate cohesion influence vector for this drone, based on its preference to be close to neighboring bees
			if (distance > 0 && distance < neighborRadius)
			{
				alignmentSum += bees[i].GetComponent<Rigidbody>().velocity;
				alignmentCount++;

				cohesionSum += bees[i].transform.position;
				cohesionCount++;
			}

			// bounds
			// calculate the bounds influence vector for this drone, based on whether or not neighboring bees are in bounds
			Bounds bounds = new Bounds(beemaker.transform.position, new Vector3(beemaker.swarmBounds.x, beemaker.swarmBounds.y, 0));
			if (distance > 0 && distance < neighborRadius && !bounds.Contains(bees[i].transform.position))
			{
				Vector3 diff = transform.position - beemaker.transform.position;
				if (diff.magnitude> 0)
				{
					boundsSum += beemaker.transform.position;
					boundsCount++;
				}
			}
		}

		//Object avoidance
		ahead = (transform.position + rb.velocity.normalized * maxSeeAhead) * (rb.velocity.magnitude / maxSpeed);
		ahead2 = ahead * 0.5f;

		RaycastHit hit;
		if(Physics.Raycast(transform.position, rb.velocity.normalized, out hit, maxSeeAhead)){
			if (hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag != "Bee") {
				_objavoidance = Steer((ahead - hit.transform.position).normalized,false);
			}

		}


		//Path Following
		Vector3 target;
		target = pathnodes [currentNode];
		if (Vector3.Distance (transform.position, target) <= 1f) {
			
			currentNode = pathDirection == "Left" ? (currentNode + 1) : (currentNode - 1);
			if (currentNode >= pathnodes.Length && pathDirection == "Left") {
				currentNode = 0;//pathnodes.Length - 1;
			} else if (currentNode <= 0 && pathDirection == "Right"){
				currentNode = pathnodes.Length-1;
			}

		}
		if (target != null) {
			_pathfollow = Steer (target, false);
		}


		if (isAttacking) {
			_attackvector = Steer (eyetarget, false);
		}


		// end
		_separation = separationCount > 0 ? (separationSum / separationCount)*0.5f : separationSum*0.5f;
		_alignment = alignmentCount > 0 ? Limit(alignmentSum / alignmentCount, maxSteer) : alignmentSum;
		_cohesion = cohesionCount > 0 ? Steer(cohesionSum / cohesionCount, false) : cohesionSum;
		_bounds = boundsCount > 0 ? Steer(boundsSum / boundsCount, false) : boundsSum;
	}

	/// <summary>
	/// Returns a steering vector to move the drone towards the target
	/// </summary>
	/// <param type="Vector3" name="target"></param>
	/// <param type="bool" name="slowDown"></param>
	protected virtual Vector3 Steer(Vector3 target, bool slowDown)
	{
		// the steering vector
		Vector3 steer = Vector3.zero;
		Vector3 targetDirection = target - transform.position;
		float targetDistance = targetDirection.magnitude;

		//transform.LookAt(target);

		if (targetDistance > 0)
		{
			// move towards the target
			targetDirection.Normalize();

			// we have two options for speed
			if (slowDown && targetDistance < 100f * speed)
			{
				targetDirection *= (maxSpeed * targetDistance / (100f * speed));
				targetDirection *= speed;
			}
			else
			{
				targetDirection *= maxSpeed;
			}

			// set steering vector
			steer = targetDirection - rb.velocity;
			steer = Limit(steer, maxSteer);
		}

		return steer;
	}

	/// <summary>
	/// Limit the magnitude of a vector to the specified max
	/// </summary>
	/// <param type="Vector3" name="v"></param>
	/// <param type="float" name="max"></param>
	protected virtual Vector3 Limit(Vector3 v, float max)
	{
		if (v.magnitude > max)
		{
			return v.normalized * max;
		}
		else
		{
			return v;
		}
	}

	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag == "Player") {
			coll.transform.parent.gameObject.BroadcastMessage ("HitByBee");			




			gm.addWoundToEye (coll.contacts [0].point);

//			swarm.bees.Remove (this.gameObject);
//			Destroy (this.gameObject);
			this.transform.position = new Vector3 (-2.3f, 9.9f, 0);
			this.currentNode = 0;
			AttackMode (false);
		}
	}

//	void OnDrawGizmos(){
//		Gizmos.DrawRay (transform.position, (rb.velocity.normalized * maxSeeAhead));
//
//	}


}
