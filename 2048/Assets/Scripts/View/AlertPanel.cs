using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AlertPanel : MonoBehaviour
{

    public Text alertText;

    public void setAlertText(string text) {
        alertText.text = text;
    }

    public virtual void Show() {
        gameObject.SetActive(true);
    }

    public virtual void Close() {
        gameObject.SetActive(false);
    }

    public void OnNewButonPressed() {
        GameObject.Find("Canvas/GameBoard").GetComponent<GameBoard>().RestartGame();
        Close();
    }

    public void OnExitButtonPressed() {
        SceneManager.LoadSceneAsync(0);
    }
}
