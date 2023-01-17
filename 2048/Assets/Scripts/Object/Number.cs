using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    private Image background;
    private Text numberText;
    private int numberValue;

    public Dictionary<int, string> backgroundColors = new Dictionary<int, string>() {
        { 2, "#eee4da" }, { 4, "#ede0c8" }, { 8, "#f9f6f2" }, { 16, "#f59563" },
        { 32, "#f67c5f" }, { 64, "#f65e3b" }, { 128, "#edcf72" },{ 256, "#edcc61" },
        { 512, "#edc850" }, { 1024, "#edc53f" }, { 2048, "#edc22e" }
    };
    ////所属格子
    //private Tile tile;

    void Awake() {
        background = transform.GetComponent<Image>();
        numberText = transform.Find("Text").GetComponent<Text>();

    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="number"></param>
    public Number Init(bool isShow, int number = 2) {
        this.SetNumberValue(number);
        SetColor(number);
        return this;
    }

    /// <summary>
    /// 设置文本显示,因为动画的存在，这是一个延时显示
    /// </summary>
    /// <param name="number"></param>
    public void SetNumberValueText(int number) {
        this.numberText.text = number.ToString();
        SetColor(number);
    }
    /// <summary>
    /// 设置实时数字
    /// </summary>
    /// <param name="number"></param>
    public void SetNumberValue(int number) {
        numberValue = number;
    }

    public int GetNumberValue() {
        return numberValue;
    }

    public void MoveToGrid(Tile target, GameBoard board) {
        transform.SetParent(target.transform);
        transform.localPosition = Vector3.zero;
    }


    private void SetColor(int value) {
        Color color = new Color(238 / 255f, 228 / 255f, 219 / 255f);
        ColorUtility.TryParseHtmlString(backgroundColors[value], out color);
        background.color = color;
    }
}
