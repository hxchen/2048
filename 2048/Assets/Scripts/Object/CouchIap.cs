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
        var serializer = new fsSerializer();

        switch (iapItem.status){
            case IapItemStatusEnum.Locked:
                // 没有解锁的进行解锁
                // 扣除金币
                bool subtractCoinsRet = GameObject.Find("GameUICanvas/BallMain").GetComponent<BallMain>().SubtractCoins(iapItem.price);
                if (!subtractCoinsRet) {
                    return;
                }
                iapItem.status = IapItemStatusEnum.Unlock;
                buttonText.text = "Use";
                CouchManager.instance.UpdateIapItem(iapItem);
                break;
            case IapItemStatusEnum.Unlock:
                // 1.查找在使用的变为已解锁 using -> unlock
                CouchIap[] iapItemsArray = GameObject.FindObjectsOfType<CouchIap>();
                foreach (CouchIap couchIap in iapItemsArray) {
                    if (couchIap.iapItem.status == IapItemStatusEnum.Using) {
                        couchIap.iapItem.status = IapItemStatusEnum.Unlock;
                        couchIap.buttonText.text = "Use";
                        CouchManager.instance.UpdateIapItem(couchIap.iapItem);
                    }
                }
                // 2.解锁过的变为使用
                iapItem.status = IapItemStatusEnum.Using;
                buttonText.text = "Using";
                CouchManager.instance.UpdateIapItem(iapItem);
                CouchManager.instance.CreateCouchByItemId(iapItem.itemId);
                break;

        }
    }

}
