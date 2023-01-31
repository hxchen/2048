using System.Collections.Generic;
using FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using static FullSerializer.fsAotVersionInfo;

public class CouchIap : MonoBehaviour
{
    #region UI
    public GameObject couch;
    public Text price;
    public Button button;
    public Text buttonText;
    public Sprite[] couchesImages;
    public List<string> string_index;
    #endregion

    #region 业务
    private IapItem iapItem;
    #endregion

    /// <summary>
    /// 设置商品UI
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(IapItem item) {
        iapItem = item;
        couch.GetComponent<Image>().sprite = couchesImages[string_index.IndexOf(item.itemId)];
        price.text = item.price.ToString();
        switch (item.status) {
            case IapItemStatusEnum.Locked:
                buttonText.text = "Unlock";
                break;
            case IapItemStatusEnum.Unlock:
                buttonText.text = "Use";
                break;
            case IapItemStatusEnum.Using:
                buttonText.text = "Using";
                break;
        }
    }
    /// <summary>
    /// 商店按钮事件
    /// 按照状态变化：Locked(显示Unlock) -> Unlock(显示Use) -> Using
    /// </summary>
    public void OnBuyButtonPressed() {
        Debug.Log($"购买:{iapItem}");
        var serializer = new fsSerializer();
        var shopConfiguration = FileUtils.LoadJsonFile<ShopConfiguration>(serializer, "Config/shop_configuration");

        switch (iapItem.status){
            case IapItemStatusEnum.Locked:
                // 没有解锁的进行解锁
                // TODO 扣除金币
                Debug.Log("TODO :扣除金币");
                iapItem.status = IapItemStatusEnum.Unlock;
                buttonText.text = "Use";
                shopConfiguration.UpdateIapItem(iapItem);
                break;
            case IapItemStatusEnum.Unlock:
                // 1.查找在使用的变为已解锁 using -> unlock
                CouchIap[] iapItemsArray = GameObject.FindObjectsOfType<CouchIap>();
                foreach (CouchIap couchIap in iapItemsArray) {
                    if (couchIap.iapItem.status == IapItemStatusEnum.Using) {
                        couchIap.iapItem.status = IapItemStatusEnum.Unlock;
                        couchIap.buttonText.text = "Use";
                        shopConfiguration.UpdateIapItem(couchIap.iapItem);
                    }
                }
                // 2.解锁过的变为使用
                Debug.Log("TODO :保存为使用中");
                iapItem.status = IapItemStatusEnum.Using;
                buttonText.text = "Using";
                shopConfiguration.UpdateIapItem(iapItem);
                CouchManager.instance.CreateCouchByItemId(iapItem.itemId);
                break;

        }
    }



}
