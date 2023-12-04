using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    InfoManager im;
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject inGameCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject playerPrefabs;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject heavyEnemyPrefab;
    [SerializeField] GameObject BossPrefab;
    [SerializeField] GameObject playerSpawner;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject bigWaveObject;
    [SerializeField] GameObject curbObject;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject explainPanel;
    [SerializeField] GameObject menuExplainPanel;
    [SerializeField] GameObject menuObjects;
    [SerializeField] GameObject popObject;
    [SerializeField] GameObject titleObjects;
    [SerializeField] GameObject audioPanel;
    [SerializeField] Text enemiesText;
    [SerializeField] Text waveText;
    [SerializeField] Text bigWaveText;
    [SerializeField] Text lifeText;
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip startSound;
    [SerializeField] AudioClip titleBGM;
    [SerializeField] AudioClip battleBGM;
    [SerializeField] AudioClip BossBGM;
    [SerializeField] AudioClip ClearJingle;
    [SerializeField] AudioClip OverJingle;
    // Start is called before the first frame update
    private void Start()
    {
        im = InfoManager.Instance;
        waveText.text = "Wave:"+ im.waveNumber;
        audioSource.clip = titleBGM;
        audioSource.Play();
        if(audioSource.loop == false)
        {
            audioSource.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの数を取得
        im.playerCount = FindObjectsOfType<PlayerController>().Length;
        //ゲーム中の処理
        if (im.isGame)
        {
            //敵の数を取得
            im.enemyCount = FindObjectsOfType<EnemyController>().Length;
            enemiesText.text = "のこり" + im.enemyCount + "体";
            //生成中ではないかつ敵がいないときの処理
            if (im.enemyCount == 0 && im.isGenerate == false && im.waveNumber <= 5)
            {
                //ウェーブ数を進める
                im.waveNumber++;
                //ウェーブ数を表示
                bigWaveText.gameObject.SetActive(true);
                //ウェーブ数の表示を更新
                bigWaveText.text = "Wave " + im.waveNumber;
                //生成中判定にする
                im.isGenerate = true;
                im.isPlay = false;
                enemiesText.gameObject.SetActive(false);
                waveText.gameObject.SetActive(false);
                //ウェーブ数の表示を拡大・縮小する
                TextExpansion(bigWaveObject);
                //三秒後に次のウェーブを始める
                StartCoroutine(WaveCoroutine(3));        
            }
            else if (im.enemyCount == 0 && im.isGenerate == false && im.waveNumber >= 6)
            {
                //ウェーブ数を進める
                im.waveNumber++;
                im.isPlay = false;
                enemiesText.gameObject.SetActive(false);
                waveText.gameObject.SetActive(false);
                NextWave();
            }
            //プレイヤーが存在しないとき
            if (im.playerCount == 0)
            {
                //プレイヤーを生成
                SpawnPlayer();
            }
            //残機が0になったとき
            if (im.lifePoint==0)
            {
                //ゲーム中判定じゃなくする
                im.isGame = false;
                //ゲーム中のキャンバスを非表示にする
                inGameCanvas.SetActive(false);
                //ゲームオーバー時のキャンバスを表示する
                gameOverPanel.SetActive(true);
                audioSource.Stop();
                audioSource.PlayOneShot(OverJingle);
                audioSource.loop = false;
            }
        }
        //ゲーム中かつメニューが表示されてなく、Escapeが押されたら
        if (Input.GetKeyDown(KeyCode.Escape) && im.isGame)
        {
            //時間の進みを止める
            Time.timeScale = 0;
            //ゲーム中判定じゃなくする
            im.isGame = false;
            //メニューのキャンバスを表示
            menuCanvas.SetActive(true);
        }
        else
        //ゲーム中でないかつメニューが表示されており、Escapeが押されたら
        if (Input.GetKeyDown(KeyCode.Escape) && im.isGame ==false &&
            startCanvas.activeSelf == false)
        {
            //時間の進みを再開する
            Time.timeScale = 1;
            //ゲーム中判定にする
            im.isGame = true;
            //メニューのキャンバスを非表示
            menuCanvas.SetActive(false);
        }
        if (Input.GetMouseButton(0) && im.isExplain == true)
        {
            explainPanel.SetActive(false);
            titleObjects.SetActive(true);
            im.isExplain = false;
        }
        if (Input.GetMouseButton(0) && im.isMenuExplain == true)
        {
            menuExplainPanel.SetActive(false);
            menuObjects.SetActive(true);
            inGameCanvas.SetActive(true);
            im.isMenuExplain = false;
        }
    }
    /// <summary>
    /// テキストの拡大縮小を行う
    /// </summary>
    public static void TextExpansion(GameObject go)
    {
        //1.5秒かけて等倍表示し、その後1.5秒かけてオブジェクトを0の極限に近づける
        go.transform.DOScale(new Vector3(1, 1, 1), 1.5f).SetEase(Ease.OutQuad).
            OnComplete(() => { go.transform.DOScale(new Vector3(0, 0, 0), 1.5f).
                SetEase(Ease.OutQuad); });
    }
    /// <summary>
    /// 次のウェーブを開始する処理
    /// </summary>
    private void NextWave()
    {
        if (im.waveNumber > 0)
        {
            enemiesText.gameObject.SetActive(true);
            //目標の表示
            enemiesText.text = "のこり" + im.enemyCount + "体";
            waveText.gameObject.SetActive(true);
        }
        im.isPlay = true;
        im.isHeal = true;
        //Debug.Log(waveNumber);
        //第4ウェーブまでの時
        if (im.waveNumber < 4 && im.isGame)
        {
            //敵を生成
            SpawnEnemy(im.waveNumber);
            //ウェーブ数の表示を変更
            waveText.text = "Wave:" + im.waveNumber;
        }
        else if (im.waveNumber >= 4 && im.waveNumber < 6 && im.isGame)
        {
            im.heavyEnemyCount++;
            SpawnEnemyHeavy(im.heavyEnemyCount);
            //ウェーブ数の表示を変更
            waveText.text = "Wave:" + im.waveNumber;
        }
        else if(im.waveNumber >= 6 && im.waveNumber < 7 && im.isGame)
        {
            SpawnBossEnemy();
            //ウェーブ数の表示を変更
            waveText.text = "Wave:" + im.waveNumber;
        }
        else if (im.waveNumber >= 7 && im.isGame)
        {
            GameClear();
            audioSource.Stop();
            audioSource.PlayOneShot(ClearJingle);
            audioSource.loop = false;
        }
    }
    /// <summary>
    /// ボタンを押されたときメニューを閉じる処理
    /// </summary>
    public void CloseMenuCanvas()
    {
        //時間の進みを再開する
        Time.timeScale = 1;
        //ゲーム中判定にする
        im.isGame = true;
        //メニューのキャンバスを非表示
        menuCanvas.SetActive(false);
    }
    /// <summary>
    /// ゲームを始める処理
    /// </summary>
    public void StartGame(float sec)
    {
        //im.waveNumber = 0;
        //リスタート状態を終える
        im.isReset = false;
        im.lifePoint = 3;
        lifeText.text = "×" + im.lifePoint;
        //4秒後にゲームを始める
        StartCoroutine(WaitStartCoroutine(sec));
        //スタート画面のキャンバスを非表示にする
        startCanvas.SetActive(false);
        //ゲーム内のキャンバスを表示する
        inGameCanvas.SetActive(true);
        popObject.SetActive(true);
        audioSource.Stop();
        audioSource.PlayOneShot(startSound);
    }
    /// <summary>
    /// カウントされた後の処理
    /// </summary>
    public void StartPlay()
    {
        //ゲーム中判定にする
        im.isGame = true;
        //
        im.isReset = false;
        //プレイヤーを生成する
        SpawnPlayer();
        //敵を生成する
        NextWave();
        audioSource.clip = battleBGM;
        audioSource.Play();
    }
    /// <summary>
    /// リスタートする処理
    /// </summary>
    public void ReStart()
    {
        if (im.waveNumber <= 4) im.heavyEnemyCount = 0;
        if (im.waveNumber >= 5) im.heavyEnemyCount = 1;
        StartGame(4);
        CloseMenuCanvas();
        im.isReset = true;
        im.isGame = false;
        im.isClear = false;
        im.lifePoint = 3;
        lifeText.text = "×" + im.lifePoint;
        popObject.SetActive(false);
        popObject.SetActive(true);
    }
    public void Continue()
    {
        if (im.waveNumber == 4) im.heavyEnemyCount = 0;
        if (im.waveNumber == 5) im.heavyEnemyCount = 1;
        StartGame(4);
        gameOverPanel.SetActive(false);
        inGameCanvas.SetActive(true);
        enemiesText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);
        im.isReset = true;
        im.isGame = false;
        im.isClear = false;
        im.lifePoint = 3;
        lifeText.text = "×" + im.lifePoint;
        im.cameraPriority++;
        vcam.Priority = im.cameraPriority;
    }
    /// <summary>
    /// スタート画面に戻るときの処理
    /// </summary>
    public void GameRestart()
    {
        CloseMenuCanvas();
        inGameCanvas.SetActive(false);
        gameOverPanel.SetActive(false);
        startCanvas.SetActive(true);
        enemiesText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);
        popObject.SetActive(false);
        im.waveNumber = 0;
        im.heavyEnemyCount = 0;
        im.isGame = false;
        im.isClear = false;
        im.isReset = true;
        im.cameraPriority++;
        vcam.Priority = im.cameraPriority;
        audioSource.clip = titleBGM;
        audioSource.Play();
        audioSource.loop = true;
    }
    public void Explain()
    {
        explainPanel.SetActive(true);
        titleObjects.SetActive(false);
        im.isExplain = true;
    }
    public void MenuExplain()
    {
        menuExplainPanel.SetActive(true);
        menuObjects.SetActive(false);
        bigWaveText.gameObject.SetActive(false);

        inGameCanvas.SetActive(false);
        im.isMenuExplain = true;
    }
    public void SoundConfig()
    {
        audioPanel.SetActive(true);
        titleObjects.SetActive(false);
        im.isConfig = true;
    }
    public void CloseSoundConfig()
    {
        audioPanel.SetActive(false);
        titleObjects.SetActive(true);
        im.isConfig = false;
    }
    public void WaveChange()
    {
        im.waveNumber = 6;
    }
    /// <summary>
    /// プレイヤーを生成する処理
    /// </summary>
    public void SpawnPlayer()
    {
        //ステージの上空の手前側にプレイヤーを生成
        Instantiate(playerPrefabs, playerSpawner.transform.position, 
            playerSpawner.transform.rotation);
    }
    /// <summary>
    /// 敵を生成する処理
    /// </summary>
    /// <param name="enemiesNum">生成する敵の数</param>
    public void SpawnEnemy(int enemiesNum)
    {
        //敵を引数分まで生成
        for (int i = 0; i < enemiesNum; i++)
        {
            //敵をステージ上かつ、ｙ軸平面のランダムな位置に生成する
            Instantiate(enemyPrefab, new Vector3(enemySpawner.transform.position.x
                +Random.Range(-7, 7),enemySpawner.transform.position.y,
                enemySpawner.transform.position.z+Random.Range(-7, 7)),
                transform.rotation);
        }
        //生成判定を終える
        im.isGenerate = false;
    }
    public void SpawnEnemyHeavy(int enemiesNum)
    {
        //敵を引数分まで生成
        for (int i = 0; i < enemiesNum; i++)
        {
            //敵をステージ上かつ、ｙ軸平面のランダムな位置に生成する
            Instantiate(heavyEnemyPrefab, new Vector3(enemySpawner.transform.position.x
                + Random.Range(-7, 7),enemySpawner.transform.position.y,
                enemySpawner.transform.position.z + Random.Range(-7, 7)),
                transform.rotation);
        }
        //生成判定を終える
        im.isGenerate = false;
    }
    public void SpawnBossEnemy()
    {
        Instantiate(BossPrefab, new Vector3(enemySpawner.transform.position.x,
            enemySpawner.transform.position.y, enemySpawner.transform.position.z),
            transform.rotation);
        //生成判定を終える
        im.isGenerate = false;
        audioSource.clip = BossBGM;
        audioSource.Play();
    }
    public void GameClear()
    {
        im.isClear = true;
        inGameCanvas.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    /// <summary>
    /// 次のウェーブに移行する処理
    /// </summary>
    /// <param name="sec">処理を止める秒数</param>
    /// <returns></returns>
    IEnumerator WaveCoroutine(float sec)
    {
        yield return new WaitForSeconds(sec);
        NextWave();
    }
    /// <summary>
    /// スタート時にカウントを待ってから始める処理
    /// </summary>
    /// <param name="sec">待つ秒数</param>
    /// <returns></returns>
    IEnumerator WaitStartCoroutine(float sec)
    {
        yield return new WaitForSeconds(sec);
        StartPlay();
        //ゲーム中判定にする
        im.isGame = true;
    }
}
