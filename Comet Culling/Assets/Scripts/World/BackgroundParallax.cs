using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    // A reference to the scene camera
    [SerializeField] Camera cam;
    // The player's transform
    [SerializeField] Transform player;

    // The starting position for the background object
    Vector2 startPos;
    // Adds a z position for objects further back - slows down their parallax movement the further away they are
    float startPosZ;

    // Travel is the distance that the camera has moved during the game
    Vector2 travelDistance => (Vector2)cam.transform.position - startPos;
    // How far back the part of the background is from the player on the z axis
    float distanceFromPlayer => transform.position.z - player.position.z;

    // Decide if we're using the camera's near of far clippping plane
    // Depending on whether the object in question is closer or further along the z axis than the player
    // This is to stop objects from moving in the wrong direction if they're closer than the player
    float clippingPlane => (cam.transform.position.z + (distanceFromPlayer > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // The factor by which we want to moved the sprite
    float parallaxFactor => Mathf.Abs(distanceFromPlayer) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startPosZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the background a varying distance depending on its parallax factor
        Vector2 newPos = startPos + travelDistance * parallaxFactor;
        transform.position = new Vector3(newPos.x, startPos.y, startPosZ);
    }
}
