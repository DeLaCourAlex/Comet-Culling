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
    //public DialogueRunner dialogueRunner;
    private DialogueRunner dialogueRunner;

    //[SerializeField] public string StartNode = Yarn.Dialogue.DefaultStartNodeName;


    //public PlayerController playerController;

    private void Start()
    {
        dialogueRunner = GameObject.FindObjectOfType<Yarn.Unity.DialogueRunner>();
        //DayCycle();
        //gameObject.SetActive(true);
    }

    // then we need a function to tell Yarn Spinner to start from {specifiedNodeName}
    //public string Day2;
    //public string Day4;
    //public string Day6;
    //public string Day6_LA;

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
