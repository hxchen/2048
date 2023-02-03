using UnityEngine;
using UnityEngine.Advertisements;

public class AdsConfiguration : MonoBehaviour, IUnityAdsInitializationListener {

    [Header("广告配置")]
    [SerializeField] public bool adsTestMode;
    [SerializeField] public string androidGameId;
    [SerializeField] public string iOSGameId;
    // 广告奖励
    [SerializeField] public string adUnitIdAndroid;
    [SerializeField] public string adUnitIdiOS;


    string _gameId;

    public static AdsConfiguration instance;

    void Awake() {
        instance = this;
        InitializeAds();
    }

    public void InitializeAds() {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSGameId
            : androidGameId;
        Advertisement.Initialize(_gameId, adsTestMode, this);
    }

    public void OnInitializationComplete() {
        //Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
