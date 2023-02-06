using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopItems
{
    [SerializeReference]
    public List<IapItem> iapItems = new List<IapItem>();

    public void Add(IapItem item) {
        iapItems.Add(item);
    }

    public List<IapItem> GetIapItems() {
        return iapItems;
    }
}
