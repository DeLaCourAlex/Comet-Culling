using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler onItemListChanged;

    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        //for (int i = 0; i < 2; i++)
        //{
        //    AddItem(new Item { itemType = Item.ItemType.cropA, amount = 1 });
        //    AddItem(new Item { itemType = Item.ItemType.cropB, amount = 1 });
        //}

        Debug.Log("Inventory");
        Debug.Log(itemList.Count);
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlredyInINventory = false;

            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlredyInINventory = true;
                }
            }

            if (!itemAlredyInINventory)
            {
                itemList.Add(item);
            }
            else
            {
                itemList.Add(item);
            }
            onItemListChanged?.Invoke(this, EventArgs.Empty);

        }
    }

        public List<Item> GetItemList()
        {
            return itemList;
        }
    }
