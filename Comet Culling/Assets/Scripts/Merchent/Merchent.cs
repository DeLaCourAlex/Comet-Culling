using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class Merchent : MonoBehaviour
{
    
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Shop;
    [SerializeField] GameObject Dialogue;
    private DialogueRunner dialogueRunner;

    private void Start()
    {
        //Look for a dialogue runner object in the scene to initialize the member variable to
        dialogueRunner = GameObject.FindObjectOfType<Yarn.Unity.DialogueRunner>();

    }

    //Methods for each of the yarnspinner nodes
    private void Day2Conversation()
    {
        dialogueRunner.StartDialogue("Day_2");
    }
    private void Day4Conversation()
    {
        dialogueRunner.StartDialogue("Day_4");
    }
    private void Day6Conversation()
    {
        dialogueRunner.StartDialogue("Day_6");
    }

    //player detection
    bool player_detection = false;

    // Update is called once per frame
    void Update()
    {
        DayCycle();

        //if player is detected 
        if (player_detection)
        {
            
            Panel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                Shop.SetActive(true);

            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                Dialogue.SetActive(true);

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Shop.SetActive(false);
                Dialogue.SetActive(false);

            }




        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "Player")
        if (collision.CompareTag("Player"))
        {
            player_detection = true;
            Debug.Log("collision triggred");

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        player_detection = false;
        Panel.SetActive(false);
    }


    private void DayCycle()
    {
        switch (TimeManager.Day)
        {
           
            case 2:
                Day2Conversation();
                break;
            
            case 4:
                Day4Conversation();
                break;
           
            case 6:
                Day6Conversation();
                break;
          

        }




    }

  


}
