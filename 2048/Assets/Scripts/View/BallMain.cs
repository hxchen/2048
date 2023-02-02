using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class BallMain : MonoBehaviour
{
    public static BallMain instance;

    #region UI
    public GameObject heartObject;

    public GameObject coinsObject;

    public GameObject scoreObject;

    public Text lifeText;

    public Text coinsNumberText;

    public Text scoreText;

    public Text bestScoreText;

    public GameOverPanel gameOverPanel;

    public WinPanel winPanel;

    public ShopPanel shopPanel;

    public Button playButton;

    public Button shopButton;

    public GameObject ballsBoard;


    #endregion

    #region 逻辑
    private int coinsNumber;

    private int score;

    public int fullLife;

    private int currentLife;
    


    #endregion

    void Awake() {
        instance = this;
        currentLife = fullLife;
        lifeText.text = currentLife.ToString();
        bestScoreText.text = PlayerPrefs.GetInt(Const.BestScoreBall, 0).ToString();
        coinsNumber = PlayerPrefs.GetInt(Const.CouchCoins, 0);
        coinsNumberText.text = coinsNumber.ToString();
        UIInDefaultModel();
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
        coinsNumber += value;
        coinsNumberText.text = coinsNumber.ToString();
        PlayerPrefs.SetInt(Const.CouchCoins, coinsNumber);
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

    public void ShowCoin(Text coinText, int coinValue, int changeCount = 10, float spaceTime = 0.1f)
    ｛
        StopAllCoroutines();
    StartCoroutine(ShowCoinAni(coinText, coinValue, changeCount, spaceTime));
    ｝
    IEnumerator ShowCoinAni(Text coinText, int coinValue, int changeCount, float spaceTime)
    ｛
        float lastGoldCount = 0;
        if (!string.IsNullOrEmpty(coinText.text))
        ｛
            try
            ｛
                lastGoldCount = System.Convert.ToInt32(coinText.text);
            ｝
            catch(System.Exception e)
            ｛
                Debug.Log(e);
            ｝
        ｝
        float onceAddCount = 1.0f / changeCount * (coinValue - lastGoldCount);
    int i = 0;
        while (i<changeCount)
        ｛
            i++;
            lastGoldCount += onceAddCount;
            coinText.text = ((int) lastGoldCount).ToString();
    yield return new WaitForSeconds(spaceTime);
        ｝
        coinText.text = coinValue.ToString();
    ｝

    /// <summary>
    /// 更新生命
    /// </summary>
    /// <param name="value"></param>
    public void SubtractLife() {
        currentLife--;
        lifeText.text = currentLife.ToString();
        if (currentLife == 0) {
            GameLose();
        }
        
    }

    /// <summary>
    /// 显示胜利UI
    /// </summary>
    public void GameWin() {
        winPanel.Show();
        BallManager.instancs.NeedNewBall(false);
    }

    /// <summary>
    /// 显示失败UI
    /// </summary>
    public void GameLose() {
        Time.timeScale = 0f;
        gameOverPanel.Show();
        AddCoins(gameOverPanel.loseCoins);
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
        BallManager.instancs.StartGame();
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame() {
        // 更新生命
        currentLife = fullLife;
        lifeText.text = currentLife.ToString();
        // 更新分数
        score = 0;
        scoreText.text = score.ToString();
        // 清空球
        ClearBalls();

        OnPlayButtonPressed();
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

        ClearBalls();
    }


}
