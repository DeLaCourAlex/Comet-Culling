using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Used to control and display the various tutorial text boxes
    [SerializeField] GameObject[] tutorialText;
    [SerializeField] GameObject[] tutorialHighlights;
    int textIndex;

    // Store a reference to the player controller to know when to move between tutorials
    [SerializeField] GameObject player;
    PlayerController playerController;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Use the player controller to access which tutorial we're currently in
        // And activate the corresponding text box
        for (int i = 0; i < tutorialText.Length; i++)
        {
            if (i == playerController.tutorialNumber)
            {
                tutorialText[i].SetActive(true);
                tutorialHighlights[i].SetActive(true);
            }
                
            else
            {
                tutorialText[i].SetActive(false);
                tutorialHighlights[i].SetActive(false);
            }
        }
    }
}
