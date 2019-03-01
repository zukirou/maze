using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
・width幅（x）、deph奥行き（z）の数値を以って地面を作る
・xとzは５以上
・１は１mだが、planeの1は10m　cubeの1は1m
・0.2×0.2のplaneメッシュを並べて地面を作る
・壁は２×２のcubeメッシュにする

・１マスは2m×2mとする
・１マスに壁を置いて迷路をつくる
　・偶数座標のマスが壁作成の開始候補マスになる
　　・よって、ダンジョンの最大サイズの数値は奇数にすること
　　・そうしないと「閉じた空間」ができてしまうので。。。
　・何も無い空間は2m×2mの通路
 　　・何も無い空間の集合体、「部屋」は、あとで考えよう
・原点はワールド座標 0, 0, 0
*/

public class MapGenerate : MonoBehaviour {
    [SerializeField] Ground ground;//床
    [SerializeField] Wall wall;//壁
    public int width;//planeをx方向に何枚並べるか
    public int depth;//planeをz方向に何枚並べるか

    public int[,] Dungeon;
    public int Path = 0;
    public int Wall = 1;

    public struct Cell {
        public int x;
        public int z;
        public Cell(int x, int z) {
            this.x = x;
            this.z = z;
        }
    }

    private enum WallDirection {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    private Stack<Cell> CurrentWallCells;//現在作成中の壁情報の保持
    private List<Cell> StartCells;//壁を作成開始する開始セル（場所）の情報の保持

	void Start () {
        width = 5;
        depth = 5;

        Dungeon = new int[width, depth];

        StartCells = new List<Cell>();
        CurrentWallCells = new Stack<Cell>();

        Dungeon = CreatGroundAndWall();
        CreateDungeon();

        //createGround(width, depth, groundTileNum);
        //createOuterWall(width, depth, outerWallsNums);
    }


    public void CreateDungeon() {
        for (int zAx = 0; zAx < this.Dungeon.GetLength(1); zAx++) {
            for (int xAx = 0; xAx < this.Dungeon.GetLength(0); xAx++) {
                if (this.Dungeon[xAx, zAx] == Wall) {
                    Instantiate(wall, new Vector3(xAx * 2, 0, zAx * 2), Quaternion.identity);
                }
                if (this.Dungeon[xAx, zAx] == Path) {
                    Instantiate(ground, new Vector3(xAx * 2, 0, zAx * 2), Quaternion.identity);
                }
            }
        }
    }

    public int[,] CreatGroundAndWall() {
        //各マスの初期設定
        for (int z = 0; z < this.depth; z++) {
            for(int x = 0; x < this.width; x++) {
                //外周を壁にしておく。それ以外を開始候補（壁作成開始地点）として保持する
                if (x == 0 || z == 0 || x == this.width - 1 || z == this.depth - 1) {
                    this.Dungeon[x, z] = Wall;
                }
                else {
                    this.Dungeon[x, z] = Path;
                    //外周ではない　偶数座標　を　壁作り開始点候補　とする
                    if (x % 2 == 0 && z % 2 == 0) {
                        //開始候補の座標
                        StartCells.Add(new Cell(x, z));
                    }
                }
            }
        }
        //壁が拡張できなくなるまで（開始候補がなくなるまで）ループ
        while(StartCells.Count > 0) {
            //ランダムに開始セルを取得して、それが再び取得されないように開始候補から削除
            var index = UnityEngine.Random.Range(0, StartCells.Count);
            var cell = StartCells[index];
            StartCells.RemoveAt(index);
            var x = cell.x;
            var z = cell.z;

            //すでに壁の場合は何もしない。そこには壁があるのか無いのか
            if(this.Dungeon[x, z] == Path) {
                //作成中の壁情報を初期化そして作成
                CurrentWallCells.Clear();
                ExtendWall(x, z);
            }
        }
        return this.Dungeon;
    }

    //指定座標から壁を作る
    private void ExtendWall(int x, int z) {
        //壁を伸ばせる方向（１マス先が通路で２マス先まで範囲内）
        //２マス先が壁もしくは、いま作成中の場合（伸びてる自分自身）のときは伸ばせない
        var directions = new List<WallDirection>();

        if (this.Dungeon[x, z - 1] == Path && !IsCurrentWall(x, z - 2)) {
            directions.Add(WallDirection.Up);
        }
        if (this.Dungeon[x + 1, z] == Path && !IsCurrentWall(x + 2, z)) {
            directions.Add(WallDirection.Right);
        }
        if (this.Dungeon[x, z + 1] == Path && !IsCurrentWall(x, z + 2)) {
            directions.Add(WallDirection.Down);
        }
        if (this.Dungeon[x - 1, z] == Path && !IsCurrentWall(x - 2, z)) {
            directions.Add(WallDirection.Left);
        }

        //ランダムに作る（のばす）
        if (directions.Count > 0) {
            //壁をつくる
            SetWall(x, z);

            //つくる先が空白（通路：Path）の場合は作り続ける
            var isPath = false;
            var dirIndex = UnityEngine.Random.Range(0, directions.Count);
            switch (directions[dirIndex]) {
                case WallDirection.Up:
                    isPath = (this.Dungeon[x, z - 2] == Path);
                    SetWall(x, --z);
                    SetWall(x, --z);
                    break;
                case WallDirection.Right:
                    isPath = (this.Dungeon[x + 2, z] == Path);
                    SetWall(++x, z);
                    SetWall(++x, z);
                    break;
                case WallDirection.Down:
                    isPath = (this.Dungeon[x, z + 2] == Path);
                    SetWall(x, ++z);
                    SetWall(x, ++z);
                    break;
                case WallDirection.Left:
                    isPath = (this.Dungeon[x - 2, z] == Path);
                    SetWall(--x, z);
                    SetWall(--x, z);
                    break;
            }
            if (isPath) {
                //既存の壁に接続していない場合は作るの続行
                ExtendWall(x, z);
            }
        }
        else {
            //すべて現在作成中の壁にぶつかる場合は、もどって再開
            var beforeCell = CurrentWallCells.Pop();
            ExtendWall(beforeCell.x, beforeCell.z);
        }
    }

    //壁をつくる
    private void SetWall(int x, int z) {
        this.Dungeon[x, z] = Wall;
        if (x % 2 == 0 && z % 2 == 0) {
            CurrentWallCells.Push(new Cell(x, z));
        }
    }

    //作成中の壁（自分自身）かどうかを判定
    private bool IsCurrentWall(int x, int z) {
        return CurrentWallCells.Contains(new Cell(x, z));
    }

}
