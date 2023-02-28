using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CROPTYPES { A, B, C}
public class CropTypes : MonoBehaviour
{
    //Parent class attributes and methods so each crop type inherits these qualities
    //Please ask abt visibility - wether public or protected, for these, and which would be the best practice
    public CROPTYPES type; 
    public int growthRate;

    public float energyYield { get; set; } 
    public int growthRateWatered { get; set; }
    public int growthRateDry { get; set; }
    public bool isWatered { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isWatered)
        {
            growthRate = growthRateWatered;
        }
        else {
            growthRate = growthRateDry;
        }
    }
}

public class CropA : CropTypes
{
    //Derived class attributes
   
    public CropA()
    {
        type = CROPTYPES.A; //Crop A is of crop type A -
                            ////now i'm not sure if we would need enums after all but let's leave it as a way to classify ig
        energyYield = 3.7f;
        growthRateWatered = 1;
        growthRateDry = 2; 
    }
}

public class CropB : CropTypes
{
    //Derived class attributes
    
    public CropB()
    {
        type = CROPTYPES.B; //Crop B is of crop type B
        energyYield = 5.5f;
        growthRateWatered = 2;
        growthRateDry = 3;
    }
}
