using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour {

    Enemy.Param enemyParam;

    public enum Phase {
        create,
        end
    }

    public Phase phase;

    public int exitTimes;//迷路のGateに入った回数をもとに敵の強さを作るようにすっか?GameProgressでこれにあたるやつをつくっといて参照するのがいいかな?


	void Start () {

        phase = Phase.create;

        exitTimes = 1;
	}

	void Update () {
        switch (phase) {
            case Phase.create:
                enemyParam.set_Lv(LevelConfirm(exitTimes));
                enemyParam.set_Hp(HpConfirm(exitTimes));
                enemyParam.set_Atk(AtkConfirm(exitTimes));
                enemyParam.set_Def(DefConfirm(exitTimes));
                enemyParam.set_TimeTillAttack(TimeTillAtkConfirm((float)exitTimes));
                enemyParam.set_DefeatNumber(DefeatNumberConfirm());
                phase = Phase.end;
                break;
            case Phase.end:
                break;
            default:
                break;
        }
	}

    public int LevelConfirm(int something) {
        int level = Random.Range(something, something * 2);
        return level;
    }

    public int HpConfirm(int something) {
        int hp = something * 7;
        return hp;
    }

    public int AtkConfirm(int something) {
        int atk = something * 10 + 1;
        return atk;
    }

    public int DefConfirm(int something) {
        int def = something * 10 + 2;
        return def;
    }

    public float TimeTillAtkConfirm(float something) {
        float t = something;
        return t;
    }

    public int[] DefeatNumberConfirm() {
        int[] defeatNum = new  int[4];
        defeatNum[0] = 0;
        defeatNum[1] = Random.Range(1, 9);

        do {
            defeatNum[2] = Random.Range(1, 9);
        } while (defeatNum[1] == defeatNum[2]);

        do {
            defeatNum[3] = Random.Range(1, 9);
        } while (defeatNum[1] == defeatNum[3] || defeatNum[2] == defeatNum[3]);

        return defeatNum;
    }
}
