using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadText : MonoBehaviour {

    public string[] textMessage;//テキストの加工前の一行を入れる変数
    public string[,] textWords;//テキストの複数列を入れる２次元配列

    public string filename;

    private int rowLength;//テキスト内の行数取得用
    private int columnLength;//テキスト内の列数取得用

    private string TextLines;

	void Start () {
        TextAsset textAsset = new TextAsset();//テキストファイルのデータを取得するインスタンス作成

        textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;//Resourcesフォルダから対象テキストファイルを取得

        TextLines = textAsset.text;//テキスト全体をstring型で保持する


        //splitで１行づつ代入した一次配列を作成
        textMessage = TextLines.Split('\n');

        //行数と列数を取得
        columnLength = textMessage[0].Split('\t').Length;
        rowLength = textMessage.Length;

        //二次配列を定義
        textWords = new string[rowLength, columnLength];

        for(int i = 0; i < rowLength ; i++) {
            string[] tempWords = textMessage[i].Split('\t');
            for(int n = 0; n < columnLength; n++) {
                textWords[i, n] = tempWords[n];

                Debug.Log(textWords[i, n]);
            }
        }
	}
	
	void Update () {
		
	}
}
