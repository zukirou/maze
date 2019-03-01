using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
//using Coffee.UIExtensions;


public class GameProgress : MonoBehaviour {

    public enum Phase {
        energyLeft,//10
        energyAllGet,//20
        leaveHere,//30
        setArrowAndMapUI,//40
        energyDecrease,//50
        arriveGate,//60
        energyZero,//70
        waitForArriveGate,//80
        waitForGameOver,//90
        justEncount,//100
    }

    public Phase phase;
    public int phaseValue;

    //public ShinyEffectForUGUI m_shinny;

    GameObject Announce_AllEnergyGet;
    GameObject Announce_LeaveHere;
    GameObject Announce_ArriveGate;
    GameObject Announce_EnergyZero;
    GameObject Announce_GameOver;


    GameObject Encount;
    VariousGenerate encountSign;
    ObjectOnOffController objectOnOff;

    GameObject MapGenerator;
    VariousGenerate gate;

    GameObject RadarCamera;
    Camera radarCam;

    GameObject BattleCamera;
    Camera battleCam;

    GameObject EnemyNameCamera;
    Camera enemyNameCam;

    GameObject DirectionArrow;
    Arrow arrow;

    GameObject Player;
    Camera playerCamera;
    PlayerController playerController;

    GameObject MainGameCanvas;
    Canvas mainGameCanvas;

    GameObject BattleGameEnergyCanvas;
    MouseInput battleGameEnergyCanvas_mouseInput;

    //全部取った時のアナウンスとか「希望」とか取った数のカウントとかを動かしたりするために用
    GameObject energyAllCountObj;
    GameObject energyCountObj;
    GameObject ui_energyCountHeader;
    GameObject ui_energyCount;


    //何か表示→非表示するときに使うタイマー用の変数
    public float timeOut;
    private float timeElapsed;

    //エネルギー(190110 希望としてる)カウントと減少させる時のタイマー用の変数
    public int energyCount;
    public float energyDecreaseTime;
    private float energyDecreaseTimeElapsed;
    GameObject CountManager;
    VariousCount variousCount;

    //フェードアウトの暗転の長さを指定するタイマー用の変数
    public float blackOutTimeElapsed;

    [SerializeField]
    Fade fade = null;// http://tsubakit1.hateblo.jp/entry/2015/11/04/015355

    public int fadeFlag;

    BGMPlayer player;

    void Start () {
        fadeFlag = 0;

        Player = GameObject.Find("MainCamera");
        playerCamera = Player.GetComponent<Camera>();
        playerController = Player.GetComponent<PlayerController>();

        Encount = GameObject.Find("EncountPlaceGenerator");
        encountSign = Encount.GetComponent<VariousGenerate>();
        objectOnOff = Encount.GetComponent<ObjectOnOffController>();

        MapGenerator = GameObject.Find("MapGenerator");
        gate = MapGenerator.GetComponent<VariousGenerate>();

        RadarCamera = GameObject.Find("RadarCamera");
        radarCam = RadarCamera.GetComponent<Camera>();

        BattleCamera = GameObject.Find("BattleCamera");
        battleCam = BattleCamera.GetComponent<Camera>();

        EnemyNameCamera = GameObject.Find("EnemyNameCamera");
        enemyNameCam = EnemyNameCamera.GetComponent<Camera>();

        DirectionArrow = GameObject.Find("ArrowPivot");
        DirectionArrow.SetActive(false);

        MainGameCanvas = GameObject.Find("Canvas_mainGame");
        mainGameCanvas = MainGameCanvas.GetComponent<Canvas>();

        BattleGameEnergyCanvas = GameObject.Find("Canvas_battleGameEnergy");
        battleGameEnergyCanvas_mouseInput = BattleGameEnergyCanvas.GetComponent<MouseInput>();


        fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();

        battleCam.enabled = false;
        enemyNameCam.enabled = false;

        battleGameEnergyCanvas_mouseInput.enabled = false;

        phase = Phase.energyLeft;
        //PhaseをPlayerPrefsセーブするのにintじゃないとだめだからphaseValueをつくった。めんどい。やばそうな作りだ・・・
        switch (phaseValue) {
            case 10:
                phase = Phase.energyLeft;
                break;
            case 20:
                phase = Phase.energyAllGet;
                break;
            case 30:
                phase = Phase.leaveHere;
                break;
            case 40:
                phase = Phase.setArrowAndMapUI;
                break;
            case 50:
                phase = Phase.energyDecrease;
                break;
            case 60:
                phase = Phase.arriveGate;
                break;
            case 80:
                phase = Phase.waitForArriveGate;
                break;
            case 70:
                phase = Phase.energyZero;
                break;
            case 90:
                phase = Phase.waitForGameOver;
                break;
            case 100:
                phase = Phase.justEncount;
                break;
            default:
                break;
        }

        //諸々、表示したいのを取得しとく。なんかこれも、効率悪そう。
        Announce_AllEnergyGet = GameObject.Find("AllEnergyGet");
        Announce_AllEnergyGet.SetActive(false);
        Announce_LeaveHere = GameObject.Find("LeaveHere");
        Announce_LeaveHere.SetActive(false);
        Announce_ArriveGate = GameObject.Find("ArriveGate");
        Announce_ArriveGate.SetActive(false);
        Announce_EnergyZero = GameObject.Find("EnergyZero");
        Announce_EnergyZero.SetActive(false);
        Announce_GameOver = GameObject.Find("GameOver");
        Announce_GameOver.SetActive(false);

        //m_shinny = new ShinyEffectForUGUI();

        //全部取った時は、総量の表示を消しておきたいと思ったので、その表示部分のオブジェクトを取得しておく
        //減るカウントとそのままのカウントがあるのがおかしいし。
        energyAllCountObj = GameObject.FindGameObjectWithTag("energyAllCount");
        //こっちは取得した数を示す。減少させるのはこっち
        CountManager = GameObject.Find("CountManager");
        variousCount = CountManager.GetComponent<VariousCount>();
        ui_energyCountHeader = GameObject.Find("energy");
        ui_energyCount = GameObject.Find("count");
        energyCount = PlayerPrefs.GetInt("Energy");


    }

    void Update() {


        //Phaseごとに展開（進行）していく感じ
        switch (phase) {
            case Phase.energyLeft:
                phaseValue = 10;
                break;
            case Phase.energyAllGet:
                GetAllEnergy();
                phaseValue = 20;
                break;
            case Phase.leaveHere:
                SetAfterEnergyGetAll();
                phaseValue = 30;
                break;
            case Phase.setArrowAndMapUI:
                SetAfterLeaveHere();
                phaseValue = 40;
                break;
            case Phase.energyDecrease:
                EnergyDecrease();
                phaseValue = 50;
                break;
            case Phase.arriveGate:
                ArriveGate();//接触判定はeffectのGateにやらせる。
                phaseValue = 60;
                break;
            case Phase.waitForArriveGate:
                WaitForNextScene("ArriveGateScene");
                phaseValue = 80;
                break;
            case Phase.energyZero:
                EnergyZero();
                phaseValue = 70;
                break;
            case Phase.waitForGameOver:
                WaitForNextScene("GameOverScene");
                phaseValue = 90;
                break;
            case Phase.justEncount:
                EncountSign();//接触判定はeffectのencountSignにやらせる
                phaseValue = 100;
                break;
            default:
                break;
        }
    }

    //エネルギーを全部取った
    public void GetAllEnergy() {
        //エネルギーの量をセーブ
        SaveEnergyCountSet();
        SaveVarious();

        //全部取ったことを知らせる文字を表示
        Announce_AllEnergyGet.SetActive(true);

        //文字を光らせたいけど、これだとヌルレファレンスでてよくわからないのでコメントアウト
        //http://baba-s.hatenablog.com/entry/2018/05/21/090000
        //m_shinny.Play();

        //全部とったことを知らせる文字表示の消去用にタイマー使う＆Phase変更
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= timeOut) {
            phase = Phase.leaveHere;
            timeElapsed = 0.0f;
            Announce_AllEnergyGet.SetActive(false);
            energyAllCountObj.SetActive(false);//エネルギー（希望）の総量を表示していたのを消す
        }

        //Gate（ゴール）をOn
        gate.enabled = true;

        //エンカウントできる場所をON
        encountSign.enabled = true;
        objectOnOff.enabled = true;

    }

    //全部取ったよ　の表示後に　出ましょう　の表示
    public void SetAfterEnergyGetAll() {

        //全部とったを消す
        Announce_AllEnergyGet.SetActive(false);

        //出よう　を表示
        if (!Announce_AllEnergyGet.activeSelf && !Announce_LeaveHere.activeSelf) {
            Announce_LeaveHere.SetActive(true);
        }

        //出よう　を画面下へ移動
        Announce_LeaveHere.transform.Translate(0, -1.0f, 0);

        //「希望」と、そのカウントを下へ移動
        ui_energyCountHeader.transform.Translate(0.1f, -1.2f, 0);
        ui_energyCount.transform.Translate(0.25f, 0, 0);
        ui_energyCountHeader.transform.localScale = new Vector3(1.5f, 1.5f, 0);


        //出よう　を知らせる文字表示の消去用にタイマー使う＆Phase変更
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeOut) {
            phase = Phase.setArrowAndMapUI;
            timeElapsed = 0.0f;
            Announce_LeaveHere.SetActive(false);
        }

    }

    //出ましょう　の動きで視線誘導して矢印とミニマップを表示する
    public void SetAfterLeaveHere() {
        //ミニマップ表示をON
        radarCam.enabled = true;

        //矢印表示をON
        DirectionArrow.SetActive(true);

        //エネルギー減少Phaseへ
        energyCount = variousCount.countAllEnergy;
        phase = Phase.energyDecrease;
    }

    //エネルギー（希望）の減少開始
    public void EnergyDecrease() {
        energyDecreaseTimeElapsed += Time.deltaTime;
        if (energyDecreaseTimeElapsed >= energyDecreaseTime) {
            energyCount --;
            variousCount.CountGetEnergyText.text = energyCount.ToString();
            if(energyCount <= 0) {
                phase = Phase.energyZero;
            }
            energyDecreaseTimeElapsed = 0.0f;
        }
    }

    //ゲートに辿り着いた！
    public void ArriveGate() {
        SaveEnergyCountSet();
        SaveVarious();

        Announce_ArriveGate.SetActive(true);
        phase = Phase.waitForArriveGate;
    }

    //エネルギー（希望）０になったー(GameOver)
    public void EnergyZero() {
        Announce_EnergyZero.SetActive(true);
        Announce_GameOver.SetActive(true);
        phase = Phase.waitForGameOver;
    }

    //戦闘にはいる
    public void EncountSign() {
        //暗転する
        if (fadeFlag == 0) {
            playerController.moveFlag = 0;
            fade.FadeIn(1);
            fadeFlag = 1;
        }

        //暗転からふっきして戦闘の画面になる→登場的なテキストを出すカメラONまで。戦闘の画面への遷移はBattleProgressでやる
        if (fadeFlag == 1 && BlackOutElapsed(blackOutTimeElapsed)) {
            fade.FadeOut(1);
            //戦闘の画面にするために・・・
            radarCam.enabled = false;//ミニマップ表示用のカメラをオフ
            playerCamera.enabled = false;//プレイヤーの主観カメラをオフ
            mainGameCanvas.enabled = false;//移動用のボタンとか矢印とかを表示しているCanvasを非表示

            enemyNameCam.enabled = true;//エンカウントのフェード演出後に「登場！」的な文字を表示

            battleCam.enabled = false;//バトル時の画面は登場的な文字のあとにしたいのでカメラおふっとく。
            fadeFlag = 2;
        }
    }

    //---------------------------------------------------------

    //セーブ
    public void SavePhaseValueSet() {
        PlayerPrefs.SetInt("Phase", phaseValue);
    }
    public void SaveEnergyCountSet() {
        PlayerPrefs.SetInt("Energy", energyCount);
    }

    public void SaveVarious() {
        PlayerPrefs.Save();
    }

    //セーブデータを全て削除
    public void DeleteSaveData() {
        PlayerPrefs.DeleteAll();
    }

    //タッチで次のシーンへ遷移　Wait用
    public void WaitForNextScene(string SceneName) {
        if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(LoadScene(SceneName));
        }
    }
    IEnumerator LoadScene(string name02) {
        AsyncOperation async = SceneManager.LoadSceneAsync(name02, LoadSceneMode.Single);
        yield return async;
    }

    //消して暗転してboolを返す。フェードアウトして暗転している時間のウェイト用関数
    public bool BlackOutElapsed(float t) {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= t) {
            return true;
        }
        return false;
    }

}
