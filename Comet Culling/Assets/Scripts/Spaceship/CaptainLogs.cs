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
     //for(int i = 0; i < 7; ++i) //Set whole array to false upon start
     //   {
     //       isLogAvailable[i] = false; 
     //   }
        //hasBeenRead = false;
        //availableLogs = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < 7; ++i) //Go through the array of bools
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

        background.SetActive(logOpen);
    }

    void DisplayLogs()
    {
        for (int i = 0; i < 7; ++i) //Go through the array of bools
        {
            if (isLogAvailable[i] && logOpen)
            {
                Debug.Log("This is day's " + TimeManager.Day + "'s log, corresponds to index " + i + ", is set to true");
                captainsLog[i].SetActive(true);
            } 

            else
                captainsLog[i].SetActive(false);
        }
    }
}
