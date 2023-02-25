using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class InventoryItemController : MonoBehaviour
{
    public GameObject Player;
   public  PlayerController playerController;

    Item item;

    public Button RemoveButton;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);


    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void UseItem()
    {
        
        switch (item.itemType) 
        {
            case Item.ItemType.Potion:
                playerController.IncreaseHealth(item.value);
                Debug.Log("using item");
                break;
            case Item.ItemType.Other:
                Debug.Log("using item2");
                playerController.IncreaseExp(item.value);
                break;
            default:
                break;
        }

        RemoveItem();
    }
}
