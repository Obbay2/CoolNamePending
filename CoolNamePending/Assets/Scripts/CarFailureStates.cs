using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFailureStates : MonoBehaviour {


    private Rigidbody rb;
    private CarReset carReset;

    private double MpSToMpH = 2.23693629;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        carReset = GetComponent<CarReset>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude * MpSToMpH > 20) // We don't really want magnitude here, this will say we died if we hit vertically at slow speed due to fast forward trajectory
        {
            print("Dead; Collision");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            print("Dead; Drowned");
            carReset.Reset();
        }
    }
}


