using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : View {


    [SerializeField] Button watchAdsButton;

    [SerializeField] Text normalCoinsNumberText;

    [SerializeField] Text adsCoinsNumberText;

    BallMain ballMain;


    void Awake()
    {
        ballMain = GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>();
        normalCoinsNumberText.text = $"+{Const.WinCoins}";
        adsCoinsNumberText.text = $"+{Const.WinCoins * 2}";
        // 广告
        UnityAdsManager.instance.Subscribe(HandleAdsEvent);
    }

    void OnDestroy() {
        UnityAdsManager.instance.UnSubscribe(HandleAdsEvent);
    }

    /// <summary>
    /// 单倍领取获胜奖励
    /// </summary>
    public void OnNormalRewardsButttonPressed() {
        Close();
        ballMain.AddCoins(Const.WinCoins);
    }

    /// <summary>
    /// 看广告，双倍领取获胜奖励
    /// </summary>
    public void OnWatchAdsButtonPressed() {
        // Disable the button:
        watchAdsButton.interactable = false;
        // Then show the ad:
        UnityAdsManager.instance.ShowRewardedAd();
    }

    /// <summary>
    /// 看完广告回调通知
    /// </summary>
    /// <param name="adsEvent"></param>
    void HandleAdsEvent(AdsEvent adsEvent) {
        if (adsEvent == AdsEvent.SHOW_REWARDED_COMPLETED) {
            Close();
            watchAdsButton.interactable = true;
            ballMain.AddCoins(Const.WinCoins * 2);
            
        }
    }

}
