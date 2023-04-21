using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System;
using CodeMonkey.Utils;
using static Item; 

public class Shop_UI : MonoBehaviour
{
    public Inventory playerInventory;
    private ShopManager shopManager; 
    public Transform shopSlotContainer;
    public Transform shopSlotTemplate;
    public PlayerController playerController;
    //[SerializeField] GameObject shopBackgroundPanel;
    bool isShopVisible = false; //For shop to open/close
    bool isTrading;
    bool confirmTrade; 
    private Item selectedItem;

    //private void Awake()
    //{
    //    shopManager = GameObject.FindObjectOfType<ShopManager>(); 

    //}
    private void Start()
    {
        isTrading = false;
        confirmTrade = false;
        
    }

    //Enable shop access to the player 
  

    //Enable shop access to shop manager
    public void SetShop(ShopManager shopMgr, Inventory inventory, PlayerController playerController)
    {
        this.shopManager = shopMgr;
        this.playerInventory = inventory;
        this.playerController = playerController;
        RefreshShopItems(); 

    }

    public void ToggleTrade()
    {
        //isTrading = !isTrading; //Flips bool when K is pressed (testing)

        
    }

    //opens shop upon talking to the merchant
    public void OpenShop()
    {
        //if (isTrading) //Trade option is selected
        //{
        //    isShopVisible = true; //Make shop visible upon selecting trade option
        //    if(/*shop slot selected basically &&*/ confirmTrade){

        //        shopManager.Trade(selectedItem, ref playerInventory);
        //    }
        //    this.gameObject.SetActive(isShopVisible);
        //    shopBackgroundPanel.SetActive(isShopVisible);
        //}
        //else //Back button is pressed for example
        //{
        //    isShopVisible = false; //Make shop visible upon selecting trade option

        //    this.gameObject.SetActive(!isShopVisible);
        //    shopBackgroundPanel.SetActive(!isShopVisible);
        //}

        isShopVisible = !isShopVisible;
        this.gameObject.SetActive(isShopVisible);
        //shopBackgroundPanel.SetActive(isShopVisible);
        Debug.Log("Shop toggle is being called");
        Debug.Log("Shop item count:" + shopManager.shopItemList.Count);
        //foreach(Item item in shopManager.shopItemList)
        //{
        //    Debug.Log("Shop item count:" + item.ToString());
        //}



    }

    private void RefreshShopItems()
    {
        //foreach (Transform child in shopSlotContainer)
        //{
        //    if (child == shopSlotTemplate) continue;
        //    Destroy(child.gameObject);
        //}

        //int x = 0;
        //int y = 0;
        //float itemSlotCellSize = 30f;

        //foreach (Item item in shopManager.GetShopItemList())
        //{

        //    RectTransform itemSlotRectTransform = Instantiate(shopSlotTemplate, shopSlotContainer).GetComponent<RectTransform>();
        //    itemSlotRectTransform.gameObject.SetActive(true);

        //    //Upon clicking on the shop item
        //    itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
        //    {
        //        shopManager.Trade(item, ref playerInventory); //Trade
        //        Debug.Log("trade is working");
        //    };

        //    itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
        //    Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
        //    image.sprite = item.GetSprite();
        //    TextMeshProUGUI itemText = itemSlotRectTransform.Find("ItemDIsc").GetComponent<TextMeshProUGUI>();
        //    TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();

        //    ////activates the stacking text if item amount is more than 1 
        //    //if (item.amount > 1)
        //    //{
        //    //    uiText.SetText(item.amount.ToString());
        //    //}
        //    //else
        //    //{
        //    //    uiText.SetText("");
        //    //}

        //    x++;
        //    if (x >= 4)
        //    {
        //        x = 0;
        //        y++;

        //    }
        //}
    }
}