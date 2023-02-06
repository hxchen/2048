using FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : View
{
    #region UI 组件

    public GameObject items;

    public GameObject couchIapPrefab;
    #endregion


    void Awake() {
        var items = CouchManager.instance.GetIapItems();
        foreach (IapItem item in items) {
            UpdateItemUI(item);
        }
    }

    /// <summary>
    /// 添加商品到items组件
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItemUI(IapItem item) {
        GameObject gameObject = GameObject.Instantiate(couchIapPrefab, items.transform);
        gameObject.GetComponent<CouchIap>().SetItem(item);
    }

    
}
