using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaminaRecharge : MonoBehaviour
{
    //float shipEnergy = 100; //Placeholder variable - this will need to be a variable that accesses the actual spaceship's energy lvl
    const int MAX_ENERGY = 100;
    const int MAX_STAMINA = 100; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float Recharge(float shipEnergy, float botStamina) //Pass these variables into this function from the player's controls
    {
        //Placeholder logic: charging 100% of the robot's stamina takes 25% of the spaceship. Will be replaced for a more optimised value in the future if needed.
        float rechargingStamina = MAX_STAMINA - botStamina; //Value needed to recharge to 100% stamina
        float takenEnergy = ((rechargingStamina * 25) / MAX_STAMINA); //Percentage of energy that will be taken from the spaceship to fill 100% of robot stamina
        
        if(shipEnergy < takenEnergy) //If the ship's energy level < energy that should be taken to recharge stamina 100%
        {
            takenEnergy = shipEnergy; //Only take what's left of the ship's energy
            rechargingStamina = (shipEnergy * MAX_STAMINA) / 25; //And only fill the robot's stamina with the proportionate value

        }

        shipEnergy -= takenEnergy; //New spaceship energy = current level - taken energy
        botStamina += min(rechargingStamina, 100); //New stamina = current stamina + taken value

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
