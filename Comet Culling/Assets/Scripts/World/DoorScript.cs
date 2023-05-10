// Functionality for triggers attached to doors
// The destination scene and starting position within that scene are set in the inspector for each door within each scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // The scene to move the player to
    [SerializeField] string destinationScene;
    // Player starting position in the new scene
    [SerializeField] Vector2 destinationStartingPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is a player
        PlayerController player = collision.GetComponent<PlayerController>();

        // If colliding object is a player, move to the desired scene
        if (player != null)
        {
            // Play the door audio
            if (AudioManager.Instance != null)
                AudioManager.Instance.playDoor();

            player.canMove = false;
            SceneChanger.Instance.ChangeScene(destinationScene, destinationStartingPosition);
        }
    }
}