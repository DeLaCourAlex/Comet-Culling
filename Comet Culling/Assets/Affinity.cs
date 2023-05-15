using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class Affinity : MonoBehaviour
{
    public GameObject AffinityText;

    public bool affinity;
    [YarnCommand("ToggleAffinity")]
    public void toggleAffinity(bool affinity_)
    {
        affinity = affinity_;
    }

    //activate affnity text when complete dialogue
    [YarnCommand("HighAffinity")]
    public void High_affinityFunc(bool Like)
    {
        if (Like)
        {
            AffinityText.SetActive(true);
            DataPermanence.Instance.highAffinity = true;
            Debug.Log("High affinity");
        }
     }


    [YarnCommand("LowAffinity")]
    public void Low_AffinityFunc(bool Hate)
    {
        if (Hate)
        {
            AffinityText.SetActive(false);
            DataPermanence.Instance.highAffinity = false;
            Debug.Log("Low affinity");
        }
    }

   
}
