using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnOffController : MonoBehaviour {

    GameObject[] objects;
    GameObject[] obj;


    public string objTagName;
    public string ObjTagName {
        get { return objTagName; }
        set { ObjTagName = value; }
    }

    [SerializeField] float appearContinuationTime;
    [SerializeField] float vanishContinuationTime;
    private float timeElapsed;
    private int objOn;



    void Start () {
        objOn = 1;
        timeElapsed = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= appearContinuationTime && objOn == 1) {
            objects = GameObject.FindGameObjectsWithTag(objTagName);
            foreach (GameObject obj in objects) {
                obj.SetActive(false);
            }
            timeElapsed = 0.0f;
            objOn = 0;
        }

        if(timeElapsed >= vanishContinuationTime && objOn == 0) {
            foreach (GameObject obj in objects) {
                obj.SetActive(true);
            }
            timeElapsed = 0.0f;
            objOn = 1;
        }
    }
}
