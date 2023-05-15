using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Yarn;
using Yarn.Unity;

public class DialogueUI : MonoBehaviour
{
    //public Animation princeNeutral; 
    public GameObject npcTalksprite;
    public GameObject princeTalksprite; 
    //private Inventory inventory; 
    Animator npcanimator, princeanimator;
  /*  [SerializeField]*/ PlayerController playerController;
    public bool isTalking; 

    void Start()
    {
        playerController = GetComponent<PlayerController>(); 
        npcanimator = npcTalksprite.GetComponent<Animator>();
        princeanimator = princeTalksprite.GetComponent<Animator>(); 
    }

    [YarnCommand("characterEmotion")] //If Dialogue.cs is now attached to an object
    //I can run stuff like characterEmotion characterEmotion("happy") in a yarn script; 
    public void DisplayEmotion(string emotion, Animator animator)
    {
        //npcTestSprite.SetActive(true);

        switch (emotion)
        {
            case "happy":
                //Load happy emotion
                //Debug.Log("HAPPY EMOTION");
                animator.SetTrigger("happy");

                break;
            case "sad":
                //Load happy emotion
                animator.SetTrigger("sad");

                break;
            case "angry":
                //Load happy emotion
                animator.SetTrigger("angry");

                break;
            default:
                //Load neutral emotion
                //Debug.Log("NEUTRAL EMOTION");
                animator.SetTrigger("neutral");


                break;

        }
    }

    [YarnCommand("toggleTalksprite")]
    public void ToggleTalksprite(bool prince, bool npc)
    {
        princeTalksprite.SetActive(prince);
        npcTalksprite.SetActive(npc);
    }
  
    [YarnCommand("enableMovement")]
    public void EnableMovement(bool m)
    {
        playerController.canMove = m;   
    }
    public void HideTalksprites()
    {
        ToggleTalksprite(false, false); 
    }
   
    // Update is called once per frame
    void Update()
    {
        isTalking = princeTalksprite.activeSelf; 
        //playerController.canMove = !(princeTalksprite.activeSelf && npcTalksprite.activeSelf); 
    }
}
