using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class Affinity : MonoBehaviour
{
    public GameObject AffinityText;

    //activate affnity text when complete dialogue
    [YarnCommand("HighAffinity")]
    public void High_affinityFunc(bool Like)
    {
        if (Like)
        {
            AffinityText.SetActive(true);
        }
     }


    [YarnCommand("LowAffinity")]
    public void Low_AffinityFunc(bool Hate)
    {
        if (Hate)
        {
            AffinityText.SetActive(false);
        }
    }
}
