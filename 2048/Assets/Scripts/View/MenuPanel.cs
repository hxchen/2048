using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{

    public SelectModelPanel selectModelPanel;

    public SettingsPanel settingsPanel;

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void OnClassicButtonPressed() {
        selectModelPanel.Show();
    }

    /// <summary>
    /// 沙发2048
    /// </summary>
    public void OnCouchButtonPressed() {
        SceneManager.LoadSceneAsync(2);
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
