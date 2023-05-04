using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDayCycle : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        switch (TimeManager.Day)
        {
            case 1:
                gameObject.SetActive(false);
                break;
            case 2:

                gameObject.SetActive(true);
                break;
            case 3:
                gameObject.SetActive(false);
                break;
            case 4:

                gameObject.SetActive(true);
                break;
            case 5:
                gameObject.SetActive(false);
                break;
            case 6:

                gameObject.SetActive(true);
                break;
            case 7:
                gameObject.SetActive(false);
                break;

        }
    }
}
