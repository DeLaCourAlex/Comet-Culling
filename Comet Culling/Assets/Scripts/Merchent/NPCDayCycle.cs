using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NPCDayCycle : MonoBehaviour
{
    public GameObject NPC;
    // Update is called once per frame
    void Update()
    {
        switch (TimeManager.Day)
        {
            case 1:
                NPC.SetActive(false);
                break;
            case 2:

                NPC.SetActive(true);
                break;
            case 3:
                NPC.SetActive(false);
                break;
            case 4:

                NPC.SetActive(true);
                break;
            case 5:
                NPC.SetActive(false);
                break;
            case 6:

                NPC.SetActive(true);
                break;
            case 7:
                NPC.SetActive(false);
                break;

        }
    }
}
