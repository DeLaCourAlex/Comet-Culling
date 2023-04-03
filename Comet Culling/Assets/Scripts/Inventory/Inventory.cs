using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    //creates a list of items to hold items in inventory 
    private List<Item> itemList;
    //Action delegate to encapsulate the useItem function
    private Action<Item> useItemAction;
    internal static object gameObject;


    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
        //AddItem(new Item { itemType = Item.ItemType.cropA, amount = 3});
        Debug.Log("Inventory is working");
        Debug.Log(itemList.Count);

    }

    //adds item to inventory(stacking)
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

        OnItemListChanged?.Invoke(this, EventArgs.Empty);

    }



    //removes item from inventory
    public void RemoveItem(Item item)
    {
        Item itemInInventory = null;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.itemType == item.itemType)
            {
                inventoryItem.amount -= item.amount;
                itemInInventory = inventoryItem;
                //itemList.Remove(itemInInventory);
            }
        }

        if (itemInInventory != null && itemInInventory.amount <= 0)
        {
            itemList.Remove(itemInInventory);
        }
        else
        {
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);

    }

    //function to use item on mouse click 
    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    //gets and returns the item list 
    public List<Item> GetItemList()
    {
        return itemList;
    }
}