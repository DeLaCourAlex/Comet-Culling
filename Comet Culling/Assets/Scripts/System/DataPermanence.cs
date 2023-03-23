// Functionality to transfer data between scenes
// Keeps track of things like stamina, crop levels, health, in game time/date etc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.Tilemaps;

public class DataPermanence : MonoBehaviour
{
    // Create an instance of the class to allow to call its functions statically
    [HideInInspector] public static DataPermanence Instance;

    // ALL VARIABLES TO SET IN THE PLAYER CONTROLLER

    // Set the player position when entering a new scene
    [HideInInspector] public Vector2 playerStartPosition;

    // The amount of the different crop types held in inventory
    [HideInInspector] public int[] cropsHarvested;

    // Player stamina
    [HideInInspector] public int playerStamina;

    // The current tutorial stage if applicable
    [HideInInspector] public int tutorialNumber;

    // THe available tools, used to minimize tool use during tutorials
    [HideInInspector] public int availableTools;

    // ALL VARIABLES FOR CROPS AND CROP MANAGEMENT
    [HideInInspector] public int cropA;
    [HideInInspector] public int cropB;





    // Store the position of each crop and its time alive in a list
    public class CropData
    {
        public Vector2 position;
        public float timeAlive;
        public float wateredMultiplier;
        public bool isWatered;
        public int cropType;
       
        // Constructor for when adding items to the crop list
        public CropData(Vector2 pos, float time, float wMultiplier, bool watered, int type)
        {
            position = pos;
            timeAlive = time;
            wateredMultiplier = wMultiplier;
            isWatered = watered;
            cropType = type;
        }
    }

    // A list of all crops in the farm scene
    [HideInInspector] public List<CropData> allCrops = new List<CropData>();

    // A reference to the tilemap in the farm scene
    [HideInInspector] public List<Vector3Int> tilledTilePositions = new List<Vector3Int>();

    // SPACESHIP VARIABLES

    [HideInInspector] public int spaceshipEnergy;

    // ADD VARIABLES TO SET ELSEWHERE HERE AS NEEDED
    // UI to display the spaceships energy
    //[Header("UI References")]
    //[SerializeField] TextMeshProUGUI energyUI;

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

        cropsHarvested = new int[2];
        playerStamina = 100;
        spaceshipEnergy = 0;
        availableTools = 1;
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

        // Update the UI with the current spaceship energy

        energyUI.UpdateValue(spaceshipEnergy);
        //Debug.Log("Spaceship energy in data manager: " + spaceshipEnergy);
        //Debug.Log("Spaceship energy in UI script: " + energyUI.attributeValue);


        energyUI.text = "Spaceship energy = " + spaceshipEnergy.ToString(); 


    }
}
