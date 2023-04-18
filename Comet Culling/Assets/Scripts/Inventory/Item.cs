using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemType itemType;
    public int amount;
    public enum ItemType
    {
        cropA,
        cropB,
        hoe,
        wateringCan,
        scythe,
        seedA,
        seedB
    }


    //Gets sprites for each item
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.cropA:
                return ItemAssets.Instance.cropASprite;
            case ItemType.cropB:
                return ItemAssets.Instance.cropBSprite;
            case ItemType.hoe:
                return ItemAssets.Instance.hoeSprite;
            case ItemType.wateringCan:
                return ItemAssets.Instance.wateringCanSprite;
            case ItemType.scythe:
                return ItemAssets.Instance.scytheSprite;
            case ItemType.seedA:
                return ItemAssets.Instance.seedASprite;
            case ItemType.seedB:
                return ItemAssets.Instance.seedBSprite;

        }
    }


}
