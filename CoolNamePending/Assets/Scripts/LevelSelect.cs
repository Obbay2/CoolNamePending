using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour {

    public Transform PlayerVehicle;

    public Transform Level1Start;
    public Transform Level2Start;

    public GameObject DayObjects;
    public GameObject NightObjects;

    public Material DaySkyBox;
    public Material NightSkyBox;

    public GameObject DayCamera;
    public GameObject NightCamera;

	// Use this for initialization
	void Start () {
		
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

    public void SetLevelDay()
    {
        PlayerVehicle.eulerAngles = Level1Start.eulerAngles;
        PlayerVehicle.transform.position = Level1Start.transform.position;
        NightObjects.SetActive(false);
        DayObjects.SetActive(true);
        RenderSettings.skybox = DaySkyBox;
        NightCamera.SetActive(false);
        DayCamera.SetActive(true);
    }

    public void SetLevelNight()
    {
        PlayerVehicle.eulerAngles = Level2Start.eulerAngles;
        PlayerVehicle.transform.position = Level2Start.transform.position;
        NightObjects.SetActive(true);
        DayObjects.SetActive(false);
        RenderSettings.skybox = NightSkyBox;
        NightCamera.SetActive(true);
        DayCamera.SetActive(false);
    }
}
