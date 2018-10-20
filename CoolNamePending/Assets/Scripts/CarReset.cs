using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarReset : MonoBehaviour {

    public GameObject Car;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

	// Use this for initialization
	void Start () {
        initialPosition = GetComponent<Transform>().position;
        initialRotation = GetComponent<Transform>().eulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Transform t = Car.GetComponent<Transform>();
            t.position = initialPosition;
            t.eulerAngles = initialRotation;
            Rigidbody rb = Car.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.Sleep();
        }
    }
}
