// Contain functionality to set stamina and energy bars

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    // a reference to the stamina and energy animator components
    Animator animatorStamina;
    Animator animatorEnergy;

    // References to the stamina and energy bar
    [SerializeField] GameObject staminaBar;
    [SerializeField] GameObject energyBar;

    private void Start()
    {
        // Initialize components
        animatorStamina = staminaBar.GetComponent<Animator>();
        animatorEnergy = energyBar.GetComponent<Animator>();
    }
 
    private void Update()
    {
        // Set the stamina bar and energy bar
        // Dividing the value by 10 gives a values out of 0-10 instead of 0-100
        // This works better with the animator blend tree to move between states

        if (animatorStamina != null)
        {
            animatorStamina.SetFloat("Value", DataPermanence.Instance.playerStamina / 10);
        }

        if (animatorEnergy != null)
        {
            animatorEnergy.SetFloat("Value", DataPermanence.Instance.spaceshipEnergy / 10);
        }

    }
}
