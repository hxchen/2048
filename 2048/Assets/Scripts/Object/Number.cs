using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    private Image background;
    private Text numberText;
    private MyGrid inGrid;

    void Awake() {
        background = transform.GetComponent<Image>();
        numberText = transform.Find("Text").GetComponent<Text>();

    }

    public void Init(MyGrid myGrid) {
        myGrid.SetNumber(this);
        this.SetGrid(myGrid);
        this.SetNumber(2);
    }

    public void SetGrid(MyGrid myGrid) {
        this.inGrid = myGrid;
    }

    public MyGrid GetGrid() {
        return this.inGrid;
    }

    public void SetNumber(int number) {
        this.numberText.text = number.ToString();
    }

    public int GetNumber() {
        return int.Parse(numberText.text);
    }

    
}
