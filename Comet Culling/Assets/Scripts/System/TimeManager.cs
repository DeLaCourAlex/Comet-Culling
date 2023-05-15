using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    //To notice whether the minute or hour has changed in-game
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    public static Action OnDayChanged;
    //Depends on how often we want certain events to happen we can 
    //Check these actions to know

    //Getsetters to access the in-game time
    public static int Minute { get; set; }
    public static int Hour { get; set; }
    public static int Day { get; set; }

    //Have IRL time affect in-game time by a certain ratio
    //Example: 0.5 seconds irl = 1 minute in-game

    private float minuteToRealTime = 0.5f;
    private float timer;

    private static int MAX_DAYS = 7;


    void Start()
    {
        //Start-off values
        Day = 1;
        Minute = 0;
        Hour = 7;

        timer = minuteToRealTime; //V important: set timer equivalent
        
        if(DataPermanence.Instance != null) 
        {
            Day = DataPermanence.Instance.day;
            Hour = DataPermanence.Instance.hour;
            Minute = DataPermanence.Instance.mins;

        }

        OnDayChanged?.Invoke();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (DataPermanence.Instance != null)
        {
            DataPermanence.Instance.day = Day;
            DataPermanence.Instance.hour = Hour;
            DataPermanence.Instance.mins = Minute;
            //Debug.Log("Data permanence time: " + DataPermanence.Instance.hour + ":" + DataPermanence.Instance.mins);
            Debug.Log("Data permanence day: " + DataPermanence.Instance.day);
        }

        if (timer <= 0) //If it's = 0, it means our time has elapsed (from 0.5 to 0)
        { //So we need to increment the minutes
            Minute++;
            //Make sure to trigger time Actions in their relevant places.
            //Question mark is the null check if statement. If OnMinuteChange != NULL (something is listening to that event), invoke it. 
            OnMinuteChanged?.Invoke();
            //Increment hours if 60 minutes + reset minutes
            if (Minute >= 60)
            {
                Hour++;
                Minute = 0;
                OnHourChanged?.Invoke();
                if (Hour > 23)
                {
                    Day++;
                    Hour = 0;
                    Minute = 0;
                    OnDayChanged?.Invoke();
                }
            }


            if (Day > MAX_DAYS)
            {
                Debug.Log("Reached final day, trigger endscene");
                SceneChanger.Instance.ChangeScene("End Cutscene", Vector2.zero);
            }

            timer = minuteToRealTime; //Reset timer
        }

    }

}
