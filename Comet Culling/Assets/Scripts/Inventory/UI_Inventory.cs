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
        //OpenInventory();
    }




    public void OpenInventory()
    {

        if (Input.GetKeyDown(KeyCode.I))
            isInventoryVisible = !isInventoryVisible;    // Flip bool value when 'I' is pressed

        this.gameObject.SetActive(isInventoryVisible);
    }


    public void SetInventory(Inventory inventory)
    {

        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

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

        Debug.Log("Item Count: " + inventory.GetItemList().Count);
        List<Item> list = inventory.GetItemList();
        for (int i = 0; i < list.Count; i++)
        {
            Item item = list[i];

            try
            {

                Debug.Log($"Item: {i} {item.itemType}");

                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer)
                    .GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);



                itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => 
                {
                    //use Item
                    inventory.RemoveItem(item);
                    //playerController.cropsHarvested[0]++;


                };

               


                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
                Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
                image.sprite = item.GetSprite();
                TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
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
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }
    }
}
