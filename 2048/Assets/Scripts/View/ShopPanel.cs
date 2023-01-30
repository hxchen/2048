using FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    #region UI 组件
    public Text coinsNumberText;

    public GameObject items;

    public GameObject couchIapPrefab;
    #endregion

    private ShopConfiguration shopConfiguration;


    void Awake() {
        var serializer = new fsSerializer();
        shopConfiguration = FileUtils.LoadJsonFile<ShopConfiguration>(serializer, "Config/shop_configuration");
        if (shopConfiguration != null && shopConfiguration.iapItems.Count > 0) {
            foreach (IapItem item in shopConfiguration.iapItems) {
                AddItemToItems(item);
            }
        }
        
    }
    /// <summary>
    /// 商店关闭
    /// </summary>
    public void OnCloseButtonPressed() {
        Debug.Log("关闭Shop");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 添加商品到items组件
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToItems(IapItem item) {
        GameObject gameObject = GameObject.Instantiate(couchIapPrefab, items.transform);
        gameObject.GetComponent<CouchIap>().SetItem(item);
    }
}
