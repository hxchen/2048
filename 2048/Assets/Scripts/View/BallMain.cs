using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class BallMain : MonoBehaviour
{
    public static BallMain instance;

    #region UI
    public Text lifeText;

    public Text scoreText;

    public Text bestScoreText;

    public AlertPanel alertPanel;

    public ShopPanel shopPanel;

    public Button playButton;

    public Button shopButton;

    #endregion

    #region 逻辑
    private int score;

    public int fullLife;


    #endregion

    void Awake() {
        instance = this;
        lifeText.text = fullLife.ToString();
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
        fullLife--;
        lifeText.text = fullLife.ToString();
        if (fullLife == 0) {
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
        playButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        BallManager.instancs.StartGame();
    }
}
