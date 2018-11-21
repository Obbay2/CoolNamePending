using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class TrafficSpawner : MonoBehaviour {

    private int iterator = 0;       // The iterator count.
    public GameObject[] cars;       // The car prefab to be spawned.
    public float spawnTime = 3f;    // How long between each spawn.
    public Transform spawnPoint;    // The spawn point the car can spawn from.

    private List<GameObject> spawnedCars = new List<GameObject>();

    public GameObject orchestrator;
    private LevelSelect selector;

    public WaypointCircuit circuit;

	// Use this for initialization
	void Start () {
	}

    private void Awake()
    {
        selector = orchestrator.GetComponent<LevelSelect>();
        selector.OnLevelChanged += LevelChanged;
    }

    // Update is called once per frame
    void Spawn() {
        if (iterator < cars.Length) {
            GameObject newCar = Instantiate(cars[iterator], spawnPoint.position, spawnPoint.rotation);
            newCar.GetComponent<BetterWaypointFollower>().circuit = circuit;
            spawnedCars.Add(newCar);
            iterator++;
        }
	}

    void LevelChanged(int level)
    {
        CancelInvoke("Spawn");
        foreach (GameObject car in spawnedCars)
        {
            Destroy(car);
        }
        spawnedCars.Clear();

        if (transform.parent.name == "DayTimeObjects" && level == 1)
        {
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }
        else if (transform.parent.name == "NightTimeObjects" && level == 2)
        {
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }
            
    }
}
