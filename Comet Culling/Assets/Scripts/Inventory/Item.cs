using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
   public ItemType itemType;
   public int amount;
    public enum ItemType
    {
        cropA,
        cropB
    }

   public Sprite GetSprite()
    {
        switch (itemType) 
        {
            default: 
            case ItemType.cropA:
                return ItemAssets.Instance.cropASprite; 

            case ItemType.cropB:
                return ItemAssets.Instance.cropBSprite;
        }
    }




}
