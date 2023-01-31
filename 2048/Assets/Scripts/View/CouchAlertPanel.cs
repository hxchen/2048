using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CouchAlertPanel : MonoBehaviour
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
        GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().RestartGame();
        Close();
    }

    public void OnBackuttonPressed() {
        SceneManager.LoadSceneAsync(0);
    }
}
