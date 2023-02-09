using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {

    public static UnityAdsManager instance;

    [Header("广告配置")]
    [SerializeField] public bool adsTestMode;
    [SerializeField] public string androidGameId;
    [SerializeField] public string iOSGameId;
    // 广告奖励
    [SerializeField] public string rewardedAndroidAdUnitId ;
    [SerializeField] public string rewardedIosAdUnitId;
    // 插页式广告
    [SerializeField] public string interstitialAndroidAdUnitId;
    [SerializeField] public string interstitialIosAdUnitId;


    string gameId;
    string rewardedAdUnitId;
    string interstitialAdUnitId;

    public bool isRewardedReady;

    // 广告类型
    enum AdsType {
        REWARDED,
        INTERSTITIAL,
        BANNER
    }

    // 广告事件代理
    public Action<AdsEvent> OnUnityAdsEvent;

    void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        // 广告设置
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            gameId = iOSGameId;
            rewardedAdUnitId = rewardedIosAdUnitId;
            interstitialAdUnitId = interstitialIosAdUnitId;
        } else {
            gameId = androidGameId;
            rewardedAdUnitId = rewardedAndroidAdUnitId;
            interstitialAdUnitId = interstitialAndroidAdUnitId;
        }
        isRewardedReady = false;
        Advertisement.Initialize(gameId, adsTestMode, this);
    }
    /// <summary>
    /// 注册订阅事件
    /// </summary>
    /// <param name="finichCallbacl"></param>
    public void Subscribe(Action<AdsEvent> finichCallback) {
        OnUnityAdsEvent += finichCallback;
        //Debug.Log($"注册事件回调:{finichCallback}");
    }

    /// <summary>
    /// 取消注册事件
    /// </summary>
    /// <param name="finichCallbacl"></param>
    public void UnSubscribe(Action<AdsEvent> finichCallback) {
        OnUnityAdsEvent -= finichCallback;
        //Debug.Log($"取消注册事件回调:{finichCallback}");
    }


    #region 广告
    /// <summary>
    /// 加载激励广告
    /// </summary>
    public void LoadRewardedAd() {
        Advertisement.Load(rewardedAdUnitId, this);
    }
    /// <summary>
    /// 播放激励广告
    /// </summary>
    public void ShowRewardedAd() {
        Advertisement.Show(rewardedAdUnitId, this);
    }
    /// <summary>
    /// 加载插页式广告
    /// </summary>
    public void LoadInterstitialAd() {
        
        Advertisement.Load(interstitialAdUnitId, this);
    }
    /// <summary>
    /// 播放插页式广告
    /// </summary>
    public void ShowInterstitialAd() {
        Advertisement.Show(interstitialAdUnitId, this);
    }

    public void OnInitializationComplete() {
        //Debug.Log("init completed");
        LoadRewardedAd();
        LoadInterstitialAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.LogError($"UnityAdsManager -> Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId) {

        //Debug.Log($"UnityAdsManager -> OnUnityAdsAdLoaded:{placementId}");
        if (placementId.Equals(rewardedAdUnitId)) {
            // Configure the button to call the ShowAd() method when clicked:
            //_showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            isRewardedReady = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        Debug.LogError($"UnityAdsManager -> {placementId} Load Failed: [{error}]: {message}");
        if (placementId.Equals(rewardedAdUnitId)) {
            isRewardedReady = false;
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        Debug.LogError($"UnityAdsManager -> {placementId} Show Failed: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId) {
        //throw new System.NotImplementedException();
        if (placementId.Equals(rewardedAdUnitId)) {
            isRewardedReady = false;
        }
        SoundManager.instance.SetBgmMute(true);
    }

    public void OnUnityAdsShowClick(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        //Debug.Log($"UnityAdsManager -> OnUnityAdsShowComplete:{placementId} ：{showCompletionState}");
        if (rewardedAdUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            //Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            SoundManager.instance.SetBgmMute(false);
            UnityAdsEvent(AdsEvent.SHOW_REWARDED_COMPLETED);
            LoadRewardedAd();
        } else if (interstitialAdUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            SoundManager.instance.SetBgmMute(false);
            UnityAdsEvent(AdsEvent.SHWO_INTERSTITIAL_COMPLETED);
            LoadInterstitialAd();
        }
    }

    /// <summary>
    /// 注册订阅的进行回调通知
    /// </summary>
    /// <param name="adsEvent"></param>
    void UnityAdsEvent(AdsEvent adsEvent) {
        //Debug.Log($"事件回调:{adsEvent}");
        OnUnityAdsEvent?.Invoke(adsEvent);
    }
    #endregion
}
