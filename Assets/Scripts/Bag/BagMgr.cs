using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包管理器
/// </summary>
public class BagMgr : BaseManager<BagMgr>
{
    public List<Item> itemList = new List<Item>();

    public void InitItemsInfo() {
        for (int i = 0; i < 10000; i++)
        {
            Item item = new Item();
            item.id = i;
            item.num = i;
            itemList.Add(item);
        }
    }

}

public class Item {
    public int id;
    public int num;
}
