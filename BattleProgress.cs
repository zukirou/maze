using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleProgress : MonoBehaviour {

    public enum Phase {
        tellEncountEnemy,
        readyChangeBattleCamera,
        changeBattleCamera,
        battleStart,
        battle_inputOrderFirst,
        battle_inputOrderSecond,
        battle_inputOrderThird,
        batlle_inputOrderEnd,
        end
    }

    public Phase phase;

    EnemyCreate enemyCreate;
    Enemy.Param enemyParam;
    Enemy enemy;
    GameObject[] enemyAttackEffect;

    public int enemyLvBase;

    public int[] getEnergyID;
    public int[] inputEnergyID;
    public int correctOrder;

    int onceFlag = 0;
    GameObject ProgressManager;
    GameProgress gameProgress;
    GameObject BattleCamera;
    Camera battleCamera;
    GameObject EnemyNameCamera;
    Camera enemyNameCamera;

    GameObject enemyHpGauge;
    GameObject enemyTimeTillAtk;

    [SerializeField]
    private TextMeshProUGUI playerEnergyAmount;

    private TextMeshProUGUI enemyName;

    [SerializeField]
    Fade fade = null;// http://tsubakit1.hateblo.jp/entry/2015/11/04/015355

    public float ellapsedTime;
    float time;

    GameObject battleGameCanvas;
    GameObject battleGameEnergyCanvas;
    MouseInput battleGameEnergyCanvas_mouseInput;
    GameObject[] battleGameEnergy;
    MeshRenderer[] battleGameEnergyMaterial;
    public Material nomalDamage;
    public Material middleDamage_Red;
    public Material middleDamage_Green;
    public Material middleDamage_Blue;
    public Material defaultMaterial;
    //BigDamageの時は、その３つのsphereをレインボーにしたいな。


    void Start () {
        enemyAttackEffect = new GameObject[10];

        inputEnergyID = new int[4];
        getEnergyID = new int[4];
        correctOrder = 0;
        battleGameEnergy = new GameObject[10];
        battleGameEnergyMaterial = new MeshRenderer[10];

        for(int i = 0; i > 4; i++) {
            inputEnergyID[i] = 0;
            getEnergyID[i] = 0;
        }

        battleGameCanvas = GameObject.Find ("Canvas_battleGame");
        battleGameEnergyCanvas = GameObject.Find("Canvas_battleGameEnergy");
        battleGameEnergyCanvas_mouseInput = battleGameEnergyCanvas.GetComponent<MouseInput>();

        battleGameEnergy[1] = GameObject.FindGameObjectWithTag("touchEnergy1");
        battleGameEnergy[2] = GameObject.FindGameObjectWithTag("touchEnergy2");
        battleGameEnergy[3] = GameObject.FindGameObjectWithTag("touchEnergy3");
        battleGameEnergy[4] = GameObject.FindGameObjectWithTag("touchEnergy4");
        battleGameEnergy[5] = GameObject.FindGameObjectWithTag("touchEnergy5");
        battleGameEnergy[6] = GameObject.FindGameObjectWithTag("touchEnergy6");
        battleGameEnergy[7] = GameObject.FindGameObjectWithTag("touchEnergy7");
        battleGameEnergy[8] = GameObject.FindGameObjectWithTag("touchEnergy8");
        battleGameEnergy[9] = GameObject.FindGameObjectWithTag("touchEnergy9");

        battleGameEnergyMaterial[1] = battleGameEnergy[1].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[2] = battleGameEnergy[2].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[3] = battleGameEnergy[3].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[4] = battleGameEnergy[4].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[5] = battleGameEnergy[5].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[6] = battleGameEnergy[6].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[7] = battleGameEnergy[7].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[8] = battleGameEnergy[8].GetComponent<MeshRenderer>();
        battleGameEnergyMaterial[9] = battleGameEnergy[9].GetComponent<MeshRenderer>();

        ProgressManager = GameObject.Find("ProgressManager");
        gameProgress = ProgressManager.GetComponent<GameProgress>();

        BattleCamera = GameObject.Find("BattleCamera");
        battleCamera = BattleCamera.GetComponent<Camera>();

        EnemyNameCamera = GameObject.Find("EnemyNameCamera");
        enemyNameCamera = EnemyNameCamera.GetComponent<Camera>();

        playerEnergyAmount = GameObject.Find("Canvas_battleGameEnergyAmount/energyAmountText").GetComponent<TextMeshProUGUI>();

        enemyName = GameObject.Find("Canvas_enemyName/enemyName").GetComponent<TextMeshProUGUI>();

        enemyCreate = new EnemyCreate();
        //絶望のノーマル攻撃エフェクトを取得
        enemyAttackEffect[1] = GameObject.Find("EnemyNormalAttack");//Objectとしてむりくり置いて、名前で強引に呼び出し
        enemyAttackEffect[1].SetActive(false);//このやり方だとエフェクトの種類ごとに取得しなきゃいけないし、さらに無駄が多そうだー

        phase = Phase.tellEncountEnemy;

        fade = GameObject.Find("FadeCanvas_enemyName2BtlScene").GetComponent<Fade>();

        enemyHpGauge = GameObject.Find("EnemyHpFill");
        enemyTimeTillAtk = GameObject.Find("EnemyTimeFill");
        enemyTimeTillAtk.SetActive(false);

        time = 0;
        ellapsedTime = 2;
    }
	
	void Update () {

        switch (phase) {
            case Phase.tellEncountEnemy:
                if (enemyNameCamera.enabled) {
                    TellEncountEnemy(enemyCreate.LevelConfirm(2));//絶望のレベルはここで設定
                    phase = Phase.readyChangeBattleCamera;
                }
                break;
            case Phase.readyChangeBattleCamera:
                time += Time.deltaTime;
                if (time > ellapsedTime) {
                    fade.FadeIn(0.5f);
                    time = 0;
                    phase = Phase.changeBattleCamera;
                }
                break;
            case Phase.changeBattleCamera:
                time += Time.deltaTime;
                if(time > ellapsedTime - 1.5) {
                    fade.FadeOut(0.5f);
                    enemyNameCamera.enabled = false;
                    battleCamera.enabled = true;
                    time = 0;
                    phase = Phase.battleStart;
                }
                break;
            case Phase.battleStart://直前準備。プレイヤーとか絶望のパラメータセットとか

                //プレイヤーがバトルで使えるエネルギー量を更新
                if (battleCamera.enabled) {
                    onceFlag = 1;
                    if (onceFlag == 1) {
                        playerEnergyAmount.text = gameProgress.energyCount.ToString();
                        onceFlag = 2;
                    }
                }
                if (!battleCamera.enabled) {
                    onceFlag = 0;
                }

                //絶望のパラメータセット
                //Hp（これを０にしたら勝利）
                enemyParam.set_Hp(enemyCreate.HpConfirm(enemyParam.Lv));
                //Atk（プレイヤーの被ダメ）
                enemyParam.set_Atk(enemyCreate.AtkConfirm(enemyParam.Lv));
                //Def（プレイヤーの与ダメを軽減）
                enemyParam.set_Def(enemyCreate.DefConfirm(enemyParam.Lv));
                //DefeatNumber（一番大きいプレイヤーの与ダメになるやつ）
                enemyParam.set_DefeatNumber(enemyCreate.DefeatNumberConfirm());
                print("DefeatNumber[1]=>" + enemyParam.DefeatNumber[1]);//チェック用のプリント
                print("DefeatNumber[2]=>" + enemyParam.DefeatNumber[2]);//チェック用のプリント
                print("DefeatNumber[3]=>" + enemyParam.DefeatNumber[3]);//チェック用のプリント

                //TimeTillAttack（この時間が経過するたびにプレイヤーを攻撃＆DefeatNumber更新&これが切れるときのタイミングで入力あると被ダメ軽減か０)
                enemyParam.set_TimeTillAttack(5.0f);

                //戦闘時の「希望」を押せるようにする（ずっとtrueにしてたら、バトル入る前から押してることになってたのでメインでfalseにしている）
                battleGameEnergyCanvas_mouseInput.enabled = true;

                phase = Phase.battle_inputOrderFirst;
                break;
            case Phase.battle_inputOrderFirst:
                //　1/3つ目を保持する
                getEnergyID[1] = battleGameEnergyCanvas_mouseInput.GetEnergyID();
                inputEnergyID[1] = getEnergyID[1];

                if (inputEnergyID[1] > 0) {
                    //押された希望がDefeatNumberのどれかかどうかでマテリアルを変える
                    DefeatNumberCheckAndChangeMaterial(inputEnergyID[1]);
                    //押された希望が正しいDefeatNumberの順番かどうかチェック
                    if(inputEnergyID[1] == enemyParam.DefeatNumber[1]) {
                        correctOrder += 1;
                    }
                    phase = Phase.battle_inputOrderSecond;
                }
                break;
            case Phase.battle_inputOrderSecond:
                //　2/3つ目を保持する
                getEnergyID[2] = battleGameEnergyCanvas_mouseInput.GetEnergyID();
                if (inputEnergyID[1] != getEnergyID[2] && getEnergyID[2] > 0) {
                    inputEnergyID[2] = getEnergyID[2];
                    //押された希望がDefeatNumberのどれかかどうかでマテリアルを変える
                    DefeatNumberCheckAndChangeMaterial(inputEnergyID[2]);
                    //押された希望が正しいDefeatNumberの順番かどうかチェック
                    if (inputEnergyID[2] == enemyParam.DefeatNumber[2]) {
                        correctOrder += 1;
                    }
                    phase = Phase.battle_inputOrderThird;
                }
                break;
            case Phase.battle_inputOrderThird:
                //　3/3つ目を保持する
                getEnergyID[3] = battleGameEnergyCanvas_mouseInput.GetEnergyID();
                if (inputEnergyID[2] != getEnergyID[3] && inputEnergyID[1] != getEnergyID[3] && getEnergyID[3] > 0) {
                    inputEnergyID[3] = getEnergyID[3];
                    //押された希望がDefeatNumberのどれかかどうかでマテリアルを変える
                    DefeatNumberCheckAndChangeMaterial(inputEnergyID[3]);
                    //押された希望が正しいDefeatNumberの順番かどうかチェック
                    if (inputEnergyID[3] == enemyParam.DefeatNumber[3]) {
                        correctOrder += 1;
                    }
                    phase = Phase.batlle_inputOrderEnd;
                }
                break;
            case Phase.batlle_inputOrderEnd:
                if(correctOrder == 3) {
                    correctOrder = 0;
                    print("ただしい順番で希望をおしたぞ！correctOrder=>" + correctOrder);
                    DecreaseEnemyHpBigDamage();
                }
                else {
                    correctOrder = 0;
                    print("correctOrder=>" + correctOrder);
                }
                time += Time.deltaTime;
                if (time > ellapsedTime) {
                    time = 0;
                    for (int i = 1; i < 4; i++) {
                        //希望のマテリアルを全部元にもどしつつ、入力されたIDをリセット
                        battleGameEnergyMaterial[inputEnergyID[i]].material = defaultMaterial;
                        inputEnergyID[i] = 0;
                        getEnergyID[i] = 0;
                        battleGameEnergyCanvas_mouseInput.ResetEnergyID();
                    }                    
                    phase = Phase.battle_inputOrderFirst;
                }

                break;
            case Phase.end:

                break;
            default:
                break;
        }

        enemyTimeTillAtk.SetActive(true);
        DecreaseEnemyTimeTillAtk();

    }

    public void TellEncountEnemy(int num) {
        enemyName.text = "レベル" + num.ToString();
        enemyParam.set_Lv(num);
    }

    //敵が攻撃してくるまでのタイマー。０で攻撃＆タイマーをリセット
    public void DecreaseEnemyTimeTillAtk() {
        float timeDecrease = enemyParam.get_TimeTillAttack() / 600;
        enemyTimeTillAtk.GetComponent<Image>().fillAmount -= timeDecrease;

        if(enemyTimeTillAtk.GetComponent<Image>().fillAmount <= 0) {
            ZetubouAttack();
            DamageDecreaseKibou();
            enemyTimeTillAtk.GetComponent<Image>().fillAmount = 1;

            //タイマーのリセットのタイミングで DefeatNumber　を更新する
            enemyParam.set_DefeatNumber(enemyCreate.DefeatNumberConfirm());
            print("defeatNum[1]" + enemyParam.DefeatNumber[1]);
            print("defeatNum[2]" + enemyParam.DefeatNumber[2]);
            print("defeatNum[3]" + enemyParam.DefeatNumber[3]);
        }
    }

    //その希望がDefeatNumberの何番目になるのかを知らせる
    //Normal,Middle与ダメージはここで入れとく
    public void DefeatNumberCheckAndChangeMaterial(int energyID) {
        if (enemyParam.DefeatNumber[1] == energyID) {
            battleGameEnergyMaterial[energyID].material = middleDamage_Red;
            DecreaseEnemyHpMiddleDamage();
        }else if (enemyParam.DefeatNumber[2] == energyID) {
            battleGameEnergyMaterial[energyID].material = middleDamage_Green;
            DecreaseEnemyHpMiddleDamage();
        }else if (enemyParam.DefeatNumber[3] == energyID) {
            battleGameEnergyMaterial[energyID].material = middleDamage_Blue;
            DecreaseEnemyHpMiddleDamage();
        }else{
            battleGameEnergyMaterial[energyID].material = nomalDamage;
            DecreaseEnemyHpNormalDamage();
        }
    }

    public void DecreaseEnemyHpNormalDamage() {
        enemyHpGauge.GetComponent<Image>().fillAmount -= 0.01f;
    }

    public void DecreaseEnemyHpMiddleDamage() {
        enemyHpGauge.GetComponent<Image>().fillAmount -= 0.05f;
    }

    public void DecreaseEnemyHpBigDamage() {
        enemyHpGauge.GetComponent<Image>().fillAmount -= 0.2f;
    }

    public void ZetubouAttack() {
        //敵の攻撃
        //エフェクト発生
        enemyAttackEffect[1].SetActive(true);//ノーマル攻撃
        //プレイヤーへダメージ
        DamageDecreaseKibou();
    }

    public void DamageDecreaseKibou() {
        //敵の攻撃によるプレイヤーの希望を減らす
        gameProgress.energyCount = gameProgress.energyCount - 3;
        //プレイヤーの希望の数を更新
        playerEnergyAmount.text = gameProgress.energyCount.ToString();
    }
}