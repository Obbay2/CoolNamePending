using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour {

    public GameObject PlayerVehicle;
    private Rigidbody VehicleRb;

    public Transform Level1Start;
    public Transform Level2Start;

    public GameObject DayObjects;
    public GameObject NightObjects;

    public Material DaySkyBox;
    public Material NightSkyBox;

    public GameObject DayCamera;
    public GameObject NightCamera;

    public int Level = 1;

	// Use this for initialization
	void Start () {
        SetLevel(1);
        VehicleRb = PlayerVehicle.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetLevelDay();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SetLevelNight();
        }
        
    }

    public void SetLevel(int level)
    {
        switch (level)
        {
            case 1: SetLevelDay(); break;
            case 2: SetLevelNight(); break;
        }

        Level = level;
    }

    public void SetLevelDay()
    {
        PlayerVehicle.transform.eulerAngles = Level1Start.eulerAngles;
        PlayerVehicle.transform.position = Level1Start.transform.position;
        NightObjects.SetActive(false);
        DayObjects.SetActive(true);
        RenderSettings.skybox = DaySkyBox;
        NightCamera.SetActive(false);
        DayCamera.SetActive(true);
        VehicleRb.velocity = Vector3.zero;
    }

    public void SetLevelNight()
    {
        PlayerVehicle.transform.eulerAngles = Level2Start.eulerAngles;
        PlayerVehicle.transform.position = Level2Start.transform.position;
        NightObjects.SetActive(true);
        DayObjects.SetActive(false);
        RenderSettings.skybox = NightSkyBox;
        NightCamera.SetActive(true);
        DayCamera.SetActive(false);
        VehicleRb.velocity = Vector3.zero;
    }
}
