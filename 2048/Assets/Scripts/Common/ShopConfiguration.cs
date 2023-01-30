using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopConfiguration
{

    public List<IapItem> iapItems = new List<IapItem>();

    public ShopConfiguration() {
        //init();
    }

    public void init() {
        Debug.Log("存储数据");
        iapItems.Add(new IapItem("couch_1", 0, IapItemStatusEnum.Using));
        iapItems.Add(new IapItem("couch_2", 999, IapItemStatusEnum.Locked));
        iapItems.Add(new IapItem("couch_3", 1499, IapItemStatusEnum.Locked));
        iapItems.Add(new IapItem("couch_4", 1999, IapItemStatusEnum.Locked));
        iapItems.Add(new IapItem("couch_5", 2499, IapItemStatusEnum.Locked));
        iapItems.Add(new IapItem("couch_6", 2999, IapItemStatusEnum.Locked));
        iapItems.Add(new IapItem("couch_7", 3499, IapItemStatusEnum.Locked));
        iapItems.Add(new IapItem("couch_8", 9999, IapItemStatusEnum.Locked));
        FileUtils.SaveJsonFile(Application.dataPath + "/Resources/Config/shop_configuration.json", this);
    }
}