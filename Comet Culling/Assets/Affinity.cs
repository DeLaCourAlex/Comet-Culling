using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class Affinity : MonoBehaviour
{
    public GameObject AffinityText;

    private DialogueRunner dialogueRunner;
    private InMemoryVariableStorage variableStorage;
    public bool NPCAffinity;
    public bool getAffinity() { return NPCAffinity; }


    public void Start()
    {
        NPCAffinity = DataPermanence.Instance.NPCAffinity;
        dialogueRunner = GameObject.FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.AddFunction<bool>("get_Affinity", getAffinity);
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        //UpdateAffinity();
    }

    //public void AffinityFunc()
    //{
    //    dialogueRunner = GameObject.FindObjectOfType<Yarn.Unity.DialogueRunner>();
    //    dialogueRunner.AddFunction<bool>("get_Affinity", getAffinity);
    //    variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
    //    //if (DataPermanence.Instance != null)
    //    //{
    //    //    Debug.Log("START IS BEING CALLED");

    //    //    NPCAffinity = DataPermanence.Instance.affinityPermanence;
    //    //}
    //}
    public void Update()
    {
        if(dialogueRunner != null)
        {
            variableStorage.TryGetValue("$affinity", out NPCAffinity);
            variableStorage.SetValue("$affinity", NPCAffinity);
            Debug.Log("AFFINITY SCRIPT AFFINITY IS SET TO: " + DataPermanence.Instance.NPCAffinity);
        }
        
        //Debug.Log("PERMANENCE AFFINITY IS SET TO: " + DataPermanence.Instance.NPCAffinity);

        //DataPermanence.Instance.NPCAffinity = NPCAffinity;
        //UpdateAffinity(); 
    }

    public void UpdateAffinity()
    {
        variableStorage.TryGetValue("$affinity", out NPCAffinity);
        variableStorage.SetValue("$affinity", NPCAffinity);
        NPCAffinity = DataPermanence.Instance.NPCAffinity;

    }

    //activate affnity text when complete dialogue
    [YarnCommand("HighAffinity")]
    public void High_affinityFunc(bool Like)
    {
        if (Like)
        {
            AffinityText.SetActive(true);
            //DataPermanence.Instance.highAffinity = true;
            Debug.Log("High affinity");
        }
    }


    [YarnCommand("LowAffinity")]
    public void Low_AffinityFunc(bool Hate)
    {
        if (Hate)
        {
            AffinityText.SetActive(false);
            //DataPermanence.Instance.highAffinity = false;
            Debug.Log("Low affinity");
        }
    }
}