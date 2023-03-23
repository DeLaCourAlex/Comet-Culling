using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    // Create an instance of the class to allow to call its functions statically
    public static TilemapManager Instance;

    // A reference to the tilemap
    public Tilemap tilemap;
    // References to the untilled and tilled tile
    [SerializeField] Tile untilledTile;
    [SerializeField] Tile tilledTile;

    // A list of the scriptable object tiles
    [SerializeField] List<ScriptableTile> scriptableTiles;

    // A dictionary to connect data from scriptable tiles to the tiles
    Dictionary<TileBase, ScriptableTile> tileData;

    // Called when the script object is initialized
    private void Start()
    {
        Instance = this;

        // Initialize the tile data dictionary
        tileData = new Dictionary<TileBase, ScriptableTile>();

        // Fill the dictionary with data using the tile as a key
        // This allows to access the data stored at each tile
        foreach (ScriptableTile data in scriptableTiles)
            tileData.Add(data.tile, data);

        // If entering the farming scene, check if any dirt tiles have previously been tilled and set them
        foreach (Vector3Int position in DataPermanence.Instance.tilledTilePositions)
            tilemap.SetTile(position, tilledTile);
    }

    // Till a tile, allowing it to have crops planted
    public void TillDirt(Vector3Int position)
    {
        // Get the tile at the player's current position
        TileBase tileToTill = tilemap.GetTile(position);

        // If there is a dirt tile at the players position, set it to tilled
        if(tileToTill != null)
        {
            tilemap.SetTile(position, tilledTile);
            DataPermanence.Instance.tilledTilePositions.Add(position);
        }
    }

    // Plant a crop in the center of a tile if it's been tilled
    public void PlantCrop(Vector3Int position, Vector2 cropPosition, int cropElement)
    {
        // Get the tile at the player's current position
        TileBase tileToPlant = tilemap.GetTile(position);

        // If there is a tilled dirt tile at the players position, plant the crop
        if (tileToPlant != null && tileData[tileToPlant].isTilled)
            Instantiate(tileData[tileToPlant].crops[cropElement], cropPosition, Quaternion.identity);
    }

    // Check if the current tile is tilled
    // Used to control player animations
    public bool IsTilled(Vector3Int position)
    {
        // Get the tile at the player's current position
        TileBase tileToCheck = tilemap.GetTile(position);

        return tileToCheck != null && tileData[tileToCheck].isTilled;
    }

    // Reset a tile to untilled after its crop has been harvested
    public void ResetTile(Vector3Int position)
    {
        // Get the tile at the player's current position
        TileBase tileToPlant = tilemap.GetTile(position);

        // If there is a tilled dirt tile at the players position, set the tile to untilled
        if (tileToPlant != null)
        {
            tilemap.SetTile(position, untilledTile);

            // Remove the tilled tile from data permanence
            if (DataPermanence.Instance.tilledTilePositions.Contains(position))
                DataPermanence.Instance.tilledTilePositions.Remove(position);
        }
    }
}
