using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour {

    public GameObject particle;

    GameObject VariousCount;
    VariousCount variousCount;

    GameObject ProgressManager;
    GameProgress gameProgress;

	void Start () {
        VariousCount = GameObject.Find("CountManager");
        variousCount = VariousCount.GetComponent<VariousCount>();

        ProgressManager = GameObject.Find("ProgressManager");
        gameProgress = ProgressManager.GetComponent<GameProgress>();
    }
	

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {

            //energy獲得時のエフェクト発生
            Instantiate(particle, transform.position, transform.rotation);

            //消す
            Destroy(this.gameObject);

            //カウントアップ
            variousCount.countGetEnergy += 10;
            variousCount.CountGetEnergyText.text = variousCount.countGetEnergy.ToString();

            //全部のエネルギーをゲットしたかチェック
            if(variousCount.countGetEnergy == variousCount.countAllEnergy) {
                gameProgress.phase = GameProgress.Phase.energyAllGet;
            }
        }
    }

}