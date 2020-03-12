using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour {

    float elapsedTime = 0f;
    float seedAverage = 0.8f;
    float average;

    public float timeToSpawn = 15f;
    public GameObject eggAnt;
    public GameObject seedAnt;

	// Use this for initialization
	void Start () {
        // get the prefabs
        
	}
	
	// Update is called once per frame
	void Update () {
        if (elapsedTime < timeToSpawn)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            average = Random.Range(0f, 1f);

            if (average <= seedAverage)
            {
                Instantiate(seedAnt, this.transform.position, this.transform.rotation);                
                Debug.Log("Spawn Seed Ant");
            }
            else
            {
                Instantiate(eggAnt, this.transform.position, this.transform.rotation);
                Debug.Log("Spawn Egg Ant");
            }

            elapsedTime = 0f;
        }
	}


}
