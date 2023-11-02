using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    private GameObject spawnedPickup;
    public GameObject pickupPrefab;
    public float spawnDelay;
    private float nextSpawnTime;
    private Transform tf;


    //Start is called before the first frame update
    void Start()
    {
        //I dont like the delayed spawn so it always spawns immediately
        //nextSpawnTime = Time.time + spawnDelay;
    }

    //Update is called once per frame
    void Update()
    {
        //nothing spawns
        if (spawnedPickup == null)
        {
            //spawn
            if (Time.time > nextSpawnTime)
            {
                // Spawn it and set the next time
                spawnedPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity) as GameObject;
                nextSpawnTime = Time.time + spawnDelay;

                spawnedPickup.transform.parent = gameObject.transform;
            }
        }
        else
        {
            //object exists, delay spawn
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
