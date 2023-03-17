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

}
