using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : View
{
    public void OnNewButonPressed() {
        GameObject.Find("Canvas/GameBoard").GetComponent<GameBoard>().RestartGame();
        Close();
    }

    public void OnExitButtonPressed() {
        SceneManager.LoadSceneAsync(0);
    }
}
