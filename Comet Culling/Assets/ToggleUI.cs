using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class ToggleUI : MonoBehaviour
{
    public GameObject staminaBar;
    public GameObject energyBar;
    public GameObject blurScreen;
  
    //activates UI objects while in dialogue
    [YarnCommand("ActivateUI")]
    public void Activate_UI()
    {
        staminaBar.SetActive(true);
        energyBar.SetActive(true);
        blurScreen.SetActive(false);
       
    }
    //deactivates UI objects after completing dialogue
    [YarnCommand("DeactivateUI")]
    public void Deactivate_UI()
    {
        staminaBar.SetActive(false);
        energyBar.SetActive(false);
        blurScreen.SetActive(true);
      
    }

   

   

}
