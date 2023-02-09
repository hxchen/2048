using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class ContinuePanel : View {


    [SerializeField] Button playOnButton;

    [SerializeField] Button giveUpButton;

    [SerializeField] Button buyButon;

    [SerializeField] Text addLifeText;

    [SerializeField] Text timesLeftText;

    [SerializeField] Text coinsToPlayOnText;

    [SerializeField] Text currentCoinsText;

    
    private BallMain ballMain;


    void Awake()
    {
        ballMain = GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>();
        addLifeText.text = $"+{Const.AdsRewardsForLife}";
        coinsToPlayOnText.text = Const.CoinsToPlayOn.ToString();
        UnityAdsManager.instance.Subscribe(HandleAdsEvent);
    }
    public override void Show()
    {
        base.Show();
        if (UnityAdsManager.instance.isRewardedReady) {
            playOnButton.interactable = true;
        } else {
            playOnButton.interactable = false;
        }
        if (PlayerPrefs.GetInt(Const.CouchCoins) >= Const.CoinsToPlayOn) {
            buyButon.interactable = true;
        } else {
            buyButon.interactable = false;
        }
        currentCoinsText.text = PlayerPrefs.GetInt(Const.CouchCoins).ToString();
        int left = ballMain.GetCurrentTimesOfAddingChances();
        timesLeftText.text = $"{left} left";

    }
    void OnDestroy()
    {
        UnityAdsManager.instance.UnSubscribe(HandleAdsEvent);
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void AddChancesAndContinueToPlay() {
        ballMain.AddLife(Const.AdsRewardsForLife);
        ballMain.Continue();
        Close();
    }
    
    /// <summary>
    /// 没有机会后放弃游戏
    /// </summary>
    public void OnGiveUpButtonPresed() {
        Close();
        // 展示游戏结束界面
        ballMain.GameLose();
    }

    /// <summary>
    /// 没有机会后,继续游戏
    /// </summary>
    public void OnPlayOnButtonPressed() {
        // Disable the button:
        playOnButton.interactable = false;
        // Then show the ad:
        UnityAdsManager.instance.ShowRewardedAd();
    }

    /// <summary>
    /// 购买按钮
    /// </summary>
    public void OnBuyButtonPressed() {
        ballMain.SubtractCoins(Const.CoinsToPlayOn);
        AddChancesAndContinueToPlay();
    }

    /// <summary>
    /// 看完广告回调通知
    /// </summary>
    /// <param name="adsEvent"></param>
    void HandleAdsEvent(AdsEvent adsEvent) {
        //Debug.Log($"接收事件回调:{adsEvent}");
        if (adsEvent == AdsEvent.SHOW_REWARDED_COMPLETED) {
            playOnButton.interactable = true;
            AddChancesAndContinueToPlay();
        }
    }
}
