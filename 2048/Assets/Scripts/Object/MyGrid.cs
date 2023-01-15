using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{

    public Number number;

    public bool isEmpty() {
        return number == null;
    }

    public void SetNumber(Number number) {
        this.number = number;
    }

    public Number GetNumber() {
        return number;
    }
}
