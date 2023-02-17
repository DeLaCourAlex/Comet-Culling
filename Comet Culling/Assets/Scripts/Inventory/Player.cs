using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //makes our class a singleton 
    public static Player Instance;

    public int Health;
    public int Exp;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ExpText;

    private void Awake()
    {
        Instance= this;
    }




    public void IncreaseHealth(int value)
    {
        Health += value;
        HealthText.text += $"HP:{Health}";
    }
    
    
    public void IncreaseExp(int value)
    {
        Exp += value;
        ExpText.text += $"HP:{Exp}";
    }
}
