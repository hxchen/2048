using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectModelPanel : View
{
    

    public void OnSelectModelButtonPressed(int count) {

        PlayerPrefs.SetInt(Const.GameModel, count);

        SceneManager.LoadSceneAsync(1);

    }

    
}
