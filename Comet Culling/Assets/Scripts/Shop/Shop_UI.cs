using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System;
using CodeMonkey.Utils;

public class Shop_UI : MonoBehaviour
{
    public Inventory playerInventory;
    public ShopManager shopManager; 
    public Transform shopSlotContainer;
    public Transform shopSlotTemplate;
    public PlayerController playerController;
    [SerializeField] GameObject shopBackgroundPanel;
    bool isShopVisible = false; //For shop to open/close
    bool isTrading;
    bool confirmTrade; 
    private Item selectedItem; 


    private void Start()
    {
        isTrading = false;
        confirmTrade = false; 
    }

    //Enable shop access to the player 
    public void SetPlayer(PlayerController playerController, ShopManager shopMgr)
    {
        this.playerController = playerController;
        
    }


    //opens shop upon talking to the merchant
    public void ToggleShop()
    {
        if (isTrading) //Trade option is selected
        {
            isShopVisible = true; //Make shop visible upon selecting trade option
            if(/*shop slot selected basically &&*/ confirmTrade){

                shopManager.Trade(selectedItem, ref playerInventory);
            }
            this.gameObject.SetActive(isShopVisible);
            shopBackgroundPanel.SetActive(isShopVisible);
        }
        else //Back button is pressed for example
        {
            isShopVisible = false; //Make shop visible upon selecting trade option

            this.gameObject.SetActive(!isShopVisible);
            shopBackgroundPanel.SetActive(!isShopVisible);
        }
    }

    
}