using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;
using DG.Tweening;

public class MusicManagerNew : MonoBehaviour {
	public static MusicManagerNew instance;
	
	public AudioMixer mainMixer;
	public AudioMixerGroup[] musicGroup;
	public AudioMixerGroup[] sfxGroup;
	public AudioClip[] Songs;
	private AudioSource[] SongDatabase;
	public AudioClip[] Sounds;
	private AudioSource[] SoundDatabase;

	void Awake(){
		if (!instance) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	void Start(){
		createDatabase ("Song", ref SongDatabase, ref Songs, musicGroup);
		createDatabase ("Sound", ref SoundDatabase, ref Sounds, sfxGroup);
	}
		
	private void createDatabase(string name, ref AudioSource[] audios, ref AudioClip[] clips, AudioMixerGroup[] mixergroup){
		audios = new AudioSource[clips.Length];
		for (int i = 0; i < clips.Length; i++) {
			GameObject child = new GameObject (name + "Player_" + i);
			child.transform.parent = gameObject.transform;
			audios [i] = child.AddComponent<AudioSource> () as AudioSource;
			audios [i].clip = clips [i];
			//audios [i].mute = true;
			audios [i].playOnAwake = false;

			if (clips [i].name == "Music_GameOver" || clips [i].name == "Music_Victory") {
				audios [i].loop = false;
			} else {
				audios [i].loop = true;
			}

			audios [i].outputAudioMixerGroup = mixergroup[i];
		}
	}


	public void PlaySound(string soundname){
		int i = GetSoundIndex (soundname);
		PlaySound (i);
	}
	public void PlaySound(int soundindex){
		if (!SoundDatabase [soundindex].isPlaying ) {
			SoundDatabase [soundindex].PlayOneShot (SoundDatabase [soundindex].clip);
		}
	}

	public void PlaySong(string songname){
		int i = GetSongIndex (songname);
		PlaySong (i);
	}
	public void PlaySong(int songindex){
		if (!SongDatabase [songindex].isPlaying ) {
			SongDatabase [songindex].Play ();
		}
	}
	public void PauseSong(string songname){
		int i = GetSongIndex (songname);
		PauseSong (i);
	}
	public void PauseSong(int songindex){
		SongDatabase [songindex].Pause ();
	}
	public void StopSong(string songname){
		int i = GetSongIndex (songname);
		StopSong (i);
	}
	public void StopSong(int songindex){
		SongDatabase [songindex].Stop ();
	}
	public void SetSongVolume(string songname, float vol){
		int i = GetSongIndex (songname);
		SetSongVolume (i, vol);
	}
	public void SetSongVolume(int songindex, float vol){
		SongDatabase [songindex].volume = vol;
	}
	public void SongFadeIn(string songname, float transitionTime){
		int i = GetSongIndex (songname);
		FadeIn (ref SongDatabase [i], transitionTime);
	}
	public void SongFadeIn(int songindex, float transitionTime){
		FadeIn (ref SongDatabase [songindex], transitionTime);
	}
	public void SongFadeOut(string songname, float transitionTime){
		int i = GetSongIndex (songname);
		FadeOut (ref SongDatabase [i], transitionTime);
	}
	public void SongFadeOut(int songindex, float transitionTime){
		FadeOut (ref SongDatabase [songindex], transitionTime);
	}
	public void SongCrossFade(string songFrom, string songTo, float transitionTime){
		int songA = GetSongIndex (songFrom);
		int songB = GetSongIndex (songTo);
		SongCrossFade (songA,songB, transitionTime);
	}
	public void SongCrossFade(int songFrom, int songTo, float transitionTime){
		CrossFade (ref SongDatabase [songFrom], ref SongDatabase [songTo], transitionTime);
	}


	public void SongFadeInVolOnly(string songname, float transitionTime){
		int i = GetSongIndex (songname);
		FadeInVolOnly (ref SongDatabase [i], transitionTime);
	}
	public void SongFadeInVolOnly(int songindex, float transitionTime){
		FadeInVolOnly (ref SongDatabase [songindex], transitionTime);
	}
	private void FadeInVolOnly(ref AudioSource aud, float dur){
		//aud.Play ();
		aud.volume = 0;
		aud.DOFade (1f, dur);
	}



	private void FadeIn(ref AudioSource aud, float dur){
		aud.Play ();
		aud.volume = 0;
		aud.DOFade (1f, dur);
	}
	private void FadeOut(ref AudioSource aud, float dur){
		aud.volume = 1f;
		aud.DOFade (0f, dur);
	}
	private void CrossFade(ref AudioSource song1, ref AudioSource song2, float dur){
		FadeOut (ref song1, dur);
		FadeIn (ref song2, dur);
	}


	private int GetSongIndex(string songname){
		for (int i = 0; i < SongDatabase.Length; i++) {
			if (SongDatabase [i].clip.name == songname) {
				return i;
			}
		}
		return 0;
	}
	private int GetSoundIndex(string soundname){
		for (int i = 0; i < SoundDatabase.Length; i++) {
			if (SoundDatabase [i].clip.name == soundname) {
				return i;
			}
		}
		return 0;
	}

}
