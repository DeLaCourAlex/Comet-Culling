using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //private InventoryItemController inventoryController;

    public Item item;
    public AudioSource clickSound;
    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        clickSound.Play();
        Destroy(gameObject);

    }

    private void OnMouseDown()
    {
        Pickup();
    }


}
