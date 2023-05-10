using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class ToggleUI : MonoBehaviour
{
    public GameObject staminaBar;
    public GameObject energyBar;
    public GameObject blurScreen;
    public GameObject PlayerDialogueBox;
    public GameObject NPCDialogueBox;

    //activates UI objects while in dialogue
    [YarnCommand("ActivateUI")]
    public void Activate_UI()
    {
        staminaBar.SetActive(true);
        energyBar.SetActive(true);
        blurScreen.SetActive(false);
        PlayerDialogueBox.SetActive(false);
        NPCDialogueBox.SetActive(false);
    }
    //deactivates UI objects after completing dialogue
    [YarnCommand("DeactivateUI")]
    public void Deactivate_UI()
    {
        staminaBar.SetActive(false);
        energyBar.SetActive(false);
        blurScreen.SetActive(true);
        PlayerDialogueBox.SetActive(true);
        NPCDialogueBox.SetActive(true);
    }

   

    // Start is called before the first frame update

}
