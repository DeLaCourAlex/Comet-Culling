using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Trading : MonoBehaviour
{
    public Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new Inventory(UseItem);
     
    }

    [YarnCommand("Trade")]
    
    //[visited()]
    //public void Trade(bool TradeItem)
    //{

        

           

    //    if (TradeItem)
    //    {
    //        inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = 2 });
    //        inventory.AddItem(new Item { itemType = Item.ItemType.seedB, amount = 2 });
    //    }
    //}

    public void UseItem(Item item)
    {
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
