using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyBug_BlackBoard : MonoBehaviour
{

    [HideInInspector]public GameObject []WayPoints;
    [HideInInspector]public GameObject []StorePoints;
    [HideInInspector]public GameObject []HatchingPoints;

    public float CloseEnoughRadius = 3f;
    public float CloseEnoughRadiusToItem = 2f;

    //EGG Detected Radius

    public float CloseEggDetectedWithSeedRadius = 25f;
    public float CloseEggDetectedRadius = 50f;
    public float FarEggDetectedRadius = 180f;

    //SEED Detected Radius

    public float CloseSeedDetectedRadius = 80f;
    public float FarSeedDetectedRadius = 125f;



    void Start() 
    {
        WayPoints = GameObject.FindGameObjectsWithTag("WAYPOINT");
        StorePoints = GameObject.FindGameObjectsWithTag("STOREPOINT");
        HatchingPoints = GameObject.FindGameObjectsWithTag("HATCHINGPOINT");

  
    }
   
}
