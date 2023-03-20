using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    // Used to control and display the various tutorial text boxes
    [SerializeField] GameObject[] tutorialText;
    [SerializeField] GameObject[] tutorialHighlights;

    // Store a reference to the door of whatever scene the player is currently in
    // This can be activated and deactivated to stop the player moving between scenes
    // depending on the stage of the tutorial
    BoxCollider2D doorCollider;
    [SerializeField] GameObject door;

    // Store a reference to the player controller to know when to move between tutorials
    [SerializeField] GameObject player;
    PlayerController playerController;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        doorCollider = door.GetComponent<BoxCollider2D>();
        //doorCollider.enabled = false;
    }

    private void Update()
    {
        // Store the name of the current scene - certain things can only be done in the spaceship
        // or in the crop scene
        string currentScene = SceneManager.GetActiveScene().name;

        // Use the player controller to access which tutorial we're currently in
        // And activate the corresponding text box
        for (int i = 0; i < tutorialText.Length; i++)
        {
            if (i == playerController.tutorialNumber)
            {
                tutorialText[i].SetActive(true);
                // Not all tutorial stages have tile highlights
                // This stops the highlights array going out of bounds
                if (i < tutorialHighlights.Length)
                    tutorialHighlights[i].SetActive(true);
            }
                
            else
            {
                tutorialText[i].SetActive(false);
                // Not all tutorial stages have tile highlights
                // This stops the highlights array going out of bounds
                if (i < tutorialHighlights.Length)
                    tutorialHighlights[i].SetActive(false);
            }
        }

        // Determine whether the door collider is enabled or not
        // And therefor if the player can leave the current scene
        if (currentScene == "AlexTestScene" && (playerController.tutorialNumber == 3 || playerController.tutorialNumber == 6) ||
            currentScene == "AlexTestScene SpaceShip" & playerController.tutorialNumber == 5)
            doorCollider.enabled = true;
        else
            doorCollider.enabled = false;

        if(playerController.tutorialNumber == 9)
        {
            doorCollider.enabled = true;
            enabled = false;
        }
    }
}
