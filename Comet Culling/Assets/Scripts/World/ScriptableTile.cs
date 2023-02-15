// Functionality to change a tile
// Used to till the earth to be able to plant a crop
// Also hopefully will be able to use to plant a crop directly in the middle of a tile

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class ScriptableTile : ScriptableObject
{
    public TileBase[] tileArray;
}
