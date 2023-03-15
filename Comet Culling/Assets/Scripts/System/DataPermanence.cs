// Functionality to transfer data between scenes
// Keeps track of things like stamina, crop levels, health, in game time/date etc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataPermanence : MonoBehaviour
{
    // Create an instance of the class to allow to call its functions statically
    public static DataPermanence Instance;

    // ALL VARIABLES TO SET IN THE PLAYER CONTROLLER

    // Set the player position when entering a new scene
    public Vector2 playerStartPosition;

    // Set the test variable for crops in player inventory
    public int testCropsHarvested;

    // ALL VARIABLES FOR CROPS AND CROP MANAGEMENT

    // Store the position of each crop and its time alive in a list
    public class CropData
    {
        public Vector2 position;
        public float timeAlive;
        public float wateredMultiplier;
        public bool isWatered;
<<<<<<< Updated upstream
=======
        
        public int cropType;
>>>>>>> Stashed changes

        // Constructor for when adding items to the crop list
        public CropData(Vector2 pos, float time, float wMultiplier, bool watered)
        {
            position = pos;
            timeAlive = time;
            wateredMultiplier = wMultiplier;
            isWatered = watered;
<<<<<<< Updated upstream
=======
            cropType = type;
          
>>>>>>> Stashed changes
        }
    }

    // A list of all crops in the farm scene
    public List<CropData> allCrops = new List<CropData>();

    // A reference to the tilemap in the farm scene
    public List<Vector3Int> tilledTilePositions = new List<Vector3Int>();

    // ADD VARIABLES TO SET ELSEWHERE HERE AS NEEDED


    // Called when the object containing the script is initialized
    private void Awake()
    {
        // Ensures that only one instance of the class is created
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the instance
        Instance = this;

        // Keeps the instance alive moving between scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // The list of crops is deleted once they're instantiated back into the scene and able to update themselves,
        // so the list will only have any elements if the player is in a non-crop containing scene
        // If the list is empty, nothing needs to be done. If it has any elements, update their time alive to use when returning to the crop containing scene
        if (allCrops.Count != 0)
            for(int i = 0; i < allCrops.Count; i++)
            {
                allCrops[i].timeAlive += Time.deltaTime * allCrops[i].wateredMultiplier;
            }
    }
}
