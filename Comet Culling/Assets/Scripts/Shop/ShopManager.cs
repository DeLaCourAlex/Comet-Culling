using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private Action<Item> useItemAct; 
    //Create a list that stores merchant item info
    public List<Item> shopItemList;
    //VARIABLES
    //Booleans that refer to each trade possibility 
    bool seedATrade, seedBTrade;

    
    void Start()
    {

        seedATrade = false; seedBTrade = false;
        UpdateStock();
        Debug.Log("Shop item count:" + shopItemList.Count);
    }


    //INVENTORY TRADE CHECK
    //first, check possible trades when opening shop keep
    // have a bool for each trade (is trade possible - for each shop item - so seeds and batteries)
    void CheckPossibleTrades(ref Inventory inventory)
    {
        int totalCropA, totalCropB/*, totalCropC*/;  
        //Find all is a method that returns a list of all the elements matching the predicate. 
        //TotalcropA/B will be the count of the returned list. 
        totalCropA = inventory.GetItemList().FindAll(delegate(Item item) { return item.itemType == Item.ItemType.cropA; }).Count;
        totalCropB = inventory.GetItemList().FindAll(delegate (Item item) { return item.itemType == Item.ItemType.cropB; }).Count;
        //totalCropB = inventory.GetItemList().FindAll(delegate (Item item) { return item.itemType == Item.ItemType.cropC; }).Count;

        //Checks for each possible trade. This is very rusty logic that can be optimized but uhh KISS
        if(totalCropB >= 4)
        {
            seedATrade = true; 
        }
        else
        {
            seedATrade = false;
        }

        if(totalCropA >= 2 && totalCropB >= 4)
        {
            seedBTrade = true; 
        }
        else
        {
            seedBTrade = false; 
        }
    }
    //first, check possible trades when opening shop keep
    // have a bool for each trade (is trade possible - for each shop item - so seeds and batteries)
    //the function would be CheckPossTrades(ref playerInventory)
    //THIS WILL NEED TO CHECK AFTER EACH TRADE AS WELL
    //{
    //declare int totalCropA, totalCropB etc... -> this'd be private (can pull this info from the inventory's list)
    //iterate through the player inventory list, counting the total of each unique crops
    //
    //checkif each trade is possible, enable ones that the player has the right crops for 
    //}
    //TALK TOMORROW: WHETHER MERCHANT RUNS OUT OF STOCK OR DOESN'T. If he does, this check will need to be applied to the shop as well

    //trade 1 example 
    //A seeds for 2B crops
    //ONCLICK UI BUTTON CHECK IF TRADE WAS MARKED AS POSSIBLE IF TRUE THEN CALL TRADE FUNC
    //TradeAFor2B( REF playerInventory to modify the player inventory)
    // {
    // merchantInventory remove 1A seed
    //  playerInvRef add 2B crops
    // call check PossTrades as inv has changed
    // }
    //There would be one function for each type of trade (so for seeds and batteries - eg 6 in total)
    //((Get this working that way and then you can optimise somehow. maybe using switch statements or polymorphism of some sort))

    public void Trade(Item tradedItem, ref Inventory inventory) 
    {//itemType is the type of the item you want to BUY FROM THE MERCHANT

        CheckPossibleTrades(ref inventory);

        switch (tradedItem.itemType)
        {
            case Item.ItemType.seedA:
                if (seedATrade) 
                {
                    inventory.AddItem(tradedItem);
                    //for(int i = 0; i < 4; i++)
                    //{
                    //    inventory.RemoveItem()
                    //}
                }
                break;
            case Item.ItemType.seedB:
                if (seedBTrade)
                {
                    inventory.AddItem(tradedItem);
                }
                break;
        }

        CheckPossibleTrades(ref inventory);

    }

    public void UpdateStock() 
    {
        shopItemList.Add(new Item { itemType = Item.ItemType.seedA, amount = 10 });
        shopItemList.Add(new Item { itemType = Item.ItemType.seedB, amount = 10 });

    }

    //public List<Item> GetShopItemList()
    //{
    //    return shopItemList;
    //}


}

