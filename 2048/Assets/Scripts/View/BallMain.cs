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

    public Text lifeText;

    public Text scoreText;

    public Text bestScoreText;

    public CouchAlertPanel alertPanel;

    public ShopPanel shopPanel;

    public Button playButton;

    public Button shopButton;

    public GameObject ballsBoard;


    #endregion

    #region 逻辑
    private int score;

    public int fullLife;

    private int currentLife;


    #endregion

    void Awake() {
        instance = this;
        currentLife = fullLife;
        lifeText.text = currentLife.ToString();
        bestScoreText.text = PlayerPrefs.GetInt(Const.BestScoreBall, 0).ToString();
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
        alertPanel.setAlertText("You Win!");
        alertPanel.Show();
    }

    /// <summary>
    /// 显示失败UI
    /// </summary>
    public void GameLose() {
        Time.timeScale = 0f;
        alertPanel.setAlertText("Game Over!");
        alertPanel.Show();
    }

    /// <summary>
    /// Shop按钮
    /// </summary>
    public void OnShopButtonPressed() {
        Debug.Log("打开商店");
        shopPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public void OnPlayButtonPressed() {
        Time.timeScale = 1f;
        heartObject.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
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
}
