using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTreeCollision : MonoBehaviour {

    public GameObject explosionParent;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        print(rb.velocity.magnitude);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.relativeVelocity.magnitude > 5)
        {
            print("Ded");
            explosionParent.SetActive(true);
        }
    }
}


