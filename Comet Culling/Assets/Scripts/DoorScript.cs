using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // The scene to move the player to
    [SerializeField] string destinationScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is a player
        PlayerController player = collision.GetComponent<PlayerController>();

        // If colliding object is a player, move to the desired scene
        if (player != null)
            player.ChangeScene(destinationScene);
    }
}