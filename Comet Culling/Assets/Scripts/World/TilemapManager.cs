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
    private void Awake()
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
        /*// Used to correct for rounding down when converting position to ints
        // We want to round down, but setting a float to an int only removes after the decimal place
        // This causes the number to round up if the float value is negative
        int yCorrector = position.y < 0 ? -1 : 0;
        int xCorrector = position.x < 0 ? -1 : 0;

        // Set the player position to ints because that's what the SetTile function uses
        Vector3Int positionInt = new Vector3Int((int)position.x + xCorrector, (int)position.y + yCorrector, 0);*/

        // Get the tile at the player's current position
        TileBase tileToTill = tilemap.GetTile(position);

        // If there is a dirt tile at the players position, set it to tilled
        if(tileToTill != null)
        {
            //Debug.Log("int position: " + position);
            tilemap.SetTile(position, tilledTile);
            DataPermanence.Instance.tilledTilePositions.Add(position);
        }
    }

    // Plant a crop in the center of a tile if it's been tilled
    public void PlantCrop(Vector3Int position, Vector2 cropPosition, int cropElement)
    {
        /*// Used to correct for rounding down when converting position to ints
        // We want to round down, but setting a float to an int only removes after the decimal place
        // This causes the number to round up if the float value is negative
        int yCorrector = position.y < 0 ? -1 : 0;
        int xCorrector = position.x < 0 ? -1 : 0;

        // Set the player position to ints because that's what the SetTile function uses
        Vector3Int positionInt = new Vector3Int((int)position.x + xCorrector, (int)position.y + yCorrector, 0);*/

        // Get the tile at the player's current position
        TileBase tileToPlant = tilemap.GetTile(position);

        /*// Check if there is a crop game object in the current position
        // Cycle through all the current crops that have been planted
        // And check their position against  where you are trying to plant a new crop
        GameObject[] allCrops = GameObject.FindGameObjectsWithTag("Crop");*/

        // Set the position of the new crop in the center of the tile
        //Vector2 cropPosition = new Vector2((float)position.x + 0.5f, (float)position.y + 0.5f);

        /*// If there is another crop in this position, leave the function
        for (int i = 0; i < allCrops.Length; i++)
            if (Vector2.Distance(cropPosition, allCrops[i].transform.position) == 0)
                return;*/

        // If there is a tilled dirt tile at the players position, plant the crop
        if (tileToPlant != null && tileData[tileToPlant].isTilled)
            Instantiate(tileData[tileToPlant].crops[cropElement], cropPosition, Quaternion.identity);
    }

    // Check if the current tile is tilled
    // Used to control player animations
    public bool IsTilled(Vector3Int position)
    {
        /*// Used to correct for rounding down when converting position to ints
        // We want to round down, but setting a float to an int only removes after the decimal place
        // This causes the number to round up if the float value is negative
        int yCorrector = position.y < 0 ? -1 : 0;
        int xCorrector = position.x < 0 ? -1 : 0;

        // Set the player position to ints because that's what the SetTile function uses
        Vector3Int positionInt = new Vector3Int((int)position.x + xCorrector, (int)position.y + yCorrector, 0);*/

        // Get the tile at the player's current position
        TileBase tileToCheck = tilemap.GetTile(position);

        return tileToCheck != null && tileData[tileToCheck].isTilled;
    }

    // Reset a tile to untilled after its crop has been harvested
    public void ResetTile(Vector3Int position)
    {
        /*// Used to correct for rounding down when converting position to ints
        // We want to round down, but setting a float to an int only removes after the decimal place
        // This causes the number to round up if the float value is negative
        int yCorrector = position.y < 0 ? -1 : 0;
        int xCorrector = position.x < 0 ? -1 : 0;

        // Set the player position to ints because that's what the SetTile function uses
        Vector3Int positionInt = new Vector3Int((int)position.x + xCorrector, (int)position.y + yCorrector, 0);*/

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
