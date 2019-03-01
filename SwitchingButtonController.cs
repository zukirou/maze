using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchingButtonController : MonoBehaviour {

    public string nextSceneName;

    public void TouchStartButton() {
        StartCoroutine(LoadScene(nextSceneName));
    }

    IEnumerator LoadScene(string name02) {
        AsyncOperation async = SceneManager.LoadSceneAsync(name02, LoadSceneMode.Single);
        yield return async;
    }
}
