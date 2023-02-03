using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class WinPanel : View, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {


    [SerializeField] Button watchAdsButton;

    [SerializeField] Text normalCoinsNumberText;

    [SerializeField] Text adsCoinsNumberText;

    BallMain ballMain;

    string gameId;

    string adUnitId;


    void Awake()
    {
        ballMain = GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>();
        normalCoinsNumberText.text = $"+{Const.WinCoins}";
        adsCoinsNumberText.text = $"+{Const.WinCoins * 2}";

        if (Application.platform == RuntimePlatform.Android) {
            gameId = AdsConfiguration.instance.androidGameId;
            adUnitId = AdsConfiguration.instance.adUnitIdAndroid;
        } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
            gameId = AdsConfiguration.instance.iOSGameId;
            adUnitId = AdsConfiguration.instance.adUnitIdiOS;
        } else if (Application.platform == RuntimePlatform.OSXEditor) {
            // 测试时候，选择Android 
            gameId = AdsConfiguration.instance.androidGameId; ;
            adUnitId = AdsConfiguration.instance.adUnitIdAndroid; ;
        } else {
            Debug.LogError("UnSupported platform :" + Application.platform);
        }
        //_showAdButton.interactable = false;

        Advertisement.Initialize(gameId, AdsConfiguration.instance.adsTestMode, this);
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
        Advertisement.Show(adUnitId, this);
    }

    #region 广告
    // Load content to the Ad Unit:
    public void LoadAd() {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + adUnitId);
        Advertisement.Load(adUnitId, this);
    }

    public void OnInitializationComplete()
    {
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (adUnitId.Equals(adUnitId)) {
            // Configure the button to call the ShowAd() method when clicked:
            //_showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            watchAdsButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            //Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            Close();
            ballMain.AddCoins(Const.WinCoins * 2);
            ballMain.UIInDefaultModel();
            // Load another ad:
            LoadAd();
        }
    }

    void OnDestroy() {
        // Clean up the button listeners:
        watchAdsButton.onClick.RemoveAllListeners();
    }
    #endregion
}
