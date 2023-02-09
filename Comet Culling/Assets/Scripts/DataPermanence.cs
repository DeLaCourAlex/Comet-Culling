using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPermanence : MonoBehaviour
{
    // Create an instance of the class to store all relevant player data
    public static DataPermanence Instance;

    public int testVariablePlayer;

    // Called when the script object is initialized
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
