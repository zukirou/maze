using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPurposeTimer : MonoBehaviour {

    public float limitTime;
    public float timeElapsed;

    void Start () {
        timeElapsed = 0;
	}

    void Update () {
        TimeElapsed();
    }

    public bool TimeElapsed() {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= limitTime) {
            timeElapsed = 0;
            return true;
        }
        return false;
    }
}
