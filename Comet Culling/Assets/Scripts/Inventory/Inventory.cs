using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{

    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        for (int i = 0; i < 10; i++)
        {
            AddItem(new Item { itemType = Item.ItemType.Potion, amount = 1 });
        }
       
     

        Debug.Log("Inventory");
        Debug.Log(itemList.Count);


    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
    }
    
    public List<Item> GetItemList() 
    {
       return itemList;
    }


}
