using UnityEngine;
/// <summary>
/// Tile 永远都不会变，变的只有其下的数字
/// </summary>
public class Tile : MonoBehaviour
{
    public GameObject numberPrefab;

    public Number number;
    // 增加一个属性用来判断，单独判断number会存在延迟不准
    private bool existNumber;

    public int x, y;

    public bool hasNumber() {
        return existNumber;
    }
    public void SetNumber(Number number) {
        this.number = number;
    }

    public Number GetNumber() {
        return number;
    }

    public void CreateNumber(int value = 2) {
        existNumber = true;
        GameObject gameObject = GameObject.Instantiate(numberPrefab, transform);
        gameObject.GetComponent<Number>().SetNumberValue(value);
        SetNumber(gameObject.GetComponent<Number>());
    }
    
    /// <summary>
    /// 与目标格子数字合并后并放在目标格子
    /// </summary>
    /// <param name="targetGrid"></param>
    public void MergeNumber(Tile targetTile) {
        
        Debug.Log($"Merge:({x},{y}) -> ({targetTile.x},{targetTile.y})");
        int newNumberValue = this.GetNumber().GetNumberValue() + targetTile.GetNumber().GetNumberValue();

        targetTile.existNumber = true;
        targetTile.GetNumber().SetNumberValue(newNumberValue);
        existNumber = false;

        var tween = LeanTween.move(number.gameObject, targetTile.gameObject.transform.position, 0.2f);
        tween.setEase(LeanTweenType.easeInQuad);
        tween.setOnComplete(() => {

            targetTile.GetNumber().SetNumberValueText(newNumberValue);
            Destroy(number.gameObject);
            

        });
        

    }

    /// <summary>
    /// 移动数字到目标Tile
    /// </summary>
    /// <param name="target"></param>
    public void MoveNumber(Tile targetTile) {
        Debug.Log($"Move:({x},{y}) -> ({targetTile.x},{targetTile.y})");
        // Update UI
        //number.transform.SetParent(targetTile.transform);
        //number.transform.localPosition = Vector3.zero;
        targetTile.CreateNumber(number.GetNumberValue());
        targetTile.number.gameObject.SetActive(false);
        targetTile.existNumber = true;

        existNumber = false;

        var tween = LeanTween.move(number.gameObject, targetTile.gameObject.transform.position, 0.2f);
        tween.setEase(LeanTweenType.easeInQuad);
        tween.setOnComplete(() => {

            targetTile.number.gameObject.SetActive(true);
            Destroy(number.gameObject);

        });

    }
}
