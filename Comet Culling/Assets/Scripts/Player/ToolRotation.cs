// Flip the tool icon to make sure it doesn't change with the player's rotation
// Probably not the best way to do this but a quick fix that worked so we kept it in

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolRotation : MonoBehaviour
{
    // Store a reference to the player
    [SerializeField] GameObject player;

    // Make sure the tool object faces to the right
    // If the player faces left, flip it to the right 180 degrees
    // otherwise face it in the same direction as the player
    float rotationY => player.transform.rotation.y > 0 ? -180 : 0;
    Quaternion rotation => Quaternion.Euler(0, rotationY, 0);

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = rotation;
    }
}
