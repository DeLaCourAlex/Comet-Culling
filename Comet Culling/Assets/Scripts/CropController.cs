// Contains basic functionality for crops to grow over time
// Right now just a basic timer
// Will add stages of growth, times to reach each stage both watered and not, etc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public float timeAlive { get; set; } = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Increase time alive
        timeAlive += Time.deltaTime;

        // Display time alive for debugging purposes
        Debug.Log("Crop time alive: " + timeAlive);
    }
}
