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
    public int count;
    
    public Button RemoveButton;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();

        UpdatedText();
    }

  



    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
      

    }

    public void AddItem(Item newItem)
    {


        item = newItem;
        IncrementCounter();
        UpdatedText();

    }

    public int IncrementCounter()
    {
        count = count +1;
        
         Debug.Log("count value is " + count);
        return count;
    }


    public void UpdatedText()
    {
       
        bool textActive = count > 0;
        countText.gameObject.SetActive(textActive); 
        countText.text = count.ToString();
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
