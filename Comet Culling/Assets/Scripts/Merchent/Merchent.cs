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
    public DialogueRunner dialogueRunner;
    //public string StartNode = Yarn.Dialogue.DefaultStartNodeName;


    //public PlayerController playerController;

    private void Start()
    {
        //DayCycle();
        //gameObject.SetActive(true);
    }
    //player detection
    bool player_detection = false;

    // Update is called once per frame
    void Update()
    {
        //DayCycle();

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


    //private void DayCycle()
    //{
    //    switch (TimeManager.Day)
    //    {
    //        case 1:
              
    //            break;
    //        case 2:
    //            //Dialogue.StartNode("Day_2");
    //            break;
    //        case 3:
                
    //            break;
    //        case 4:
    //            gameObject.SetActive(true);
    //            break;
    //        case 5:
    //            gameObject.SetActive(false);
    //            break;
    //        case 6:
    //            gameObject.SetActive(true);
    //            break;
    //        case 7:
    //            gameObject.SetActive(false);
    //            break;

    //    }


    //}

}
