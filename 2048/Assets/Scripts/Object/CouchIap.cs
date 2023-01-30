using UnityEngine;
using UnityEngine.UI;

public class CouchIap : MonoBehaviour
{
    public Image couch;
    public Text price;
    public Button button;
    public Text buttonText;


    /// <summary>
    /// 设置商品UI
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(IapItem item) {
        price.text = item.price.ToString();
        switch (item.status) {
            case IapItemStatusEnum.Using:
                buttonText.text = "Using";
                break;
            case IapItemStatusEnum.Locked:
                buttonText.text = "Unlock";
                break;
            default:
                buttonText.text = "Unlock";
                break;
        }
        

    }

    public void OnBuyButtonPressed() {
        Debug.Log("购买");
    }



}
