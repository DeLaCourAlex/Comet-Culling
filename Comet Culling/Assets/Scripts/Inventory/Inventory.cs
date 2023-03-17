using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    internal static object gameObject;

    public Inventory()
    {
        itemList = new List<Item>();
        AddItem(new Item { itemType = Item.ItemType.cropA, amount = 3});
        AddItem(new Item { itemType = Item.ItemType.cropB, amount = 3});
        Debug.Log("Inventory");
        Debug.Log(itemList.Count);
    }

    public void AddItem(Item item)
    {
      
            Debug.Log("Stacking");
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        else
        {
            Debug.Log("Items are adding");
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

    }

//        


public List<Item> GetItemList()
    {
        return itemList;
    }
}

