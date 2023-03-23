using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System;
using CodeMonkey.Utils;

public class UI_Inventory : MonoBehaviour
{

    private Inventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public PlayerController playerController;
    bool isInventoryVisible = false;

    private void Start()
    {
        
    }

    //sets the player to the UI_inventory scripts so it can aceses the player 
    public void SetPlayer(PlayerController playerController)
    {
        this.playerController = playerController;
    }


    //opens inventory upon key press
    public void OpenInventory()
    {

        if (Input.GetKeyDown(KeyCode.I))
            isInventoryVisible = !isInventoryVisible;    // Flip bool value when 'I' is pressed

        this.gameObject.SetActive(isInventoryVisible);
    }


    //instantiates and updates inventory 
    public void SetInventory(Inventory inventory)
    {

        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    //refresh inventory if item list has changed 
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    //refresh inventory
    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 30f;

        foreach (Item item in inventory.GetItemList())
        {

            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            //sets the mouse click to the useItem function  
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                //use Item
                inventory.UseItem(item);
                //inventory.RemoveItem(item);
                Debug.Log("clicking is working");
            };

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();

            //activates the stacking text if item amount is more than 1 
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }


            x++;
            if (x >= 4)
            {
                x = 0;
                y++;

            }
        }
    }
}
