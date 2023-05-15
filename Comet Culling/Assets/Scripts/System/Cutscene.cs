// Control movement between cutscene panels and into the next scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Intro Panel 3")
                StartCoroutine(FinishCutscene(2));
            else if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "End Panel 5")
                StartCoroutine(FinishCutscene(0));
            else
                StartCoroutine(ChangePanel());
        }
            
    }

    // Use a coroutine to start the fade out and then change the scene after a brief delay
    IEnumerator ChangePanel()
    {
        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.25f);

        // Set the trigger for the fade out animation
        animator.SetTrigger("Next");
    }

    IEnumerator FinishCutscene(int nextScene)
    {
        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.25f);

        // Move to the next scene
        SceneManager.LoadScene(nextScene);
    }
}
