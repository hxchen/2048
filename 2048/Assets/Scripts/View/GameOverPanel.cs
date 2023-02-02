using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    // 失败鼓励金币
    public int loseCoins = 10;

    public Text RewardsNumberText;

    void Awake()
    {
        RewardsNumberText.text = $"+{loseCoins}";
    }

    public virtual void Show() {
        gameObject.SetActive(true);
    }

    public virtual void Close() {
        gameObject.SetActive(false);
        GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().UIInDefaultModel();
    }

    public void OnNewButonPressed() {
        Close();
        GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().RestartGame();
        
    }

    public void OnBackuttonPressed() {
        SceneManager.LoadSceneAsync(0);
    }

}
