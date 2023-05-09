using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    // Used to control and display the various tutorial text boxes
    [SerializeField] GameObject[] tutorialText;
    [SerializeField] GameObject[] tutorialHighlights;
    [SerializeField] GameObject textBox;

    // Store a reference to the door of whatever scene the player is currently in
    // This can be activated and deactivated to stop the player moving between scenes
    // depending on the stage of the tutorial
    BoxCollider2D doorCollider;
    //BoxCollider2D screenCollider;
    [SerializeField] GameObject door;
    //[SerializeField] GameObject screen;

    // Store a reference to the player controller to know when to move between tutorials
    [SerializeField] GameObject player;
    PlayerController playerController;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        doorCollider = door.GetComponent<BoxCollider2D>();
        //screenCollider = screen.GetComponent<BoxCollider2D>();
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
        if (currentScene == "AlexTestScene" && (playerController.tutorialNumber == 3 || playerController.tutorialNumber == 8) ||
            currentScene == "AlexTestScene SpaceShip" && playerController.tutorialNumber == 6)
            doorCollider.enabled = true;
        else
            doorCollider.enabled = false;

        /*if(screen != null)
        {
            if (playerController.tutorialNumber == 5)
                screenCollider.enabled = false;
            else
                screenCollider.enabled = true;
        }*/

        if (playerController.tutorialNumber == 7 && playerController.npc_detection)
        {
            tutorialText[7].SetActive(false);
            textBox.SetActive(false);
        }
        else
        {
            textBox.SetActive(true);
        }
        Debug.Log("In TUtorial, number " + playerController.tutorialNumber);

        if (playerController.tutorialNumber == 11)
        {
            Debug.Log("Ending tutorial");
            doorCollider.enabled = true;
            textBox.SetActive(false);
            enabled = false;
        }
    }
}
