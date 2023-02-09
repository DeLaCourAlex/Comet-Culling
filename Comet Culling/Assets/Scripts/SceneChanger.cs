// Change between scenes in the game
// And play a fade out/in transition

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Create an instance of the class to allow to call its functions statically
    public static SceneChanger Instance;

    // An animator to call the fade out when changing scenes
    [SerializeField] Animator animator;

    // Called when the object containing the script is initialized
    private void Awake()
    {
        // Initialize the instance of the class
        Instance = this;
    }

    // Change scene ie between spaceship, main map, dungeon etc
    // Parameters set which scene to change to and where the player starts that scene
    public void ChangeScene(string scene, Vector2 startingPosition)
    {
        // Set the position of the player in the new scene
        DataPermanence.Instance.playerStartPosition = startingPosition;

        // Start coroutine to play the fadeout and load the new scene
        StartCoroutine(ChangeLevel(scene));
    }

    // Use a coroutine to start the fade out and then change the scene after a brief delay
    IEnumerator ChangeLevel(string scene)
    {
        // Set the trigger for the fade out animation
        animator.SetTrigger("SceneEnd");

        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.5f);

        // Load the new scene
        SceneManager.LoadScene(scene);
    }
}
