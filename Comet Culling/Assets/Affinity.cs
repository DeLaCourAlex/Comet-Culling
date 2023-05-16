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
    public static bool NPCAffinity;
    public bool getAffinity() { return DataPermanence.Instance.highAffinity; }


    public void Start()
    {
        dialogueRunner = GameObject.FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.AddFunction<bool>("get_Affinity", getAffinity);
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();


        //NPCAffinity = DataPermanence.Instance.highAffinity;

        //variableStorage.TryGetValue("$affinity", out NPCAffinity);
        //variableStorage.SetValue("$affinity", NPCAffinity);
    }


    //activate affnity text when complete dialogue
    [YarnCommand("HighAffinity")]
    public void High_affinityFunc(bool Like)
    {
        if (Like)
        {
            AffinityText.SetActive(true);
            DataPermanence.Instance.highAffinity = true;
            NPCAffinity = true;
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
            NPCAffinity = false;
            Debug.Log("Low affinity");
        }
    }

    public void Update()
    {

        if (NPCAffinity == true)
        {
            DataPermanence.Instance.highAffinity = true;
        }
        if (DataPermanence.Instance.highAffinity == true)
        {
            NPCAffinity = true;
        }


        variableStorage.TryGetValue("$affinity", out DataPermanence.Instance.highAffinity);
        variableStorage.SetValue("$affinity", DataPermanence.Instance.highAffinity);
       
        Debug.Log("PERMANENCE AFFINITY IS SET TO: " + DataPermanence.Instance.highAffinity);



        //DataPermanence.Instance.highAffinity = NPCAffinity;
        //UpdateAffinity(); 
    }










}
