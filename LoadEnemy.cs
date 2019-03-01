using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadEnemy : MonoBehaviour {

    Sprite[] images;
    GameObject enemyImg;

	void Start () {
        images = Resources.LoadAll<Sprite>("Enemy/zako");
        enemyImg = GameObject.Find("EnemyImage");

        enemyImg.GetComponent<SpriteRenderer>().sprite = images[10];

    }
	
	// Update is called once per frame
	void Update () {
    }
}
