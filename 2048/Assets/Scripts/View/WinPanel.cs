using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {

    public int winCoins = 50;

    public int watchAdsCoins = 100;


    #region 广告
    [SerializeField]
    private Button watchAdsButton;

    public string gameIdAndroid;

    public string gameIdiOS;

    public string adUnitIdAndroid;

    public string adUnitIdiOS;

    public bool adsTestMode;

    private string gameId = null;

    private string adUnitId = null;
    #endregion

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android) {
            gameId = gameIdAndroid;
            adUnitId = adUnitIdAndroid;
        } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
            gameId = gameIdiOS;
            adUnitId = adUnitIdiOS;
        } else if (Application.platform == RuntimePlatform.OSXEditor) {
            // 测试时候，选择Android 
            gameId = gameIdAndroid;
            adUnitId = adUnitIdAndroid;
        } else {
            Debug.LogError("UnSupported platform :" + Application.platform);
        }
        //_showAdButton.interactable = false;

        Advertisement.Initialize(gameId, adsTestMode, this);
    }

    public virtual void Show() {
        gameObject.SetActive(true);
    }

    public virtual void Close() {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 单倍领取获胜奖励
    /// </summary>
    public void OnNormalRewardsButttonPressed() {
        Close();
        GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().AddCoins(winCoins);
        GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().UIInDefaultModel();
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
            GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().AddCoins(watchAdsCoins);
            GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().UIInDefaultModel();
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
