using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : View {

    [SerializeField]
    Text RewardsNumberText;

    [SerializeField]
    Button claimButton;

    BallMain ballMain;

    void Awake()
    {
        ballMain = GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>();
        RewardsNumberText.text = $"+{Const.LoseCoins}";
    }

    public override void Show()
    {
        base.Show();
        claimButton.gameObject.SetActive(true);
    }
    /// <summary>
    /// 领取失败奖励
    /// </summary>
    public void OnClaimButtonPressed() {
        ballMain.AddCoins(Const.LoseCoins);
        Close();
    }
    
}
