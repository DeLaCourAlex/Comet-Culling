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
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Final Panel")
                StartCoroutine(FinishCutscene());
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

    IEnumerator FinishCutscene()
    {
        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.25f);

        // Move to the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FinishCutsceneEvent()
    {
        StartCoroutine(FinishCutscene());
    }
}
