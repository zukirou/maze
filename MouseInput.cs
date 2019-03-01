using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    [Header("Input Settings:")]
    public float pressureForce;

    public RaycastHit hitInfo;
    public Ray ray;

    public Vector3 inputPoint;
    public JellyBody jellyBody;
    public GameObject jelly;

    public JellyBody.Status jellyBodyStatus;

    public static MouseInput instance;

    public GameObject BattleCamera;
    public Camera battleCamera;

    public int energyID;

	void Start () {
        instance = this;
        pressureForce = 20f;

        energyID = 0;

        BattleCamera = GameObject.Find("BattleCamera");
        battleCamera = BattleCamera.GetComponent<Camera>();

        jellyBodyStatus = JellyBody.Status.released;//これ、つかってなくね？
	}


    public void Update() {
        CheckForTouch();
        GetEnergyID();
        jellyBodyStatus = JellyBody.Status.released;
    }


    public int GetEnergyID() {
        return energyID;
    }

    public void ResetEnergyID() {
        energyID = 0;
    }

    public void CheckForTouch() {
        if (Input.GetMouseButton(0)) {
            ray = battleCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hitInfo)) {
                jellyBody = hitInfo.collider.gameObject.GetComponent<JellyBody>();
                if(jellyBody != null) {
                    inputPoint = hitInfo.point;
                    inputPoint += hitInfo.normal * 0.1f;
                    jellyBody.AddPointForce(pressureForce, inputPoint);

                    //どのエネルギーにタッチしたか、そのIDを返す
                    //IDは個々にpublicで設定
                    jellyBodyStatus = JellyBody.Status.touched;
                    energyID = jellyBody.id;
                }
            }
        }
    }
}

