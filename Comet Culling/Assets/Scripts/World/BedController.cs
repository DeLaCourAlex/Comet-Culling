// Bed controller - mostly used to play the sleep animation
// Actual changing of time and date when sleeping is done in the player controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : MonoBehaviour
{
    // Two animators - one for the bed, one for the fade in and fade out
    [SerializeField] Animator fadeoutAnimator;
    Animator animator;

    // A reference to the player to enable/disable them when sleeping
    [SerializeField] GameObject player;

    // A reference to the time display to enable/disable when sleeping
    [SerializeField] GameObject timeDisplay;
    [SerializeField] GameObject dayDisplay;

    // a reference to the tutorial displays
    [SerializeField] GameObject tutorial;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void BedAnimation()
    {
        StartCoroutine(Sleep());
    }

    // Use a coroutine to start the fade out and then change the scene after a brief delay
    IEnumerator Sleep()
    {
        dayDisplay.SetActive(false);
        if (tutorial != null)
            tutorial.SetActive(false);

        // Set the trigger for the fade out animation
        fadeoutAnimator.SetTrigger("Fade Out");

        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.25f);

        // Set the bed to sleeping, deactivate the player object
        animator.SetTrigger("Sleeping");
        player.SetActive(false);
        timeDisplay.SetActive(false);

        // Set the trigger for the fade in animation
        fadeoutAnimator.SetTrigger("Fade In");

        // Pass two seconds of sleeping
        yield return new WaitForSeconds(2f);

        // Set the trigger for the fade out animation
        fadeoutAnimator.SetTrigger("Fade Out");

        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.25f);

        // Set the trigger for the fade in animation
        fadeoutAnimator.SetTrigger("Fade In");

        // Set the bed to awake, reactivate the player object
        animator.SetTrigger("Awake");
        player.SetActive(true);
        timeDisplay.SetActive(true);
        dayDisplay.SetActive(true);
        if (tutorial != null)
            tutorial.SetActive(true);
    }
}
