using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager 
{
    public int enemyCount;
    public int playerCount;
    public int waveNumber;
    public int lifePoint = 3;
    public int cameraPriority = 15;
    public int heavyEnemyCount;
    //ゲーム中判定
    public bool isGame = false;
    //生成時判定、多重阻止用
    public bool isGenerate;
    //リスタート用判定
    public bool isReset = false;
　　//プレイヤーの場外判定
    public bool isOut;
    //ウェーブ中判定
    public bool isPlay;
    //ダッシュ回復用判定
    public bool isHeal;
    //説明中判定
    public bool isExplain;
    //説明中判定
    public bool isMenuExplain;
    //全ウェーブクリア判定
    public bool isClear;
    public bool isConfig;
    private static InfoManager _instance;
    public static InfoManager Instance
    {
        get
        {
            if (_instance == null) _instance = new InfoManager();
            return _instance;
        }
    }
    private InfoManager() { }
}
