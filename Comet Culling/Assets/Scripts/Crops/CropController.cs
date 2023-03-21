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

    // The age at which a crop is grown
    [SerializeField] float timeToGrow; 
    // The age at which a crop is grown when watered
    [SerializeField] float timeToGrowWatered; 
    [SerializeField] float energyGiven;

    // Used to determine if the crop type from other scripts
    [field: SerializeField] public int elementNumber { get; set; }

    // The time since the crop was planted
    // Used to determine if it can be harvested or not
    public float timeAlive { get; set; } = 0;
    
    // If the crop is watered
    // If so, if grows faster
    public bool isWatered { get; set; }

    public bool isGrown { get; private set; }
    public bool isWithered { get; private set; }
    public float wateredMultiplier { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        isWithered = false; 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Multiplier to increase the speed that a crop grows if watered
        // This is calculated based on the quotient of the two times to grown - watered and not
        // This line says that if the crop is not watered, its multiplier is 1 - it grows in normal time
        // If it is watered, its multiplier is the larger number (time to grow unwatered) divided by the smaller number (time to grow watered)
        // This causes the time alive to increase faster when it's watered
        wateredMultiplier = isWatered ? timeToGrow / timeToGrowWatered : 1f;

        // Increase time alive
        timeAlive += Time.deltaTime * wateredMultiplier;

        if (timeAlive >= timeToGrow)
            isGrown = true;

        if (isGrown && timeAlive > TimeManager.Day)
            //If the crop has been mature + alive for more than one day, wither
            //Will add another && here so it can check the condition if the player has gone to sleep 
            isWithered = true; 
        else
            isWithered = false;
        // Animator parameters

        // Set the age of the crop
        animator.SetBool("Is Grown", isGrown);
        // Set if the crop has been watered
        animator.SetBool("Watered", isWatered);
        // Set if the crop is withered
        animator.SetBool("Withered", isWithered);
        // Set the crop type depending on its element number
        animator.SetFloat("Element", elementNumber);
    }
}
