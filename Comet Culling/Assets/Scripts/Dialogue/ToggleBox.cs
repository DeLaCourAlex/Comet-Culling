using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBox : MonoBehaviour
{

    public GameObject PlayerBox;
    public GameObject NPCBox;
    // Start is called before the first frame update
    public void PlayerDialogueBox()
    {
        PlayerBox.SetActive(true);
    }

    public void NPCDialogueBox()
    {
        NPCBox.SetActive(true);
    }
}
