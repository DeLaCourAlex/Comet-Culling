using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    //To notice whether the minute or hour has changed in-game
    public static Action OnMinuteChanged;
    public static Action OnHourChanged; 
    //Depends on how often we want certain events to happen we can 
    //Check these actions to know
    
    //Getsetters to access the in-game time
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    //Have IRL time affect in-game time by a certain ratio
    //Example: 0.5 seconds irl = 1 minute in-game
    private float minuteToRealTime = 0.5f;
    private float timer; 

    void Start()
    {
        //Start-off values
        Minute = 0;
        Hour = 10;
        timer = minuteToRealTime; //V important: set timer equivalent
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <=0) //If it's = 0, it means our time has elapsed (from 0.5 to 0)
        { //So we need to increment the minutes
            Minute++;
            //Make sure to trigger time Actions in their relevant places.
            //Question mark is the null check if statement. If OnMinuteChange != NULL (something is listening to that event), invoke it. 
            OnMinuteChanged?.Invoke();
            //Increment hours if 60 minutes + reset minutes
            if(Minute >= 60)
            {
                Hour++;
                Minute = 0;
                OnHourChanged?.Invoke(); 
            }
            timer = minuteToRealTime; //Reset timer
        }

    }
}
