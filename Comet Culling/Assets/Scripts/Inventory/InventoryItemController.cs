using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class InventoryItemController : MonoBehaviour
{
    public GameObject Player;
    public  PlayerController playerController;

    Item item;
    [Header("UI")]
    public TextMeshProUGUI countText;
    public int count = 1;
    
    public Button RemoveButton;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
    }


    public int Count
    {
        get
        { return count; }

        set
        {
            if (value > 9)
            {
                count = 9;
            }
            else
            {
                count = value;
            }

        }
    }


    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);


    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 0;
        countText.gameObject.SetActive(textActive);
        Debug.Log(count);

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
