using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class InventoryItemController : MonoBehaviour
{
    [SerializeField] GameObject Player;
    PlayerController playerController;

    Item item;

    public Button RemoveButton;
    void Start()
    {
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
                break;
            case Item.ItemType.Other:
                playerController.IncreaseExp(item.value);
                break;
            default:
                break;
        }

        RemoveItem();
    }
}
