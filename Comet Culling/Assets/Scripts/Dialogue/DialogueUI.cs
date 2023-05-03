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

    void Start()
    {
        
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

    [YarnCommand("toggleTalksprite")]
    public void ToggleTalksprite(bool prince, bool npc)
    {
        princeTalksprite.SetActive(prince);
        npcTalksprite.SetActive(npc);
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
