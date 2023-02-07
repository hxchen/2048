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

    // 广告类型
    enum AdsType {
        REWARDED,
        INTERSTITIAL,
        BANNER
    }
    //当前播放的广告类型
    AdsType currentAdsType;

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
        Advertisement.Initialize(gameId, adsTestMode, this);
    }
    /// <summary>
    /// 注册订阅事件
    /// </summary>
    /// <param name="finichCallbacl"></param>
    public void Subscribe(Action<AdsEvent> finichCallback) {
        OnUnityAdsEvent += finichCallback;
    }

    /// <summary>
    /// 取消注册事件
    /// </summary>
    /// <param name="finichCallbacl"></param>
    public void UnSubscribe(Action<AdsEvent> finichCallback) {
        OnUnityAdsEvent -= finichCallback;
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
        currentAdsType = AdsType.REWARDED;
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
        currentAdsType = AdsType.INTERSTITIAL;
        Advertisement.Show(interstitialAdUnitId, this);
    }

    public void OnInitializationComplete() {
        Debug.Log("init completed");
        LoadRewardedAd();
        LoadInterstitialAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.LogError($"UnityAdsManager -> Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId) {
        Debug.Log($"UnityAdsManager -> OnUnityAdsAdLoaded:{placementId}");
        //if (adUnitId.Equals(adUnitId)) {
        //    // Configure the button to call the ShowAd() method when clicked:
        //    //_showAdButton.onClick.AddListener(ShowAd);
        //    // Enable the button for users to click:
        //    UnityAdsEvent("Loaded");
        //}
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId) {
        //throw new System.NotImplementedException();
        SoundManager.instance.SetBgmMute(true);
    }

    public void OnUnityAdsShowClick(string placementId) {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        if (rewardedAdUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            //Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            SoundManager.instance.SetBgmMute(false);
            UnityAdsEvent(AdsEvent.SHOW_REWARDED_COMPLETED);
            
            // Load another ad:
            LoadRewardedAd();
        } else if (interstitialAdUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            SoundManager.instance.SetBgmMute(false);
            UnityAdsEvent(AdsEvent.SHWO_INTERSTITIAL_COMPLETED);
            LoadInterstitialAd();
        }
    }


    void UnityAdsEvent(AdsEvent adsEvent) {
        OnUnityAdsEvent?.Invoke(adsEvent);
    }
    #endregion
}
