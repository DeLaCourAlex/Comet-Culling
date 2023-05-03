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
    public GameObject npcTestSprite;
    //private Inventory inventory; 
    Animator animator;

    void Start()
    {
        animator = npcTestSprite.GetComponent<Animator>(); 

    }




    [YarnCommand("characterEmotion")] //If Dialogue.cs is now attached to an object
    //I can run stuff like characterEmotion DisplayEmotion("happy") in a yarn script; 
    public void DisplayEmotion(string emotion)
    {
        npcTestSprite.SetActive(true);

        switch (emotion)
        {
            case "happy":
                //Load happy emotion
                Debug.Log("HAPPY EMOTION");
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
                Debug.Log("NEUTRAL EMOTION");
                animator.SetTrigger("neutral");


                break;

        }
    }

    //public void AddSeeds()
    //{
    //    inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = 2 }); 
    //}
    // Start is called before the first frame update
   
    // Update is called once per frame
    void Update()
    {
        //DisplayEmotion("neutral");
    }
}
