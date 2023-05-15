// Control movement between cutscene panels and into the next scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    // A reference to the cutscenes animator component
    Animator animator;

    private void Start()
    {
        // Initialize components
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        // When "action" key is pressed, go to the next panel
        // Or change scene if on the last panel of each cutscene
        if (Input.GetButtonDown("Action"))
        {
            // Move from intro cutscene to main game at the end of intro panel
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Intro Panel 3")
                StartCoroutine(FinishCutscene(2));
            // Move from end cutscene back to the main menu at the end of the end cutscene
            else if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "End Panel 5")
            {
                DataPermanence.Instance.RestartGame();
                StartCoroutine(FinishCutscene(0));
            }
                
            // Move to the next panel of the cutscene
            else
                StartCoroutine(ChangePanel());
        }
            
    }


    // Use a coroutine to move to the next panel after a very brief pause
    IEnumerator ChangePanel()
    {
        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.25f);

        // Set the trigger for the fade out animation
        animator.SetTrigger("Next");
    }

    // Move to another scene at the end of a cutscene
    IEnumerator FinishCutscene(int nextScene)
    {
        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.25f);

        // Move to the next scene
        SceneManager.LoadScene(nextScene);
    }
}
