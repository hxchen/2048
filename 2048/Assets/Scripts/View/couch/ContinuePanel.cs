using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class ContinuePanel : View, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {

    [SerializeField] Button playOnButton;

    [SerializeField] Button giveUpButton;

    [SerializeField] Text addLifeText;

    private string gameId = null;

    private string adUnitId = null;

    private BallMain ballMain;


    void Awake()
    {
        ballMain = GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>();
        addLifeText.text = $"+{Const.AdsRewardsForLife}";
        // 广告设置
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
        Advertisement.Show(adUnitId, this);
    }

    /// <summary>
    /// 没有机会后放弃游戏
    /// </summary>
    public void OnGiveUpButtonPresed() {
        Close();
        // 展示游戏结束界面
        ballMain.GameLose();
    }

    #region 广告
    // Load content to the Ad Unit:
    public void LoadAd() {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + adUnitId);
        Advertisement.Load(adUnitId, this);
    }

    public void OnInitializationComplete() {
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsAdLoaded(string placementId) {
        if (adUnitId.Equals(adUnitId)) {
            // Configure the button to call the ShowAd() method when clicked:
            //_showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            playOnButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        if (adUnitId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            //Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            AddChancesAndContinueToPlay();
            
            //GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().UIInDefaultModel();
            // Load another ad:
            LoadAd();
        }
    }

    void OnDestroy() {
        // Clean up the button listeners:
        playOnButton.onClick.RemoveAllListeners();
    }
    #endregion
}
