using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : View
{

    public Slider soundSlider;

    public Slider musicSlider;


    /// <summary>
    /// 关闭
    /// </summary>
    public void OnCloseButtonPrssed() {
        Close(); 
    }
    /// <summary>
    /// 音效
    /// </summary>
    public void OnSoundedValueChange(float value) {
        PlayerPrefs.SetFloat(Const.Sound, value);
        SoundManager.instance.OnSoundValueChange(value);
    }
    /// <summary>
    /// 音乐
    /// </summary>
    public void OnMusicValueChange(float value) {
        PlayerPrefs.SetFloat(Const.Music, value);
        SoundManager.instance.OnMusicValueChange(value);
    }

    public override void Show() {
        base.Show();
        soundSlider.value = PlayerPrefs.GetFloat(Const.Sound, 0);
        musicSlider.value = PlayerPrefs.GetFloat(Const.Music, 0);
    }

}
