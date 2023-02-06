using UnityEngine;
using System.Collections;

public class IapItem
{
    /// <summary>
    /// 商品ID
    /// </summary>
    public string itemId;
    /// <summary>
    /// 金币价格
    /// </summary>
    public int price;
    /// <summary>
    /// 状态 0-未解锁 1-已解锁 2-在使用
    /// </summary>
    public IapItemStatusEnum status;

    public IapItem(string  itemId, int price, IapItemStatusEnum status) {
        this.itemId = itemId;
        this.price = price;
        this.status = status;
    }
}

