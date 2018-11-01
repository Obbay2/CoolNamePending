using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopLightChange : MonoBehaviour {

    public GameObject RedLight;
    public GameObject BlueLight;
    public Material RedMat;
    public Material BlueMat;

    public float flashingTime;
    public float startupTime;

    private bool lightOn = true; // True for blue, false for red

	// Use this for initialization
	void Start () {
        Invoke("StartUpLights", startupTime); // Add to the chaos
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartUpLights()
    {
        InvokeRepeating("SetLights", flashingTime, flashingTime);
    }

    void SetLights()
    {
        if (lightOn)
        {
            BlueMat.DisableKeyword("_EMISSION");
            BlueLight.SetActive(false);
            RedMat.EnableKeyword("_EMISSION");
            RedLight.SetActive(true);
            lightOn = false;
        }
        else
        {
            BlueMat.EnableKeyword("_EMISSION");
            BlueLight.SetActive(true);
            RedMat.DisableKeyword("_EMISSION");
            RedLight.SetActive(false);
            lightOn = true;
        }
    }
}
