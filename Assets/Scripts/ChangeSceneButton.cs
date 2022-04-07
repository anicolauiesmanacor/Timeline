using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour {
    public void LoadScene(string level) {
        GameObject game = GameObject.Find("_GameLogic");

        SceneManager.LoadScene(level);
        if (level == "1" || level == "2" || level == "3" || level == "4" || level == "5" || level == "6" || 
            level == "7" || level == "8" || level == "9" || level == "10" || level == "11" || level == "12") {
            game.GetComponent<Game>().initGame();
        }

    //SOUND
        if (level == "MainMenu" || level == "LevelMenu" || level == "OptionMenu") {
            GameObject sndManager = GameObject.Find("SoundManager");
            GameObject sndSource = GameObject.Find("SoundSource");
            if (game.GetComponent<Game>().isMusicEnabled && sndSource.GetComponent<AudioSource>().clip == sndManager.GetComponent<SoundManager>().gameMusic) {
                Debug.Log("playing music");
                sndManager.GetComponent<SoundManager>().PlayMusic(sndManager.GetComponent<SoundManager>().menuMusic);
            }
        } else {
            if (game.GetComponent<Game>().isMusicEnabled) { 
                GameObject sndManager = GameObject.Find("SoundManager");
                sndManager.GetComponent<SoundManager>().PlayMusic(sndManager.GetComponent<SoundManager>().gameMusic);
            }
        }
    }

    public void exitApp() {
        Application.Quit();
    }
}