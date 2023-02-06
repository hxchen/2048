using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip bgmAudioClip, mergeAudioClip;
    public AudioSource bgmAudioSource, mergeAudioSource;
    public static SoundManager instance;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    private void Awake() {
        if (instance != null) {
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        PlayBgm();

        instance.bgmAudioSource.volume = PlayerPrefs.GetFloat(Const.Music, 0.5f);
        instance.mergeAudioSource.volume = PlayerPrefs.GetFloat(Const.Sound, 0.5f);
    }
    /// <summary>
    /// 播放声音
    /// </summary>
    public void PlaySound() {
        instance.mergeAudioSource.PlayOneShot(mergeAudioClip);
    }

    public void PlayBgm() {
        instance.bgmAudioSource.clip = bgmAudioClip;
        instance.bgmAudioSource.loop = true;
        instance.bgmAudioSource.Play();

    }

    public void OnSoundValueChange(float value) {
        instance.mergeAudioSource.volume = value;
    }

    public void OnMusicValueChange(float value) {
        instance.bgmAudioSource.volume = value;
    }


    public void SetBgmMute(bool mute) {
        instance.bgmAudioSource.mute = mute; 
    }
}
