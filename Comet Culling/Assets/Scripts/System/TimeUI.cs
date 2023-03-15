using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This is a placeholder class to add time behavior working before we organize all UI into its respective class
public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
<<<<<<< Updated upstream

=======
    public TextMeshProUGUI dateText;
    private void Start()
    {
        
        UpdateTime();
        UpdateDate();

    }
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
        //dateText.text = $"{TimeManager.Day}";
    }
    private void UpdateDate()
    {
        dateText.text = $"Day {TimeManager.Day}";
               //dateText.text = $"Day one";

>>>>>>> Stashed changes
    }
}
