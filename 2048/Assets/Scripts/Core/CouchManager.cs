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

    private ShopItems shopItems;

    void Awake() {
        instance = this;
        if (!PlayerPrefs.HasKey(Const.ShopConfiguration)) {
            init();
        } else {
            shopItems = FileUtils.LoadJsonPrefs<ShopItems>(Const.ShopConfiguration);
        }
        IapItem currentItem = GetCurrentCouch();
        CreateCouchByItemId(currentItem.itemId);
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
    /// <summary>
    /// 商店内容初始化
    /// </summary>
    public void init() {
        shopItems = new ShopItems();
        shopItems.Add(new IapItem("couch_1", 0, IapItemStatusEnum.Using));
        shopItems.Add(new IapItem("couch_2", 500, IapItemStatusEnum.Locked));
        shopItems.Add(new IapItem("couch_3", 1000, IapItemStatusEnum.Locked));
        shopItems.Add(new IapItem("couch_4", 1500, IapItemStatusEnum.Locked));
        shopItems.Add(new IapItem("couch_5", 2000, IapItemStatusEnum.Locked));
        shopItems.Add(new IapItem("couch_6", 2500, IapItemStatusEnum.Locked));
        shopItems.Add(new IapItem("couch_7", 3000, IapItemStatusEnum.Locked));
        shopItems.Add(new IapItem("couch_8", 3500, IapItemStatusEnum.Locked));
        FileUtils.SaveJsonPrefs(Const.ShopConfiguration, shopItems);
    }

    public void initItem() {
        IapItem item = new IapItem("couch_7", 3499, IapItemStatusEnum.Locked);
        FileUtils.SaveJsonPrefs("couch_7", item);
    }

    public void UpdateIapItem(IapItem item) {
        shopItems.GetIapItems().Find((IapItem obj) => item.itemId == obj.itemId).status = item.status;
        FileUtils.SaveJsonPrefs(Const.ShopConfiguration, shopItems);
    }
    /// <summary>
    /// 获取当前选用的couch
    /// </summary>
    /// <returns></returns>
    public IapItem GetCurrentCouch() {
        return shopItems.GetIapItems().Find(iapItem => iapItem.status == IapItemStatusEnum.Using);
    }

    public List<IapItem> GetIapItems() {
        return shopItems.GetIapItems();
    }

}
