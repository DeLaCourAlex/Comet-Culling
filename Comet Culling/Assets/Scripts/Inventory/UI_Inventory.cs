using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using static UnityEditor.Progress;
using System;
using CodeMonkey.Utils;
using static Item;

public class UI_Inventory : MonoBehaviour
{

    private Inventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public PlayerController playerController;
    [SerializeField] GameObject ItemInfo;
    public bool isInventoryVisible { get; private set; } = false;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

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


        isInventoryVisible = !isInventoryVisible;    // Flip bool value when 'I' is pressed

        this.gameObject.SetActive(isInventoryVisible);
        //backgroundPanel.SetActive(isInventoryVisible);
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
            //TextMeshProUGUI itemText = itemSlotRectTransform.Find("ItemDIsc").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI itemInfo = ItemInfo.GetComponent<TextMeshProUGUI>();
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


            itemSlotRectTransform.GetComponent<Button_UI>().MouseOverFunc = () =>
            {

                switch (item.itemType)
                {
                    default:
                    case ItemType.cropA:
                        itemInfo.SetText("Lantern Plant \r\nLantern plant can be used to replenish 10% energy to the spaceship. " +
                            "\r\nIt gets its name from its shape, resembling an oil lantern. It attracts pollinators to its large inflorescence by " +
                            "performing a dance using side tendrils. ");
                        break;

                    case ItemType.cropB:
                        itemInfo.SetText("Taco Plant\r\nTaco plant can be used to replenish 20% energy to the spaceship. \r\nNamed after an ancient Earthian dish consisting of filling inside a corn flour tortilla shell. " +
                            "Taco plants are carnivorous and attract their prey using bioluminescent appendices covered in a sticky sap. After something touches them, the plant closes and begins to digest the catch. ");
                        break;

                    case ItemType.hoe:
                        itemInfo.SetText("Hoe\r\nUsed to prepare the soil for planting seeds. \r\nHoes have been a popular agricultural hand tool for ages. Humans may have conquered space travel, but still haven’t found a way to improve them. ");
                        break;

                    case ItemType.wateringCan:
                        itemInfo.SetText("Watering Can \r\nUsed to help your crops grow faster. \r\nIt separates hydrogen gas and oxygen gas from the atmosphere. \r\nIn short, it uses science to never run out of water!");
                        break;

                    case ItemType.scythe:
                        itemInfo.SetText("Scythe\r\nUse in order to harvest mature crops. " +
                            "\r\nA curved blade connected to a telescopic handle, which can be collapsed for easy storage." +
                            " Simple tool but makes you look cool while in use! ");
                        break;

                    case ItemType.seedA:
                        itemInfo.SetText("Lantern plant seeds. Takes half a day to mature if watered.");
                        break;

                    case ItemType.seedB:
                        itemInfo.SetText("Taco plant seeds. Takes a day to mature if watered.");
                        break;


                }

                Debug.Log("middle clicking is working");
            };

            itemSlotRectTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () =>
            {

                switch (item.itemType)
                {
                    default:
                    case ItemType.cropA:
                        itemInfo.SetText("");
                        break;

                    case ItemType.cropB:
                        itemInfo.SetText("");
                        break;

                    case ItemType.hoe:
                        itemInfo.SetText("");
                        break;

                    case ItemType.wateringCan:
                        itemInfo.SetText("");
                        break;

                    case ItemType.scythe:
                        itemInfo.SetText("");
                        break;

                    case ItemType.seedA:
                        itemInfo.SetText("");
                        break;

                    case ItemType.seedB:
                        itemInfo.SetText("");
                        break;


                }

                Debug.Log("mouse out is working");
            };




            x++;
            if (x >= 4)
            {
                x = 0;
                y++;

            }
        }
    }
}