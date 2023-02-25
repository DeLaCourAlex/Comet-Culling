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

    public Toggle EnableRemove;
    //inventory item array
    public InventoryItemController[] InventoryItems;
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
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].name == item.name)
            {
                ++Items[i].itemAmount;
                //return true;
            }
        }

        Items.Add(item);

    }
    /// <summary>
    /// removes items from list
    /// </summary>
    /// <param name="item"></param>
    public void Remove(Item item)
    {
        Items.Remove(item);

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
