using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant_Blackboard : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] Waypoints;
    public GameObject[] ExitPoints;

    public float CloseEnoughRadius = 2f;

    void Start()
    {
        Waypoints = GameObject.FindGameObjectsWithTag("WAYPOINT");
        ExitPoints = GameObject.FindGameObjectsWithTag("EXITPOINT");
    }

    // Update is called once per frame
   
}
