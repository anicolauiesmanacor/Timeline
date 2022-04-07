using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashScreen : MonoBehaviour {
    void Start() {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
}
