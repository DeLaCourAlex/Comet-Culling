using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This is a placeholder class to add time behavior working before we organize all UI into its respective class
public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;
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

    private void OnEnable()
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

    private void UpdateTime()
    {
        if(timeText != null)
            timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}"; //The 00 is a mask here so the empty string space can be filled w/ a 0

    }
    private void UpdateDate()
    {

        if(dateText != null)
            dateText.text = $"Day {TimeManager.Day}";

        Debug.Log("time in data permanence: " + DataPermanence.Instance.day);
        Debug.Log("Time in time manager: " + TimeManager.Day);

    }
}
