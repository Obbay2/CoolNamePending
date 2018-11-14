using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

public class CarFailureStates : MonoBehaviour {

    public delegate void LevelTriggerHandler(string levelTriggerName);
    public event LevelTriggerHandler OnLevelTriggerEntered;

    public delegate void CollisionTriggerHandler();
    public event CollisionTriggerHandler OnCollision;

    public delegate void LeavingRoadHandler();
    public event LeavingRoadHandler OnLeavingRoad;
    public event LeavingRoadHandler OnEnteringRoad;

    public Text warningText;

    private bool OnRoad = true;

    private Rigidbody rb;
    private float lastVelocity;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        lastVelocity = rb.velocity.magnitude;
    }
	
	// Update is called once per frame
	void Update () {
        GroundCheck();
	}

    void FixedUpdate()
    {
        print("Current: " + rb.velocity.magnitude * 2.23693629f + " Last: " + lastVelocity);
        if(rb.velocity.magnitude * 2.23693629f < lastVelocity - 5)
        {
            print("Crashed!");
            if (OnCollision != null)
            {
                OnCollision();
            }
        }
        lastVelocity = rb.velocity.magnitude * 2.23693629f;

    }

    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "OtherCar")
        {
            print("Collided with other car");
            if (OnCollision != null)
            {
                OnCollision();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            print("Collided with water");
            if (OnCollision != null)
            {
                OnCollision();
            }
           
        }
        else if (other.name == "Level2Trigger")
        {
            if (OnLevelTriggerEntered != null)
            {
                OnLevelTriggerEntered(other.name);
            } 
        }
        else if (other.name == "Level3Trigger")
        {
            if (OnLevelTriggerEntered != null)
            {
                OnLevelTriggerEntered(other.name);
            }
        }
    }

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 0.5f;
        Vector3 dir = -transform.up;

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            if (OnRoad && hit.collider.tag == "Road")
            {
                OnRoad = true;
            }
            else if (OnRoad && hit.collider.tag != "Road")
            {
                if (OnLeavingRoad != null)
                {
                    OnLeavingRoad();
                }
                OnRoad = false;
                print("leaving");
                //Invoke("ActivateWarning", 2.0f);
            }
            else if (!OnRoad && hit.collider.tag == "Road")
            {
                if (OnEnteringRoad != null)
                {
                    OnEnteringRoad();
                }
                OnRoad = true;
                print("entering road");
                //CancelInvoke("ActivateWarning");
            }
            else if (!OnRoad && hit.collider.tag != "Road")
            {
                OnRoad = false;
            }
        }
    }

    private void ActivateWarning()
    {
        GameObject obj = warningText.transform.parent.gameObject;
        warningText.text = "Warning\nLeaving Roadway";
        obj.SetActive(true);
    }
}


