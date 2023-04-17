// Functionality to transfer data between scenes
// Keeps track of things like stamina, crop levels, health, in game time/date etc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [HideInInspector] public bool playerTutorial;

    // The current tutorial stage if applicable
    [HideInInspector] public int tutorialNumber;

    // THe available tools, used to minimize tool use during tutorials
    [HideInInspector] public int availableTools;

    // Determine if the player is out of resources and, because of this, when their impending death is 
    [HideInInspector] public bool resourcesDepleted;
    [HideInInspector] public int deathDay;
    [HideInInspector] public int deathHour;
    [HideInInspector] public int deathMinute;

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

    // TIME/DAY VARIABLES
    [HideInInspector] public int day;
    [HideInInspector] public int hour;
    [HideInInspector] public int mins;

    // ADD VARIABLES TO SET ELSEWHERE HERE AS NEEDED
    // UI to display the spaceships energy
    //[Header("UI References")]
    //[SerializeField] UI energyUI;

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

        playerTutorial = true;

        //Start-off values
        day = 1;
        mins = 0;
        hour = 7;
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

        //energyUI.UpdateValue(spaceshipEnergy);
    }

    public void PlayerStartTutorial()
    {
        playerTutorial = true;
        tutorialNumber = 0;
        availableTools = 1;
    }

    public void PlayerStartNoTutorial()
    {
        playerTutorial = false;
        tutorialNumber = 9;
        availableTools = 5;

    }

    public void RestartGame()
    {
        Destroy(gameObject);
    }

    /*availableTools; tutorialNumber; playerTutorial; playerStamina; cropsHarvested; playerStartPosition;
        cropB; cropA; deathMinute;deathHour; deathDay; resourcesDepleted;*/
}
