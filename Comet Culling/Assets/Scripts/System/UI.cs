using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    Animator animatorStamina;
    Animator animatorEnergy;
    [SerializeField] GameObject staminaBar;
    [SerializeField] GameObject energyBar;

    // The attribute to be displayed ie stamina, energy, health etc.
    /*public int attributeValue;*/

    private void Start()
    {
        animatorStamina = staminaBar.GetComponent<Animator>();
        animatorEnergy = energyBar.GetComponent<Animator>();
    }

    private void Update()
    {
        if (animatorStamina != null)
        {
            animatorStamina.SetFloat("Value", DataPermanence.Instance.playerStamina / 10);
/*            Debug.Log("UPDATING STAMINA UI");*/
            //Debug.Log("Stamina: " + DataPermanence.Instance.playerStamina / 10);
        }
            

        if (animatorEnergy != null)
        {
            animatorEnergy.SetFloat("Value", DataPermanence.Instance.spaceshipEnergy / 10);
            Debug.Log("UPDATING ENERGY UI");
            Debug.Log("Energy: " + DataPermanence.Instance.spaceshipEnergy);
        }

    }
    // Update is called once per frame
/*    void Update()
    {
        // Set the attribute in the animator to display how full the UI bar is
        animator.SetFloat("Value", attributeValue);
        Debug.Log("Value: " + attributeValue);
    }*/
}
