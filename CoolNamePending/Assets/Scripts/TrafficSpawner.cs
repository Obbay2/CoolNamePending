using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour {

    private int iterator = 0;       // The iterator count.
    public GameObject[] cars;       // The car prefab to be spawned.
    public float spawnTime = 3f;    // How long between each spawn.
    public Transform spawnPoint;    // The spawn point the car can spawn from.

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Spawn() {
        if (iterator < cars.Length) {
            Instantiate(cars[iterator], spawnPoint.position, spawnPoint.rotation);
            iterator++;
        }
	}
}
