using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;

public class CouchManager : MonoBehaviour
{
    public static CouchManager instance;

    public GameObject couchManager;

    public List<GameObject> couchPrefabs;
    // 商店itemId到prefab的索引
    public List<string> id_number_index;

    private ShopConfiguration shopConfiguration;

    void Awake() {
        instance = this;
        var serializer = new fsSerializer();
        shopConfiguration = FileUtils.LoadJsonFile<ShopConfiguration>(serializer, "Config/shop_configuration");
        IapItem iapItem = shopConfiguration.GetCurrentCouch();
        CreateCouchByItemId(iapItem.itemId);
    }

    /// <summary>
    /// 根据商店的ID创建couch
    /// </summary>
    /// <param name="itemId"></param>
    public void CreateCouchByItemId(string itemId) {
        // 先删除原有couch
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
        // 再创建新的couch
        GameObject couch = Instantiate(couchPrefabs[id_number_index.IndexOf(itemId)]);
        couch.transform.SetParent(couchManager.transform);
    }
}
