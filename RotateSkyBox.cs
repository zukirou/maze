using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour {

    //回転スピード
    [SerializeField]
    private float rotateSpeed = 0.5f;

    //スカイボックスのマテリアル
    private Material skyboxMaterial;

    void Start() {
        //設定されたスカイボックスのマテリアルを取得
        skyboxMaterial = RenderSettings.skybox;
    }

    void Update () {
        transform.Rotate(Vector3.one * rotateSpeed * Time.deltaTime);
    }
}
