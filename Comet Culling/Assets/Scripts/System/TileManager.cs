using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    //Allow objects to be dragged and dropped in while remaining private
    [SerializeField] private Tilemap interactible;
    [SerializeField] private Tile hiddenInteractible;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var position in interactible.cellBounds.allPositionsWithin)
        {
            interactible.SetTile(position, hiddenInteractible);
        }
    }

}
