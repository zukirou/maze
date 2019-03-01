using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

    public ParticleSystem AttackParticle;

    public struct Param {

        public string Name;
        public int Lv;
        public int Hp;
        public int Atk;
        public int Def;
        public int[] DefeatNumber;//大きめの与ダメージ与えられる数字
        public float TimeTillAttack;//攻撃を行うまでの時間（タイマー０で攻撃する）

        public void set_Name(string n) {
            Name = n;
        }
        public string get_Name() {
            return Name;
        }

        public void set_Lv(int var) {
            Lv = var;
        }
        public int get_Lv() {
            return Lv;
        }

        public void set_Hp(int var) {
            Hp = var;
        }
        public int get_Hp() {
            return Hp;
        }

        public void set_Atk(int var) {
            Atk = var;
        }
        public int get_Atk() {
            return Atk;
        }

        public void set_Def(int var) {
            Def = var;
        }
        public int get_Def() {
            return Def;
        }

        public void set_DefeatNumber(int[] num) {
            DefeatNumber = num;
        }
        public int[] get_DefeatNumber() {
            return DefeatNumber;
        }

        public void set_TimeTillAttack(float timeTillAtk) {
            TimeTillAttack = timeTillAtk;
        }
        public float get_TimeTillAttack() {
            return TimeTillAttack;
        }

    }


    void Start () {
	}
	
	void Update () {

    }
}
