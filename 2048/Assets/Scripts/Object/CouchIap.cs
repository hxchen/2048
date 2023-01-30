using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static FullSerializer.fsAotVersionInfo;

public class CouchIap : MonoBehaviour
{
    public GameObject couch;
    public Text price;
    public Button button;
    public Text buttonText;
    public Sprite[] couchesImages;
    public List<string> string_index;

    /// <summary>
    /// 设置商品UI
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(IapItem item) {
        couch.GetComponent<Image>().sprite = couchesImages[string_index.IndexOf(item.itemId)];
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
