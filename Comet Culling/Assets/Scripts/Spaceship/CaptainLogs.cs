using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainLogs : MonoBehaviour
{
    [SerializeField] GameObject[] captainsLog;
    [SerializeField] GameObject background;
    bool[] isLogAvailable = new bool[7]; //Array of bools that'll determine whether captain log is available or not
    public bool hasBeenRead; //Bool to check if player has read the log of each day
    public bool logOpen;
    int availableLogs; 
    // Start is called before the first frame update
    void Start()
    {
        if (DataPermanence.Instance != null)
        {
            availableLogs = DataPermanence.Instance.availableLogs;
            isLogAvailable = DataPermanence.Instance.isLogAvailable;
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
            Debug.Log("The captain log has been read today"); 
            availableLogs++; //Add to the available logs counter
            Debug.Log("Available log counter:" + availableLogs);

            hasBeenRead = false; //Immediately toggle hasBeenRead off so it doesn't update this counter more times
        }
        DisplayLogs();
        DataPermanence.Instance.availableLogs = availableLogs;
        DataPermanence.Instance.isLogAvailable = isLogAvailable;
        background.SetActive(logOpen);
    }

    void DisplayLogs()
    {
        for (int i = 0; i < 7; ++i) //Go through the array of bools
        {
            if (isLogAvailable[i] && logOpen)
            {
                if (i == 6 && DataPermanence.Instance.highAffinity)
                    i++;

                captainsLog[i].SetActive(true);
            } 

            else
                captainsLog[i].SetActive(false);
        }
    }
}
