using UnityEngine;
/// <summary>
/// Tile
/// </summary>
public class Tile : MonoBehaviour
{
    public Number number;

    //判断格子中是否有数字
    public bool HaveNumber() {
        return number != null;
    }

    //获取数字
    public Number GetNumber() {
        return number;
    }

    //设置数字
    public void SetNumber(Number number) {
        this.number = number;
    }
}
