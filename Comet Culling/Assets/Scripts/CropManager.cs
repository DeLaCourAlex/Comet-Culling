// Used for any scenes which contains crops
// Takes all the crop data from the data permanence class and uses it to initialize crops

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    // A reference to the different crop types
    [SerializeField] GameObject crop;

    // Start is called before the first frame update
    void Start()
    {
        // Cycle through the list of crops
        // Instantiate each crop with the correct position and time alive
        for(int i = 0; i < DataPermanence.Instance.allCrops.Count; i++)
        {
            // Instantiate the crop
            GameObject newCrop = Instantiate(crop, DataPermanence.Instance.allCrops[i].position, Quaternion.identity);
            // Access its crop controller to set the time alive from that saved in the data permanence class
            CropController cropController = newCrop.GetComponent<CropController>();
            cropController.timeAlive = DataPermanence.Instance.allCrops[i].timeAlive;
        }

        // Then delete the list of crops once they've all been instantiated
        DataPermanence.Instance.allCrops.Clear();
    }
}
