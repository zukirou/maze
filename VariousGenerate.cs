using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariousGenerate : MonoBehaviour {

    public GameObject various;
    private int energyCountAll;

    GameObject CountManager;
    VariousCount variousCount;

    GameObject MainCamera;
    PlayerController playerController;

    public int SetWay;
    //0 とりあえず通路に全部置きとか通路にｎ個おきに置くとか
    //1 指定された座標に１個置く

    int pathCount;//通路にのみに配置する。その通路のカウント


    public int setIntervalCount;//いくつおきに配置するかの「いくつ」の設定。これは、n個おきに置かれる　ということではなく、　MapGenerateで振られた番号をn個おきにチェックし、それが通路だったら配置　としている

    GameObject DungeonInfo;
    MapGenerate dungeon;
    int width;
    int depth;
    bool firstSet;

    public float zAxis, xAxis, yAxis;
    public float xPos, yPos, zPos;

    void Start () {
        DungeonInfo = GameObject.Find("MapGenerator");
        dungeon = DungeonInfo.GetComponent<MapGenerate>();
        firstSet = true;
        pathCount = 0;

        energyCountAll = 0;
        CountManager = GameObject.Find("CountManager");
        variousCount = CountManager.GetComponent<VariousCount>();

        MainCamera = GameObject.Find("MainCamera");
        playerController = MainCamera.GetComponent<PlayerController>();
    }
	
	void Update () {
        if (firstSet) {
            switch (SetWay) {
                case 0:
                    VariousSetOnPath(various);
                    break;
                case 1:
                    VariousSetFixedPlaceOne(various, xPos, zPos);
                    break;
                default:
                    break;
            }
            firstSet = false;
        }
    }

    //通路上にｎ個置き配置
    void VariousSetOnPath(GameObject obj) {
        width = dungeon.width;
        depth = dungeon.depth;
        for (int z = 0; z < depth; z += 1) {
            for (int x = 0; x < width; x += 1) {
                if (dungeon.Dungeon[x, z] == dungeon.Path) {
                    pathCount++;
                    if (pathCount % setIntervalCount == 0) {
                        Instantiate(obj, new Vector3(x * 2, yPos, z * 2), Quaternion.Euler(xAxis, yAxis, yAxis));

                        //配置されたエネルギーの総数カウント
                        if(obj.transform.tag == "energy") {
                            energyCountAll++;
                            variousCount.countAllEnergy = energyCountAll * 10;
                            variousCount.CountAllEnergyText.text = variousCount.countAllEnergy.ToString();
                        }
                    }
                }
            }
        }
    }

    //通路上のx,zに１個だけ置く
    void VariousSetFixedPlaceOne(GameObject obj,float x, float z) {
        Instantiate(obj, new Vector3(x * 2, yPos, z * 2), Quaternion.Euler(xAxis, yAxis, yAxis));
    }

    //Playerの前上方に置く。Gate出現時のコンパス用途
    void VariousSetObliquelyFoward(GameObject obj, float x, float y, float z) {
        float xPos = playerController.transform.position.x;
        float yPos = playerController.transform.position.y;
        float zPos = playerController.transform.position.z;

        Instantiate(obj, new Vector3(xPos, yPos + y, zPos - z), Quaternion.Euler(xAxis, yAxis, yAxis));
    }
}
