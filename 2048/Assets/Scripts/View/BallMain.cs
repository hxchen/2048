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
    private int coinsNumber;

    private int score;

    [Header("Number of life per round")]
    public int fullLife;

    private int currentLife;

    private int currentTimesOfAddingChances;

    #endregion

    void Awake() {
        instance = this;
        currentLife = fullLife;
        currentTimesOfAddingChances = Const.AdsRewardsForLifeTimes;
        lifeText.text = currentLife.ToString();
        bestScoreText.text = PlayerPrefs.GetInt(Const.BestScoreBall, 0).ToString();
        coinsNumber = PlayerPrefs.GetInt(Const.CouchCoins, 0);
        coinsNumberText.text = coinsNumber.ToString();
        UIInDefaultModel();
        targetCoinsPosition = coinsObject.transform.position;
        PrepareCoins();
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
    /// 飞金币动画
    /// </summary>
    /// <param name="collectedCoinsPostion"></param>
    /// <param name="amount"></param>
    void PlayAnimateCoin(Vector3 collectedCoinsPostion, int amount) {
        for (int i = 0; i < amount; i++) {
            if (coinsQueue.Count > 0) {
                GameObject coin = coinsQueue.Dequeue();
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
            } else {
                Debug.Log("金币实体数量不足");
            }
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
        int bestScore = PlayerPrefs.GetInt(Const.BestScoreBall, 0);
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
        lifeText.text = currentLife.ToString();
        if (currentLife <= 0) {
            if (currentTimesOfAddingChances > 0) {
                NowMoreChances();
            } else {
                GameLose();
            }
            
        }
        
    }

    /// <summary>
    /// 添加生命
    /// </summary>
    /// <param name="value"></param>
    public void AddLife(int value) {
        if (currentTimesOfAddingChances <= 0) {
            return;
        }
        currentLife += value;
        lifeText.text = currentLife.ToString();
        currentTimesOfAddingChances--;
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
