using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageController : MonoBehaviour {
    Image GoStop_Image;
    public Sprite goImage;
    public Sprite stopImage;
    GameObject mainCamera;
    PlayerController playerController;


	void Start () {
        GoStop_Image = GetComponent<Image>();
        GoStop_Image.sprite = goImage;

        mainCamera = GameObject.Find("MainCamera");
        playerController = mainCamera.GetComponent<PlayerController>();
	}
	
	void Update () {
        if (playerController.moveFlag == 0){
            GoStop_Image.sprite = goImage;
        }
        if (playerController.moveFlag == 1){
            GoStop_Image.sprite = stopImage;
        }
	}
}
