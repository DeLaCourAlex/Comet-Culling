using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    Animator animator;

    // The attribute to be displayed ie stamina, energy, health etc.
    int attributeValue;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateValue(int value)
    {
        if(animator != null)
            animator.SetFloat("Value", value);
    }
    // Update is called once per frame
/*    void Update()
    {
        // Set the attribute in the animator to display how full the UI bar is
        animator.SetFloat("Value", attributeValue);
        Debug.Log("Value: " + attributeValue);
    }*/
}
