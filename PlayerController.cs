using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    Rigidbody m_Rigidbody;
    GameObject LookBackButton;
    GameObject DungeonInfo;
    GameObject Energy;
    Energy energyScript;
    GameObject energyGetParticle;
    MapGenerate mapGenerateScript;

    float panSpeed = 1.0f;
    float dollySpeed = 2.0f;
    public float x = 0.0f;
    public float z = 0.0f;
    public int moveFlag = 0;
    int Stop = 0;
    int Go = 1;

    bool rightTurnBTN = false;
    bool leftTurnBTN = false;
    bool lookBack = false;
    bool playFirst = true;

    BGMPlayer player;

    void Start(){

        // 自分のRigidbodyを取ってくる
        m_Rigidbody = GetComponent<Rigidbody>();

        //振り返るボタンの表示非表示をするため取得
        LookBackButton = GameObject.Find("LookBackButton");

        //マップ内で壁か通路かを確認するために取得
        //開始時の正面が壁じゃないようにするためにやってみた
        DungeonInfo = GameObject.Find("MapGenerator");
        mapGenerateScript = DungeonInfo.GetComponent<MapGenerate>();

        player = new BGMPlayer("Nyonnyonyomo");
        player.playBGM();

    }

    void Update(){

        //開始時の正面が壁を向いていないようにする
        if (playFirst) {
            if(mapGenerateScript.Dungeon[1, 2] == 1) {
                transform.Rotate(new Vector3(0.0f, 90, 0.0f));
                playFirst = false;
            }
        }

        //移動のvelocityをリセットするためにやってる。これがないとうごきっぱなしになっちゃったから。
        x = 0.0f;
        z = 0.0f;


        //各ボタン押された時の挙動　振り返るボタンの表示非表示の切り替え
        if (moveFlag == Go){
            z += dollySpeed;
            LookBackButton.SetActive(false);
        }
        if(moveFlag == Stop) {
            LookBackButton.SetActive(true);
        }

        if (rightTurnBTN) {
            transform.Rotate(new Vector3(0.0f, -2 * panSpeed, 0.0f));
        }

        if (leftTurnBTN) {
            transform.Rotate(new Vector3(0.0f, 2 * panSpeed, 0.0f));
        }

        m_Rigidbody.velocity = z * transform.forward;

        if (lookBack) {
            transform.Rotate(new Vector3(0.0f, 180, 0.0f));
            lookBack = false;

        }
    }

    public void GoStopButtonDown(){
        if(moveFlag == Stop){
            moveFlag = Go;
            return;
        }
        if(moveFlag == Go){
            moveFlag = Stop;
            return;
        }
    }

    public void TurnLeftButtonDown(){
        rightTurnBTN = true;
    }

    public void TurnLeftButtonUp() {
        rightTurnBTN = false;
    }

    public void TurnRightButtonDown(){
        leftTurnBTN = true;
    }

    public void TurnRightButtonUp() {
        leftTurnBTN = false;
    }

    public void LookBackButtonDown() {
        lookBack = true;
    }

}
