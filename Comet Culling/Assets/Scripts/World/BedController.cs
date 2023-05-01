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
        // Set the trigger for the fade out animation
        fadeoutAnimator.SetTrigger("Fade Out");

        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.5f);

        // Set the bed to sleeping, deactivate the player object
        animator.SetTrigger("Sleeping");
        player.SetActive(false);

        // Set the trigger for the fade in animation
        fadeoutAnimator.SetTrigger("Fade In");

        // Pass two seconds of sleeping
        yield return new WaitForSeconds(4f);

        // Set the trigger for the fade out animation
        fadeoutAnimator.SetTrigger("Fade Out");

        // Pause the function to play the fade out animation
        yield return new WaitForSeconds(0.5f);

        // Set the trigger for the fade in animation
        fadeoutAnimator.SetTrigger("Fade In");

        // Set the bed to awake, reactivate the player object
        animator.SetTrigger("Awake");
        player.SetActive(true);
    }
}
