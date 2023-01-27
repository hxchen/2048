using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMain : MonoBehaviour
{
    public static BallMain instance;

    #region UI
    public Text scoreText;

    public Text bestScoreText;

    #endregion

    #region 逻辑
    private int score;


    #endregion

    void Awake() {
        instance = this;
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



}
