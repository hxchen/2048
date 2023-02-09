using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BallMain : MonoBehaviour {
    public static BallMain instance;

    #region UI

    [SerializeField] GameObject heartObject;

    [SerializeField] GameObject coinsObject;

    [SerializeField] GameObject scoreObject;

    [SerializeField] Text lifeText;

    [SerializeField] Text coinsNumberText;

    [SerializeField] Text scoreText;

    [SerializeField] Text bestScoreText;

    [SerializeField] GameOverPanel gameOverPanel;

    [SerializeField] ContinuePanel continuePanel;

    [SerializeField] WinPanel winPanel;

    [SerializeField] ShopPanel shopPanel;

    [SerializeField] Button playButton;

    [SerializeField] Button shopButton;

    [SerializeField] Button homeButton;

    [SerializeField] Button exitButton;

    [SerializeField] GameObject ballsBoard;

    [SerializeField] GameObject animatedCoinPrefab;

    [SerializeField] GameObject coinsContainer;

    [Space]
    [Header("Available coins:(coins to pool)")]
    public int maxCoins;
    // Queue to store coins to animate later.
    Queue<GameObject> coinsQueue = new Queue<GameObject>();

    [Space]
    [Header("Animation settings")]
    [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField] [Range(0.5f, 0.9f)] float maxAnimDuration;

    [SerializeField] Ease easeTYpe;
    [SerializeField] float spreed;

    Vector3 targetCoinsPosition;

    #endregion

    #region 逻辑
    //当前金币
    private int coinsNumber;

    private int score;

    private int bestScore;

    [Header("Number of life per round")]
    public int fullLife;

    private int currentLife;

    private int currentTimesOfAddingChancesByAds;

    [Header("Init Coins")]
    public int initCoins;

    #endregion

    void Awake() {
        instance = this;
        currentLife = fullLife;
        currentTimesOfAddingChancesByAds = Const.AdsRewardsForLifeTimes;
        lifeText.text = currentLife.ToString();
        bestScore = PlayerPrefs.GetInt(Const.BestScoreBall, 0);
        bestScoreText.text = bestScore.ToString();
        // 初始化金币
        if (PlayerPrefs.HasKey(Const.CouchCoins)) {
            coinsNumber = PlayerPrefs.GetInt(Const.CouchCoins);
        } else {
            coinsNumber = initCoins;
            PlayerPrefs.SetInt(Const.CouchCoins, coinsNumber);
        }
        coinsNumberText.text = coinsNumber.ToString();
        UIInDefaultModel();
        targetCoinsPosition = coinsObject.transform.position;
        PrepareCoins();
    }
    void Update()
    {
        // 因为有动画，不能更新到实时金币显示
        //coinsNumberText.text = coinsNumber.ToString();
        bestScoreText.text = bestScore.ToString();
        lifeText.text = currentLife.ToString();
    }
    /// <summary>
    /// 初始化金币池
    /// </summary>
    void PrepareCoins() {
        maxCoins = maxCoins < Const.WinCoins * 2 ? Const.WinCoins : maxCoins; 
        GameObject coin;
        for (int i = 0; i < maxCoins; i++) {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = coinsContainer.transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    /// <summary>
    /// 获得一个金币，优先从池里取，池里没有就新建
    /// </summary>
    private GameObject GetCoinObject() {
        GameObject coin;
        if (coinsQueue.Count > 0) {
            coin = coinsQueue.Dequeue();
        } else {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = coinsContainer.transform;
            coin.SetActive(false);
        }
        return coin;
    }

    /// <summary>
    /// 飞金币动画
    /// </summary>
    /// <param name="collectedCoinsPostion"></param>
    /// <param name="amount"></param>
    void PlayAnimateCoin(Vector3 collectedCoinsPostion, int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject coin = GetCoinObject();
            coin.SetActive(true);
            coin.transform.position = collectedCoinsPostion + new Vector3(Random.Range(-spreed, spreed), 0f, 0f);
            float duration = Random.Range(minAnimDuration, maxAnimDuration);
            coin.transform.DOMove(targetCoinsPosition, duration)
                .SetEase(easeTYpe)
                .OnComplete(() => {
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                    coinsNumberText.text = (int.Parse(coinsNumberText.text) + 1).ToString();
                });
        }
        
    }

    /// <summary>
    /// 增加积分
    /// </summary>
    /// <param name="value"></param>
    public void AddScore(int value) {
        score += value;
        scoreText.text = score.ToString();
        UpdateBestScore(score);
    }

    /// <summary>
    /// 更新最高分
    /// </summary>
    /// <param name="score"></param>
    public void UpdateBestScore(int currentScore) {
        if (currentScore > bestScore) {
            bestScore = currentScore;
            bestScoreText.text = bestScore.ToString();
            PlayerPrefs.SetInt(Const.BestScoreBall, bestScore);
        }
    }

    /// <summary>
    /// 添加金币
    /// </summary>
    /// <param name="value"></param>
    public void AddCoins(int value) {
        // 保存数据
        coinsNumber += value;
        PlayerPrefs.SetInt(Const.CouchCoins, coinsNumber);
        // 播放动画
        PlayAnimateCoin(new Vector3(0f, 0f, 0f), value);
    }

    /// <summary>
    /// 扣除金币
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool SubtractCoins(int value) {
        if (value > coinsNumber) {
            return false;
        } else {
            coinsNumber -= value;
            coinsNumberText.text = coinsNumber.ToString();
            PlayerPrefs.SetInt(Const.CouchCoins, coinsNumber);
            return true;
        }
    }


    /// <summary>
    /// 更新生命
    /// </summary>
    /// <param name="value"></param>
    public void SubtractLife() {
        currentLife--;
        if (currentLife <= 0) {
            if (currentTimesOfAddingChancesByAds > 0 || PlayerPrefs.GetInt(Const.CouchCoins) >= Const.CoinsToPlayOn) {
                BallManager.instancs.ReleaseSelectedBall();
                NowMoreChances();
            } else {
                BallManager.instancs.ReleaseSelectedBall();
                GameLose();
            }
        }
        currentLife = currentLife < 0 ? 0 : currentLife;
        lifeText.text = currentLife.ToString();

    }

    /// <summary>
    /// 添加生命
    /// </summary>
    /// <param name="value"></param>
    public void AddLife(int value) {
        currentLife += value;
        lifeText.text = currentLife.ToString();
    }

    /// <summary>
    /// 通过广告添加生命
    /// </summary>
    /// <param name="value"></param>
    public void AddLifeByAds(int value) {
        AddLife(value);
        currentTimesOfAddingChancesByAds--;
    }

    /// <summary>
    /// 获取剩余加生命次数
    /// </summary>
    /// <returns></returns>
    public int GetCurrentTimesOfAddingChances() {
        return currentTimesOfAddingChancesByAds;
    }

    /// <summary>
    /// 显示胜利UI
    /// </summary>
    public void GameWin() {
        winPanel.Show();
        BallManager.instancs.SetGameState(GameState.Hang);
        BallManager.instancs.NeedNewBall(false);
        UIInDefaultModel();
    }

    /// <summary>
    /// 没有游戏次数，展示是否继续游戏面板
    /// </summary>
    public void NowMoreChances() {
        continuePanel.Show();
        BallManager.instancs.SetGameState(GameState.Hang);
        BallManager.instancs.NeedNewBall(false);
    }

    /// <summary>
    /// 显示失败UI
    /// </summary>
    public void GameLose() {
        //Time.timeScale = 0f;
        gameOverPanel.Show();
        BallManager.instancs.SetGameState(GameState.Hang);
        //AddCoins(gameOverPanel.loseCoins);
        UIInDefaultModel();
    }

    /// <summary>
    /// Shop按钮
    /// </summary>
    public void OnShopButtonPressed() {
        shopPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public void OnPlayButtonPressed() {
        Time.timeScale = 1f;
        UIInPlayModel();
        // 更新生命
        currentLife = fullLife;
        lifeText.text = currentLife.ToString();
        // 更新分数
        score = 0;
        scoreText.text = score.ToString();
        // 更新看广告次数
        currentTimesOfAddingChancesByAds = Const.AdsRewardsForLifeTimes;
        // 清空球
        ClearBalls();
        BallManager.instancs.StartGame();
    }

    /// <summary>
    /// 回到首页
    /// </summary>
    public void OnHomeButtonPressed() {
        SceneManager.LoadSceneAsync(0);
    }

    /// <summary>
    /// 退出到游戏开始
    /// </summary>
    public void OnExitButtonPressed() {
        BallManager.instancs.SetGameState(GameState.Hang);
        BallManager.instancs.NeedNewBall(false);
        UIInDefaultModel();
    }

    /// <summary>
    /// 清除小球
    /// </summary>
    public void ClearBalls() {
        for (int i = 0; i < ballsBoard.transform.childCount; i++) {
            Destroy(ballsBoard.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 游戏模式下的UI显示控制
    /// </summary>
    public void UIInPlayModel() {
        coinsObject.SetActive(false);
        heartObject.SetActive(true);
        scoreObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        shopButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// 默认下的UI显示控制
    /// </summary>
    public void UIInDefaultModel() {
        coinsObject.SetActive(true);
        heartObject.SetActive(false);
        scoreObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        ClearBalls();
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void Continue() {
        BallManager.instancs.SetGameState(GameState.Play);
        BallManager.instancs.NeedNewBall(true);
    }

}
