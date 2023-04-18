using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public Sprite cropASprite;
    public Sprite cropBSprite;
    public Sprite hoeSprite;
    public Sprite wateringCanSprite;
    public Sprite scytheSprite;
    public Sprite seedASprite;
    public Sprite seedBSprite;



}
