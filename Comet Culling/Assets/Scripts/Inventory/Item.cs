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
        cropC, 

        seedsA,
        seedsB,
        seedsC,

        batteryA,
        batteryAA,
        batteryAAA
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
    //public bool IsStackable()
    //{
    //    switch (itemType)
    //    {

    //        case ItemType.cropA:
    //            return true;
    //        case ItemType.cropB:
    //            return true;
    //        default:
    //            return false;
    //    }

    //}
 

    public int GetBattery(ItemType type) {//Method that returns a value depending on the battery type
        //This value would then get added to the stamina
        switch (type)
        {
            case ItemType.batteryA:
                return 75;
            case ItemType.batteryAA:
                return 50;
            case ItemType.batteryAAA:
                return 25;
            default:
                return 0; 
        }
    }
}