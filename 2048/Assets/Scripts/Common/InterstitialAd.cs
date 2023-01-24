using UnityEngine;
using UnityEngine.Advertisements;
/// <summary>
/// 在具有自然过渡点的应用中，插页式广告的效果最好。
/// 此类过渡点通常存在于应用内的任务结束时，例如分享完图片或完成一个游戏关卡时。
/// 用户希望可以在操作过程中休息一下，因此这时展示插页式广告不会影响用户体验。
/// </summary>
public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {

    public static InterstitialAd instance;

    [SerializeField]
    string _androidAdUnitId = "Interstitial_Android";

    [SerializeField]
    string _iOsAdUnitId = "Interstitial_iOS";

    string _adUnitId;

    void Awake() {

        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    // Load content to the Ad Unit:
    public void LoadAd() {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        //Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd() {
        // Note that if the ad content wasn't previously loaded, this method will fail
        //Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId) {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) {
        //Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }
    /// <summary>
    /// 广告开始 -> 关闭背景音
    /// </summary>
    /// <param name="adUnitId"></param>
    public void OnUnityAdsShowStart(string adUnitId) {
        //Debug.Log("OnUnityAdsShowStart");
        SoundManager.instance.SetBgmMute(true);
    }
    public void OnUnityAdsShowClick(string adUnitId) {
        //Debug.Log("OnUnityAdsShowClick");
    }

    /// <summary>
    /// 广告结束 -> 恢复背景音、加载新广告
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="showCompletionState"></param>
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {
        //Debug.Log($"OnUnityAdsShowComplete" + showCompletionState);
        SoundManager.instance.SetBgmMute(false);
        LoadAd();
    }
}
