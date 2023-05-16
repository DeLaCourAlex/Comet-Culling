// Contain functionality to display the correct captains log
// Every time that one is read, the next day a new log is available
// If a log is not read, a new log is not available the next day

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainLogs : MonoBehaviour
{
    // An array of the various captains logs text blocks
    [SerializeField] GameObject[] captainsLog;
    [SerializeField] GameObject background;

    //Array of bools that'll determine whether captain log is available or not
    bool[] isLogAvailable;
    //Bool to check if player has read the log of each day
    public bool hasBeenRead; 
    // Bool to check if the player is currently reading a log
    public bool logOpen;
    //Number of available logs
    int availableLogs; 

    // Start is called before the first frame update
    void Start()
    {//Initialize values from the ones stored in data permanence
        if (DataPermanence.Instance != null)
        {
            availableLogs = DataPermanence.Instance.availableLogs; 
            isLogAvailable = DataPermanence.Instance.isLogAvailable;
            hasBeenRead = DataPermanence.Instance.hasBeenRead;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 7; ++i) //Go through the array of bools
        {
            if (TimeManager.Day == (i + 1) && hasBeenRead) //If the day number == the index + 1 && it has been read
            {
                isLogAvailable[availableLogs] = true; //Turn the counter index bool on
                                                      //This way if the day == 3 && the player has interacted with screen
                                                      //If player didn't interact on day 1 but did on day 2,
                                                      //The available log would be log no. 2, not log no. 3
                                                      //Prompting player to interact with screen everyday to unlock logs
            }

            else if (i != availableLogs && hasBeenRead)
                isLogAvailable[i] = false; //Turn the rest off

        }
        if (hasBeenRead) //If the log has been read
        {
            availableLogs++; //Add to the available logs counter

            hasBeenRead = false; //Immediately toggle hasBeenRead off so it doesn't update this counter more times
        }

        DisplayLogs();

        // Update the available logs in data permanence
        DataPermanence.Instance.availableLogs = availableLogs;
        DataPermanence.Instance.isLogAvailable = isLogAvailable;
        DataPermanence.Instance.hasBeenRead = hasBeenRead;
        background.SetActive(logOpen);
    }

    void DisplayLogs() 
    {
        for (int i = 0; i < 7; ++i) //Go through the array of bools
        {
            if (isLogAvailable[i] && logOpen)
            {
                // Activate the current available log
                captainsLog[i].SetActive(true);
            } 

            // Deactivate the rest of the logs
            else
                captainsLog[i].SetActive(false);
        }
    }
}
