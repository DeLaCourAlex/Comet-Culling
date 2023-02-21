using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    //Attributes
    public int spaceshipEnergy { set; get; } //The global spaceship energy. Contains set and get methods for easier access and manipulation. 
   
    const int MAX_ENERGY = 100;
    const int MAX_STAMINA = 100;
    public enum CROP_TYPE { A, B, C };//This enum would need to be public and belonging to the crop system attributes


    // Start is called before the first frame update
    void Start()
    {
        spaceshipEnergy = MAX_ENERGY;
    }

    //Recharging spaceship
    public void ChargeSpaceship(CROP_TYPE type) //We either pass a crop game object to it or its enum, not sure yet (ask the boys)
    {
        float energyYield = 0; //Placeholder temp

        switch (type)
        {
            case (CROP_TYPE.A):
                //Check how much energy type A yields (that's why I thought of passing a ref to a crop gameobject)
                //Round it (up, maybe)
                //So it'd be something like energyYield = crop.getEnergy(); 

                break;
            case (CROP_TYPE.B):
                break;
            case (CROP_TYPE.C):
                break;

        }
        
        spaceshipEnergy += Mathf.RoundToInt(energyYield); //Add the energy yield to the spaceship's energy

    }

    public void ChargePlayer(int botStamina) //Pass these variables into this function from the player's controls
    {
        //Placeholder logic: charging 100% of the robot's stamina takes 25% of the spaceship. Will be replaced for a more optimised value in the future if needed.
        int rechargingStamina = MAX_STAMINA - botStamina; //Value needed to recharge to 100% stamina
        int takenEnergy = ((rechargingStamina * 25) / MAX_STAMINA); //Percentage of energy that will be taken from the spaceship to fill 100% of robot stamina
        int addedStamina = rechargingStamina; //The actual stamina that will be added to the level

        if (spaceshipEnergy == 0)
        {
            takenEnergy = 0;
            addedStamina = 0;

        }
        else if (spaceshipEnergy < takenEnergy) //If the ship's energy level < energy that should be taken to recharge stamina 100%
        {
            takenEnergy = spaceshipEnergy; //Only take what's left of the ship's energy
            addedStamina = (spaceshipEnergy * MAX_STAMINA) / 25; //And only fill the robot's stamina with the proportionate value

        }

        spaceshipEnergy -= takenEnergy; //New spaceship energy = current level - taken energy
        botStamina += Mathf.Min(addedStamina, rechargingStamina); //New stamina = current stamina + taken value.
        //Only the minimum value between the two will be added to the final stamina. 

    }

    // Update is called once per frame
    void Update()
    {

    }
}
