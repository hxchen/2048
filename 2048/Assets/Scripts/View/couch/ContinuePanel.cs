using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class ContinuePanel : View {


    [SerializeField] Button playOnButton;

    [SerializeField] Button giveUpButton;

    [SerializeField] Text addLifeText;

    
    private BallMain ballMain;


    void Awake()
    {
        ballMain = GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>();
        addLifeText.text = $"+{Const.AdsRewardsForLife}";
        UnityAdsManager.instance.Subscribe(HandleAdsEvent);
    }

    void OnDestroy()
    {
        UnityAdsManager.instance.UnSubscribe(HandleAdsEvent);
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void AddChancesAndContinueToPlay() {
        Close();
        ballMain.AddLife(Const.AdsRewardsForLife);
        ballMain.Continue();
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
    /// 没有机会后放弃游戏
    /// </summary>
    public void OnGiveUpButtonPresed() {
        Close();
        // 展示游戏结束界面
        ballMain.GameLose();
    }

    /// <summary>
    /// 看完广告回调通知
    /// </summary>
    /// <param name="adsEvent"></param>
    void HandleAdsEvent(AdsEvent adsEvent) {
        if (adsEvent == AdsEvent.SHOW_REWARDED_COMPLETED) {
            playOnButton.interactable = true;
            AddChancesAndContinueToPlay();
        }
    }
}
