using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour {

    public GameObject car;          // The car prefab to be spawned.
    public float spawnTime = 3f;    // How long between each spawn.
    public Transform[] spawnPoints;    // The spawn point the car can spawn from.

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Spawn() {
        /*
		 * If we decide to have multiple spawn points then we just uncomment the code below
		 * int spawnPointIdx = Random.Range(0, spawnPoints.Length);
		 * 
		 */

        int spawnPointIdx = 0;

        Instantiate(car, spawnPoints[spawnPointIdx].position, spawnPoints[spawnPointIdx].rotation);
	}
}
