using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AlertPanel : View
{

    public Text alertText;

    public void setAlertText(string text) {
        alertText.text = text;
    }

    public void OnNewButonPressed() {
        GameObject.Find("Canvas/GameBoard").GetComponent<GameBoard>().RestartGame();
        Close();
    }

    public void OnExitButtonPressed() {
        SceneManager.LoadSceneAsync(0);
    }
}
