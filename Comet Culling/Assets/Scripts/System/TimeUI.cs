using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Class to monitor time behavior via UI
public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText; //Text that'll display the hours and minutes
    public TextMeshProUGUI dateText; //Text that'll display the days

    //Update both time and date at the start and at each frame
    private void Start()
    {
        UpdateTime();
        UpdateDate();
    }

    public void Update()
    {
        UpdateTime();
        UpdateDate();
    }

    private void OnEnable() //Update time and date depending on the values updated in the TimeManager class
    {
        TimeManager.OnMinuteChanged += UpdateTime;
        TimeManager.OnHourChanged += UpdateTime;
        TimeManager.OnDayChanged += UpdateDate;
    }

    private void OnDisable() //For if we need to disable the time UI
    {
        TimeManager.OnMinuteChanged -= UpdateTime;
        TimeManager.OnHourChanged -= UpdateTime;
        TimeManager.OnDayChanged -= UpdateDate;

    }

    private void UpdateTime() //Updates the minutes and hours' text according to the Time Manager's values
    {
        if(timeText != null)
            timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}"; //The 00 is a mask here so the empty string space can be filled w/ a 0
    }
    private void UpdateDate() //Likewise but for the date
    {
        if(dateText != null)
            dateText.text = $"Day {TimeManager.Day}";
    }
}
