using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

	public AudioMixer mix;
	public AudioMixerGroup normal;
	public AudioMixerGroup weird;
	public AudioClip[] Clips;
	private AudioSource[] audioSources;

	bool increaseVol = true;
	bool gameEnded = false;
	public float timeTillMaxVol;

	//LevelManagerScript levelManager;
	public static MusicManager instance = null;

	float tweenDown;
	float tween;
	float timeEnded;

	void Awake() {
//		if (instance != null) {
//			Destroy (gameObject);
//		} else {
//			instance = this;
//			GameObject.DontDestroyOnLoad (gameObject);
//		}
	}

	// Use this for initialization
	void Start () {
		
		audioSources = new AudioSource[Clips.Length];
		int i = 0;
		while (i < Clips.Length)
		{
			GameObject child = new GameObject("Player");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>() as AudioSource;
			audioSources[i].clip = Clips[i];
			//audioSources [i].mute = true;
			audioSources [i].volume = 0.1f;
			audioSources [i].loop = true;
			audioSources[i].outputAudioMixerGroup = normal;
			i++;
		}

		audioSources[0].Play();

	}
		
	
	// Update is called once per frame
	void Update () {
		
		if (!gameEnded) {
			tween = Mathf.Lerp (0.6f, 1, Time.time / (timeTillMaxVol * 60));
		} else {
			tween = Mathf.Lerp (tweenDown, 0, (Time.time - timeEnded) / 1.5f);
		}
	//	tween = 1f;

		audioSources [0].volume = tween;
	}

//	public void lastWeird(){
//		audioSources [6].mute = false;
//	}
//
//	public void MakeItWeird(){
//		audioSources[0].outputAudioMixerGroup = weird;
//
//		if (levelManager.GetCoffeeCount () >= 1) {
//
//			mix.SetFloat ("weird_volume", -10f + Mathf.Clamp (levelManager.GetCoffeeCount () / 4f, 0, 1f) * 3f);
//			mix.SetFloat ("flange_rate", 0.1f + Mathf.Clamp (levelManager.GetCoffeeCount () / 3f, 0, 1f) * 4f);
//			mix.SetFloat ("echo_delay", 0f + Mathf.Clamp (levelManager.GetCoffeeCount () / 6f, 0, 1f) * 19.0f);
//			mix.SetFloat ("distort_level", 0f + Mathf.Clamp (levelManager.GetCoffeeCount () / 6f, 0, 1f) * 0.7f);
//
//		}
//
//	}

	public void GameOver(){
		tweenDown = audioSources [0].volume;
		gameEnded = true;
		timeEnded = Time.time;

	}



}
