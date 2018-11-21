using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

public class CarFailureStates : MonoBehaviour {

    public delegate void LevelTriggerHandler(string trigger);
    public event LevelTriggerHandler OnLevelChangeTrigger;

    private Rigidbody rb;
    private float lastVelocity;

    public AudioSource crashSource;
    public GameObject fire;

    public GameObject LevelOrchestrator;
    private LevelSelect selector;

    private bool LevelChanging = false;

    // Use this for initialization
    void Start () {
        selector = LevelOrchestrator.GetComponent<LevelSelect>();
        selector.OnLevelChanged += LevelChanged;
        rb = GetComponent<Rigidbody>();
        lastVelocity = rb.velocity.magnitude;
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {
        //print("Current: " + rb.velocity.magnitude * 2.23693629f + " Last: " + lastVelocity);
        if (!LevelChanging)
        {
            if (rb.velocity.magnitude * 2.23693629f < lastVelocity - 5)
            {
                print("Crashed!");
                crashSource.Play();
                fire.SetActive(true);
                if (OnLevelChangeTrigger != null)
                {
                    LevelChanging = true;
                    OnLevelChangeTrigger("crash");
                    lastVelocity = 0f;
                    return;
                }
            }

            lastVelocity = rb.velocity.magnitude * 2.23693629f;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Level2Trigger")
        {
            if (OnLevelChangeTrigger != null)
            {
                LevelChanging = true;
                lastVelocity = 0f;
                OnLevelChangeTrigger(other.name);
            } 
        }
        else if (other.name == "Level3Trigger")
        {
            if (OnLevelChangeTrigger != null)
            {
                LevelChanging = true;
                lastVelocity = 0f;
                OnLevelChangeTrigger(other.name);
            }
        }
    }

    void LevelChanged(int level)
    {
        print("Inactivating Fire");
        LevelChanging = false;
        fire.SetActive(false);
    }
}


