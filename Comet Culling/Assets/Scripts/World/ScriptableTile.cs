// Functionality to change a tile
// Used to till the earth to be able to plant a crop
// Also hopefully will be able to use to plant a crop directly in the middle of a tile

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Scriptable Tile", menuName = "Scriptable Tile")]
public class ScriptableTile : ScriptableObject
{
    // The tile that this data will attach to
    public TileBase tile;

    // Sprites to display if a dirt tile has been tilled or not
    //[SerializeField] Sprite tilledDirt;
    //[SerializeField] Sprite untilledDirt;

    // Used to perform a check to see if the current tile is tilled
    public bool isTilled;

    // The crop to be planted on the tile
    public GameObject[] crops;
}
