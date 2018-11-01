using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarFailureStates : MonoBehaviour {

    public GameObject orchestrator;
    private LevelSelect selector;
    private CarController carController;

    // Use this for initialization
    void Start () {
        selector = orchestrator.GetComponent<LevelSelect>();
        carController = GetComponent<CarController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (carController.CurrentSpeed > 20) // We don't really want magnitude here, this will say we died if we hit vertically at slow speed due to fast forward trajectory
        {
            print("Dead; Collision");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            print("Dead; Drowned");
            selector.SetLevel(selector.Level);
        }
        else if (other.name == "Level2Trigger" && selector.Level < 2)
        {
            selector.SetLevel(2);
        }
    }
}


