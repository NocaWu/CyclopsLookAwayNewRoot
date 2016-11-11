using UnityEngine;
using System.Collections;
using UnityEngine.UI; //this allows us to address the text object

public class GameLogic : MonoBehaviour {

	//public variable and functions can be seen, used, and modfied (for vars) outside the class
	//public variables can also be set/modified  in the inspector
	public GameObject player;
	public GameObject upperEyelid;
	public GameObject lowerEyelid;
	float score;
	float highscore;
	bool gameStarted ;

	public Text scoreText;
	public Text highScoreText;
	public CanvasGroup welcome;
	
	public GameObject enemyPrefab;
	public Transform enemyContainer; //to hold new enemies and stop from cluttering our editor.

	float timeSinceLastEnemyCreated = 0f;
	float enemySpawnDelay = 1f;

	// Use this for initialization
	void Start () {
		highscore = 0;
		Reset();

	}
	
	// Update is called once per frame
	void Update () {

		highScoreText.text = "" + highscore;  //set the high score text

		if (gameStarted){
			scoreText.text = "" + score;

			timeSinceLastEnemyCreated += Time.deltaTime; //increase this timer by the time since the last frame
			Debug.Log (timeSinceLastEnemyCreated);
			Debug.Log (enemySpawnDelay);
			if (timeSinceLastEnemyCreated > enemySpawnDelay){ //if enough time has passed, time to spawn a new enemy
				//create a new enemy and reset the timer
				//we want to create the enemy at a fixed distance from the player
				float enemySpawnDistance = 15.0f; 

				//and at a random angle to the player (in Radians rather than degree)
				float enemySpawnAngle = Random.Range(-Mathf.PI, Mathf.PI);

				//start by getting the ring position and then add an offset from the random angle
				Vector2 newEnemyPosition = player.transform.position; //we'll use the ring as the center

				//Again, if your trig is rusty, don't worry about this. We'll talk about it in Code Lab 0
				newEnemyPosition.x += enemySpawnDistance * Mathf.Cos(enemySpawnAngle);
				newEnemyPosition.y += enemySpawnDistance * Mathf.Sin(enemySpawnAngle);

				//Make a new enemy from a Prefab
				GameObject newEnemy = Instantiate(enemyPrefab,newEnemyPosition,Quaternion.identity) as GameObject;
				newEnemy.transform.parent = enemyContainer;

				timeSinceLastEnemyCreated = 0; //reset the timer
				enemySpawnDelay *= 0.98f; //get faster
			}
			if (welcome.alpha <= 1f && welcome.alpha > 0){
				welcome.alpha -= 1.0f * Time.deltaTime;
			} else {
				welcome.alpha = 0;
			}


		} else {
			scoreText.text = "click dot";
		}

		//if the ring is larger than normal, animate it back
		if (player.transform.localScale.x > 4f) {
			//we can't change the transform x,y or z direct, so we make a copy
			Vector3 newScale = player.transform.localScale;
			newScale.x /= 1.001f;
			newScale.y /= 1.001f;
			newScale.z /= 1.001f;
			
			//now copy it back
			player.transform.localScale = newScale;
		}
	}

	//function is called by PlayerControl when the circle is clicked.
	public void StartGame(){
		gameStarted = true;
		//welcome.GetComponent<UnityEngine.UI.Text>().CrossFadeAlpha (0f, 1.0f, false);
	}

	public void Reset() { //Tell the game manager to reset everything

		gameStarted = false;

		//Kill any living enemies
		for(int i = 0; i < enemyContainer.childCount; i++){
			Transform enemy = enemyContainer.GetChild(i);
		  	Destroy(enemy.gameObject);
		}

		//Put the player and ring back in the center of the world
		EyeballController playerControl = player.GetComponent<EyeballController>();
		//playerControl.Reset();
	

		if (score > highscore){
			highscore = score; //store high score
		}

		score = 0;
		timeSinceLastEnemyCreated = 0f;
		enemySpawnDelay = 1f;

		//play the start sound
		GetComponent<AudioSource>().Play();
	}

	//this function is called when an enemy touches the ring and sends a message to call this function

	public void EnemyTouch(GameObject sender){
		//they say 'don't kill the messenger' but in this case we have to
		Destroy(sender);

		score += 10f;
		
		//give the ring a little reaction when it gets hit, by increasing its size
		player.transform.localScale = new Vector3(4.2f,4.2f,4.2f); 
		//make the player turn red would be nice
		//player.GetComponent<MeshRenderer> ().material.color = Color.white;

		//Close the eyelid
		upperEyelid.transform.localEulerAngles = new Vector3(330f + (score/2), 0,0);
		lowerEyelid.transform.localEulerAngles = new Vector3(330f - (score/2), 0,0);
	
			//upperEyelid.transform.rotation = Quaternion.Slerp(upperEyelid.transform.rotation, 330f+score, Time.deltaTime * 2.0f);
			//upperEyelid.transform.Rotate (new Vector3(0, 30f,0));


	}
}
