// Contains basic functionality for crops to grow over time
// Right now just a basic timer
// Will add stages of growth, times to reach each stage both watered and not, etc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    // MEMBER OBJECTS AND COMPONENTS
    Animator animator;
    [SerializeField] int growthRate; //This needs to be serialized in order to cookie cutter it
    [SerializeField] float energyYield;
    [SerializeField] int growthRateWatered;
    [SerializeField] int growthRateDry;
  
    public float timeAlive { get; set; } = 0;

    // If the crop is watered
    // If so, if grows faster
    public bool isWatered { get; set; }
    // The time since the crop was planted
    // Used to determine if it can be harvested or not
    public bool isGrown { get; private set; }
    public bool isWilted { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isWilted = false;
        animator = GetComponent<Animator>();
        growthRate = growthRateDry;
    }

    // Update is called once per frame
    void Update()
    {
        // Multiplier to increase the speed that a crop grows if watered
        if (isWatered)
        {
            growthRate = growthRateWatered;
        }
        else
        {
            growthRate = growthRateDry;
        }

        // Increase time alive
        timeAlive += Time.deltaTime;

        if (timeAlive >= growthRate)
            isGrown = true;

        //Setting wilted to true or false depending on its status
        if(isGrown && timeAlive >= (timeToGrow*0.5)) //If the crop has grown and it has been alive for more than half the time it takes to grow
            isWilted = true;
        else
            isWilted = false;




        // Animator parameters

        // Set the age of the crop
<<<<<<< Updated upstream
        animator.SetFloat("Age", timeAlive);
=======
        animator.SetBool("Is Grown", isGrown);
        //Set crop wilted status
        animator.SetBool("Is Wilted", isWilted);

>>>>>>> Stashed changes
        // Set if the crop has been watered
        animator.SetBool("Watered", isWatered);

        // Display time alive for debugging purposes
       // Debug.Log("Crop time alive: " + timeAlive);
    }
}
