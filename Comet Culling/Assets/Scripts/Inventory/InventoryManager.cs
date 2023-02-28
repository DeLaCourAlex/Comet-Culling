using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    /// <summary>
    /// creating a list to hold items
    /// </summary>
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    private const int maxInventorySize = 20; // maximum number of stacks in the inventory

    public int id; // unique item id
    public string name; // item name
    public int maxStack; // maximum stack size for this item
    public int quantity; // current quantity of the item

    public Toggle EnableRemove;
    //inventory item array
    public InventoryItemController[] InventoryItems;
    private void Awake()
    {
        Instance = this;

    }

    private bool AddNewStack(Item item, int id, string name, int maxStack, int quantity)
    {
      
        // check if there is space in the inventory for a new stack
        if (item.quantity < maxInventorySize)
        {
            // add a new stack for the item
            item.id = id;
            item.name = name;
            item.maxStack = maxStack;
            item.quantity = quantity;
            Items.Add(item);
            return true;
        }
        else
        {
            // inventory is full, cannot add a new stack
            return false;
        }
    }

    /// <summary>
    /// adding items to the list
    /// </summary>
    /// <param name="item"></param>
    public bool Add(Item item)
    {
        // check if the item is already in the inventory
         //Items = item.MaxStack;
        if (item != null)
        {
            // add to the stack if it hasn't reached the maximum stack size
            if (item.quantity <= item.maxStack)
            {

                item.quantity += quantity; 
                return true;
            }
            else
            {
                // if the stack is full, add a new stack if possible
                if (AddNewStack(item,id, name, maxStack, quantity))
                {
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            // add a new stack for the item
            return AddNewStack(item,id, name, maxStack, quantity);
        }

    }



    public bool RemoveItem(int id, int quantity)
    {
        Item item = GetItem(id);
        if (item != null)
        {
            // remove from the stack
            if (item.quantity >= quantity)
            {
                item.quantity -= quantity;
                if (item.quantity == 0)
                {
                    Items.Remove(item);

                }
                return true;
            }
            else
            {
                // not enough quantity in the stack
                return false;
            }
        }
        else
        {
            // item not found
            return false;
        }
    }

    public Item GetItem(int id)
    {
        // search for an item in the inventory
        GameObject obj = Instantiate(InventoryItem, ItemContent);

        foreach (Item item in Items)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        // item not found
        return null;
    }

    



    public void ListItems()
    {
        //cleans content before opening 
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);

        }

        foreach (Item item in Items)
        {
            //Debug.Log("item name and icon is not null");
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            if (EnableRemove.isOn)
            {
                removeButton.gameObject.SetActive(true);
            }




        }

        SetInventoryItems();

    }

    public void EnableItemsRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);

            }

        }
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);

            }

        }

    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for(int i =0; i < Items.Count; i++) 
        {

            InventoryItems[i].AddItem(Items[i]);

        }

    }
}
