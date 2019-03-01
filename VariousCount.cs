using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariousCount : MonoBehaviour {

    public Text CountAllEnergyText;
    public Text CountGetEnergyText;
    public int countAllEnergy;
    public int countGetEnergy;

	void Start () {
        CountAllEnergyText.text = "";
        CountGetEnergyText.text = "";

        countAllEnergy = 0;
        countGetEnergy = 0;
	}
	
	void Update () {
    }

}
