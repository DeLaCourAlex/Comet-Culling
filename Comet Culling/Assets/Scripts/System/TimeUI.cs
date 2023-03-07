using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This is a placeholder class to add time behavior working before we organize all UI into its respective class
public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private void OnEnable()
    {
        TimeManager.OnMinuteChanged += UpdateTime;
        TimeManager.OnHourChanged += UpdateTime; 
    }

    private void OnDisable() //For if we need to disable the time UI
    {
        TimeManager.OnMinuteChanged -= UpdateTime;
        TimeManager.OnHourChanged -= UpdateTime;
    }

    private void UpdateTime()
    {
        timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}"; //The 00 is a mask here so the empty string space can be filled w/ a 0
    }
}