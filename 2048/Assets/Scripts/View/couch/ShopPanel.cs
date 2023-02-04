using FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : View
{
    #region UI 组件

    public GameObject items;

    public GameObject couchIapPrefab;
    #endregion

    private ShopConfiguration shopConfiguration;


    void Awake() {
        var serializer = new fsSerializer();
        shopConfiguration = FileUtils.LoadJsonFile<ShopConfiguration>(serializer, "Config/shop_configuration");
        if (shopConfiguration != null && shopConfiguration.iapItems.Count > 0) {
            foreach (IapItem item in shopConfiguration.iapItems) {
                UpdateItemUI(item);
            }
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