using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
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
    //calling InventoryItemController into the constructor and caching the instance
    private InventoryItemController inventoryController;




    //item count
    //public int count;
    public Transform ItemContent;
    public GameObject InventoryItem;
    private const int maxInventorySize = 9; // maximum number of stacks in the inventory
    //toggle for enabling remove button 
    public Toggle EnableRemove;
    //inventory item array
    public InventoryItemController[] InventoryItems;
    private void Awake()
    {
        Instance = this;
       

        //InventoryItemController obj = Instantiate(inventoryController);
    }

    public void Start()
    {

        inventoryController = new InventoryItemController();
        inventoryController = GetComponent<InventoryItemController>();

    }



    /// <summary>
    /// adding items to the list
    /// </summary>
    /// <param name="item"></param>
    public void Add(Item item)
    {

        foreach (Item InventoryItems in Items)
        {

            if (InventoryItems.itemType == item.itemType)

            {

                Debug.Log("items are stacking");
                inventoryController.count = inventoryController.count + 1;





                return;
            }
        }

        Items.Add(item);
        Debug.Log("items are being added");

    }





    //removes item from inventory 
    public void Remove(Item item)
    {
        Items.Remove(item);

    }


    //locates and lists items in the inventory
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
            //var count = obj.transform.Find("count").GetComponent<TMPro.TextMeshProUGUI>();

            itemName.text = item.itemName;
            //count.text = item.count;
            itemIcon.sprite = item.icon;

            if (EnableRemove.isOn)
            {
                removeButton.gameObject.SetActive(true);
            }


        }

        SetInventoryItems();

    }

    //enables item remove option 
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
    //Adds item to inventory list 
    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItems[i].AddItem(Items[i]);

        }

    }
}
