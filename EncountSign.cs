using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountSign : MonoBehaviour {

    GameObject gameProgress;
    GameProgress Progress;

    void Start() {

        gameProgress = GameObject.Find("ProgressManager");
        Progress = gameProgress.GetComponent<GameProgress>();

    }

    void Update() {

    }

    public void OnTriggerEnter(Collider other) {
        Progress.phase = GameProgress.Phase.justEncount;

        //繰り返し同じ場所でエンカウントできないように、いまエンカウントしたそこを破棄
        Destroy(this.gameObject);
    }
}
