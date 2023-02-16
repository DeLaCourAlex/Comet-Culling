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

    // The time since the crop was planted
    // Used to determine if it can be harvested or not
    public float timeAlive { get; set; } = 0;
    
    // If the crop is watered
    // If so, if grows faster
    public bool isWatered { get; set; }

    public bool isGrown { get; private set; }

    // The age at which  a crop is grown
    float grownAge = 15;
    public float wateredMultiplier { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Multiplier to increase the speed that a crop grows if watered
        wateredMultiplier = isWatered ? 2f : 1f;

        // Increase time alive
        timeAlive += Time.deltaTime * wateredMultiplier;

        if (timeAlive >= grownAge)
            isGrown = true;

        // Animator parameters

        // Set the age of the crop
        animator.SetFloat("Age", timeAlive);
        // Set if the crop has been watered
        animator.SetBool("Watered", isWatered);

        // Display time alive for debugging purposes
        //Debug.Log("Crop time alive: " + timeAlive);
    }
}
