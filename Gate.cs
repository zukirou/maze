using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    GameObject gameProgress;
    GameProgress Progress;
    
	void Start () {

        gameProgress = GameObject.Find("ProgressManager");
        Progress = gameProgress.GetComponent <GameProgress> ();

	}
	
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other) {
        Progress.phase = GameProgress.Phase.arriveGate;
    }
}
