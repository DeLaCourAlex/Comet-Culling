using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    /// <summary>
    /// creating a list to hold items
    /// </summary>
    public List<Item> items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// adding items to the list
    /// </summary>
    /// <param name="item"></param>
    public void Add(Item item)
    {
        items.Add(item);
    }
    /// <summary>
    /// removes items from list
    /// </summary>
    /// <param name="item"></param>
    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Item item in items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite= item.icon;
        }

    }

}