// Functionality to transfer data between scenes
// Keeps track of things like stamina, crop levels, health, in game time/date etc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPermanence : MonoBehaviour
{
    // Create an instance of the class to allow to call its functions statically
    public static DataPermanence Instance;

    // ALL VARIABLES TO SET IN THE PLAYER CONTROLLER

    // A test variable to make sure data permanence works
    public int testVariablePlayer;

    // Set the player position when entering a new scene
    public Vector2 playerStartPosition;

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
}
