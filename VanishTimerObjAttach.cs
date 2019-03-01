using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishTimerObjAttach : MonoBehaviour {

    GameObject obj;
    public float vanishTime;
    private float timeElapsed;

	void Start () {
        obj = this.gameObject;
        obj.SetActive(true);
	}
	
	void Update () {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= vanishTime) {
            obj.SetActive(false);
        }
	}
}
