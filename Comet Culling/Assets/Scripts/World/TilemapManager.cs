using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    // Create an instance of the class to allow to call its functions statically
    public static TilemapManager Instance;

    // A reference to the tilemap
    [SerializeField] Tilemap tilemap;
    // References to the untilled and tilled tile
    [SerializeField] Tile untilledTile;
    [SerializeField] Tile tilledTile;

    // A list of the scriptable object tiles
    [SerializeField] List<ScriptableTile> scriptableTiles;

    // A dictionary to connect data from scriptable tiles to the tiles
    Dictionary<TileBase, ScriptableTile> tileData;

    // Called when the script object is initialized
    private void Awake()
    {
        Instance = this;

        // Initialize the tile data dictionary
        tileData = new Dictionary<TileBase, ScriptableTile>();

        // Fill the dictionary with data using the tile as a key
        // This allows to access the data stored at each tile
        foreach (ScriptableTile data in scriptableTiles)
            tileData.Add(data.tile, data);
    }

    // Till a tile, allowing it to have crops planted
    public void TillDirt(Vector2 position)
    {
        // Used to correct for rounding down when converting position to ints
        // We want to round down, but setting a float to an int only removes after the decimal place
        // This causes the number to round up if the float value is negative
        int yCorrector = position.y < 0 ? -1 : 0;
        int xCorrector = position.x < 0 ? -1 : 0;

        // Set the player position to ints because that's what the SetTile function uses
        Vector3Int positionInt = new Vector3Int((int)position.x + xCorrector, (int)position.y + yCorrector, 0);

        // Get the tile at the player's current position
        TileBase tileToTill = tilemap.GetTile(positionInt);

        // If there is a dirt tile at the players position, set it to tilled
        if(tileToTill != null)
        {
            Debug.Log("int position: " + positionInt);
            tilemap.SetTile(positionInt, tilledTile);
        }
    }

    // Plant a crop in the center of a tile if it's been tilled
    public void PlantCrop(Vector2 position)
    {
        // Used to correct for rounding down when converting position to ints
        // We want to round down, but setting a float to an int only removes after the decimal place
        // This causes the number to round up if the float value is negative
        int yCorrector = position.y < 0 ? -1 : 0;
        int xCorrector = position.x < 0 ? -1 : 0;

        // Set the player position to ints because that's what the SetTile function uses
        Vector3Int positionInt = new Vector3Int((int)position.x + xCorrector, (int)position.y + yCorrector, 0);

        // Get the tile at the player's current position
        TileBase tileToPlant = tilemap.GetTile(positionInt);

        // Check if there is a crop game object in the current position
        // Cycle through all the current crops that have been planted
        // And check their position against  where you are trying to plant a new crop
        GameObject[] allCrops = GameObject.FindGameObjectsWithTag("Crop");

        // Set the position of the new crop in the center of the tile
        Vector2 cropPosition = new Vector2((float)positionInt.x + 0.5f, (float)positionInt.y + 0.5f);

        // If there is another crop in this position, leave the function
        for (int i = 0; i < allCrops.Length; i++)
            if (Vector2.Distance(cropPosition, allCrops[i].transform.position) == 0)
                return;

        // If there is a tilled dirt tile at the players position, plant the crop
        if (tileToPlant != null && tileData[tileToPlant].isTilled)
            Instantiate(tileData[tileToPlant].crop, cropPosition, Quaternion.identity);
    }

    // Reset a tile to untilled after its crop has been harvested
    public void ResetTile(Vector2 position)
    {
        // Used to correct for rounding down when converting position to ints
        // We want to round down, but setting a float to an int only removes after the decimal place
        // This causes the number to round up if the float value is negative
        int yCorrector = position.y < 0 ? -1 : 0;
        int xCorrector = position.x < 0 ? -1 : 0;

        // Set the player position to ints because that's what the SetTile function uses
        Vector3Int positionInt = new Vector3Int((int)position.x + xCorrector, (int)position.y + yCorrector, 0);

        // Get the tile at the player's current position
        TileBase tileToPlant = tilemap.GetTile(positionInt);

        // If there is a tilled dirt tile at the players position, set the tile to untilled
        if (tileToPlant != null)
            tilemap.SetTile(positionInt, untilledTile);
    }
}
