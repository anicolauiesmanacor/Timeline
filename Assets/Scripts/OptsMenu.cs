using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptsMenu : MonoBehaviour {
    GameObject game;

    public GameObject txtMusica;
    public GameObject txtFX;
    public GameObject ScrBarMusica;
    public GameObject ScrBarFX;
    public GameObject txtDif;
    GameObject sndManager;
    
    void Awake() {
        initOptionsMenu();
    }

    public void initOptionsMenu() {
        game = GameObject.Find("_GameLogic");
        sndManager = GameObject.Find("SoundManager");

        if (PlayerPrefs.GetInt("isMusicEnabled") == 1) {
            game.GetComponent<Game>().isMusicEnabled = true;
            txtMusica.GetComponent<Text>().text = "OFF";
        } else {
            game.GetComponent<Game>().isMusicEnabled = false;
            txtMusica.GetComponent<Text>().text = "ON";
        }

        if (PlayerPrefs.GetInt("isFXEnabled") == 1) {
            game.GetComponent<Game>().isFXEnabled = true;
            txtFX.GetComponent<Text>().text = "OFF";
        } else {
            game.GetComponent<Game>().isFXEnabled = false;
            txtFX.GetComponent<Text>().text = "ON";
        }

        game.GetComponent<Game>().musicVolume = PlayerPrefs.GetFloat("musicVolume");
        ScrBarMusica.GetComponent<Scrollbar>().value = game.GetComponent<Game>().musicVolume;
        sndManager.GetComponent<SoundManager>().setMusicVolume(game.GetComponent<Game>().musicVolume);

        game.GetComponent<Game>().fxVolume = PlayerPrefs.GetFloat("fxVolume");
        ScrBarFX.GetComponent<Scrollbar>().value = game.GetComponent<Game>().fxVolume;
        sndManager.GetComponent<SoundManager>().setFXVolume(game.GetComponent<Game>().fxVolume);

        if (game.GetComponent<Game>().difficulty == 0) {
            txtDif.GetComponent<Text>().text = "Fàcil";
        } else if (game.GetComponent<Game>().difficulty == 1) {
            txtDif.GetComponent<Text>().text = "Normal";
        } else if (game.GetComponent<Game>().difficulty == 2) {
            txtDif.GetComponent<Text>().text = "Difícil";
        }
    }

    public void enableMusic() {
        game.GetComponent<Game>().isMusicEnabled = !game.GetComponent<Game>().isMusicEnabled;
        PlayerPrefs.SetInt("isMusicEnabled", game.GetComponent<Game>().isMusicEnabled ? 1 : 0);

        if (game.GetComponent<Game>().isMusicEnabled) {
            txtMusica.GetComponent<Text>().text = "OFF";
            sndManager.GetComponent<SoundManager>().PlayMusic(sndManager.GetComponent<SoundManager>().menuMusic);
        } else {
            txtMusica.GetComponent<Text>().text = "ON";
            sndManager.GetComponent<SoundManager>().StopMusic();
        }
    }

    public void enableFX() {
        game.GetComponent<Game>().isFXEnabled = !game.GetComponent<Game>().isFXEnabled;
        PlayerPrefs.SetInt("isFXEnabled", game.GetComponent<Game>().isFXEnabled ? 1 : 0);

        if (game.GetComponent<Game>().isFXEnabled) { 
            txtFX.GetComponent<Text>().text = "OFF";
            sndManager.GetComponent<SoundManager>().Play(1);
        } else { 
            txtFX.GetComponent<Text>().text = "ON";
        }
    }

    public void setMusicVolume() {
        game.GetComponent<Game>().musicVolume = ScrBarMusica.GetComponent<Scrollbar>().value;
        sndManager.GetComponent<SoundManager>().setMusicVolume(ScrBarMusica.GetComponent<Scrollbar>().value);
        PlayerPrefs.SetFloat("musicVolume", game.GetComponent<Game>().musicVolume);
    }

    public void setFXVolume() {
        game.GetComponent<Game>().fxVolume = ScrBarFX.GetComponent<Scrollbar>().value;
        sndManager.GetComponent<SoundManager>().setFXVolume(ScrBarMusica.GetComponent<Scrollbar>().value);
        PlayerPrefs.SetFloat("fxVolume", game.GetComponent<Game>().fxVolume);
    }

    public void setDifficulty() {
        game.GetComponent<Game>().difficulty = (game.GetComponent<Game>().difficulty + 1) % 3;

        txtDif = GameObject.Find("TextDif");
        if (game.GetComponent<Game>().difficulty == 0)
            txtDif.GetComponent<Text>().text = "Fàcil";
        else if (game.GetComponent<Game>().difficulty == 1)
            txtDif.GetComponent<Text>().text = "Normal";
        else if (game.GetComponent<Game>().difficulty == 2)
            txtDif.GetComponent<Text>().text = "Difícil";

        PlayerPrefs.SetInt("difficulty", game.GetComponent<Game>().difficulty);
    }
}