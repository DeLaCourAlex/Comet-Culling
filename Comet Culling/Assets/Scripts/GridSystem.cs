using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    // Dirt tiles - the tiles on which a crop can be planted
    // Give them their own layer mask and then set this in the inspector
    [SerializeField] LayerMask dirtTile;

    // Direction in which the player is currently facing
    Vector2 directionFacing;

    void PlantCrop(GameObject crop)
    {
        // Set a vector in the direction which the player is facing
        // Initialize to 0, 0
        Vector2 rayDirection = new Vector2(0, 0);

        // These vertical and horizontal directions are the direction in which the player is facing
        // Set depending on player movement

        // Check to see if the player if facing up or down
        if (directionFacing.y > 0)
            rayDirection = new Vector2(0, 1);
        else if (directionFacing.y < 0)
            rayDirection = new Vector2(0, -1);

        // If neither, check to see if the player is facing left or right
        else
        {
            if (directionFacing.x > 0)
                rayDirection = new Vector2(1, 0);
            else if (directionFacing.x < 0)
                rayDirection = new Vector2(-1, 0);
        }

        // Raycast looking to hit a "dirt tile" layer mask if the player is facing one and within half a unit
        // Can play around with that value
        RaycastHit2D cropRaycast = Physics2D.Raycast(transform.position, rayDirection, 0.5f, dirtTile);

        // If the raycast hits a dirt tile
        if (cropRaycast.collider != null)
        {
            // The position to plant the crop
            float cropPosX = 0f;
            float cropPosY = 0f;

            // Find the closest value to the raycast hit that sits at a .5 value
            // This ensures the crop is planted in the middle of a tile
            if (directionFacing.y > 0)
            {
                cropPosX = Mathf.Ceil(cropRaycast.point.x) - 0.5f;
                cropPosY = Mathf.Ceil(cropRaycast.point.y) + 0.5f;
            }
            else if (directionFacing.y < 0)
            {
                cropPosX = Mathf.Ceil(cropRaycast.point.x) - 0.5f;
                cropPosY = Mathf.Ceil(cropRaycast.point.y) - 0.5f;
            }
            else
            {
                if (directionFacing.x < 0)
                {
                    cropPosX = Mathf.Ceil(cropRaycast.point.x) - 0.5f;
                    cropPosY = Mathf.Ceil(cropRaycast.point.y) - 0.5f;
                }
                else if (directionFacing.x > 0)
                {
                    cropPosX = Mathf.Ceil(cropRaycast.point.x) + 0.5f;
                    cropPosY = Mathf.Ceil(cropRaycast.point.y) - 0.5f;
                }
            }

            Vector2 cropPos = new Vector2(cropPosX, cropPosY);

            // Stop from placing a crop outside of a dirt tile
            // Above calculations aren't perfect - plenty of scope to improve, but this is a bandaid fix
            // Sometimes they try to plant a crop in a location that's not a dirt tile. This raycast checks the location set by above calculations to double check it's a dirt tile
            RaycastHit2D stillDirt = Physics2D.Raycast(cropPos, Vector2.zero, 0f, dirtTile);

            // Cycle through all the current crops that have been planted
            // And check their position against  where you are trying to plant a new crop
            GameObject[] allCrops = GameObject.FindGameObjectsWithTag("Crop");

            // Can't plant a crop if there's already one in the location attemtping to plant
            bool canPlant = true;

            for (int i = 0; i < allCrops.Length; i++)
            {
                if (Vector2.Distance(cropPos, allCrops[i].transform.position) == 0)
                {
                    canPlant = false;
                }
            }

            // Plant a crop if able to do so
            if (stillDirt.collider != null && canPlant)
                Instantiate(crop, cropPos, Quaternion.identity);
        }
    }
}
