using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{

    public SelectModelPanel selectModelPanel;

    public SettingsPanel settingsPanel;

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void OnStartButtonPressed() {
        selectModelPanel.Show();
    }

    /// <summary>
    /// 游戏设置
    /// </summary>
    public void OnSettingsButtonPressed() {

        settingsPanel.Show();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void OnExitButtonPressed() {
        Application.Quit(0);
    }
}
