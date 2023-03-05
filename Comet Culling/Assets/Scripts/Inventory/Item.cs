
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    //item data 
    public int id;
    public string itemName;
    public int value;
    public int count;
    public Sprite icon;
    public ItemType itemType;
   
    //creates diffrent types of items in the form of enums 
    public enum ItemType
    {
        Potion,
        Other
    }
  
}