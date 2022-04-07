using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	// Audio players components.
	public AudioSource EffectsSource;
	public AudioSource MusicSource;

	public AudioClip menuMusic;
	public AudioClip gameMusic;
	public AudioClip clickFX;
	public AudioClip correctFX;
	public AudioClip incorrectFX;
	public AudioClip winnerFX;
	public AudioClip lostFX;

	public GameObject game;

	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	// Singleton instance.
	public static SoundManager Instance = null;

	// Initialize the singleton instance.
	private void Awake() {
		GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
		if (objs.Length > 1) {
			Destroy(this.gameObject);
		}
		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(this.gameObject);
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip) {
		if (game.GetComponent<Game>().isFXEnabled) {
			EffectsSource.clip = clip;
			EffectsSource.Play();
		}
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip) {
		if (game.GetComponent<Game>().isMusicEnabled) {
			MusicSource.enabled = true;
			MusicSource.loop = true;
			MusicSource.clip = clip;
			MusicSource.Play();
		}
	}
	
	public void Play(int i) {
		if (game.GetComponent<Game>().isFXEnabled) {
			EffectsSource.enabled = true;
			if (i == 0) {
				EffectsSource.clip = clickFX;
			} else if (i == 1) {
				EffectsSource.clip = correctFX;
			} else if (i == 2) {
				EffectsSource.clip = incorrectFX;
			} else if (i == 3) {
				EffectsSource.clip = winnerFX;
			}
			EffectsSource.Play();
		}
	}

	// Play a single clip through the music source.
	public void PlayMusic(int i) {
		if (game.GetComponent<Game>().isMusicEnabled) {
			MusicSource.enabled = true;
			MusicSource.loop = true;
			if (i == 0) { 
				MusicSource.clip = menuMusic;
			} else if (i == 1) {
				MusicSource.clip = gameMusic;
			}
			MusicSource.Play();
		}
	}

	public void playMenuMusic() {
		if (game.GetComponent<Game>().isMusicEnabled)
			PlayMusic(menuMusic);
	}

	public void playGameMusic() {
		if (game.GetComponent<Game>().isMusicEnabled)
			PlayMusic(gameMusic);
	}

	public void StopMusic() {
		MusicSource.Stop();
	}

	public void setMusicVolume(float f) {
		MusicSource.volume = f;
	}

	public void setFXVolume(float f) {
		EffectsSource.volume = f;
	}
}