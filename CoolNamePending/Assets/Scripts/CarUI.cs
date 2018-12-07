using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour {

    public GameObject vehicle;
    public Text velocityText;
    public Text songText;
    public Text timerText;

    private RadioScript radioScript;
    private Rigidbody rb;
    private float startTime;

    private double MpSToMpH = 2.23693629;

	// Use this for initialization
	void Start () {
        rb = vehicle.GetComponent<Rigidbody>();
        radioScript = vehicle.GetComponent<RadioScript>();

        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        velocityText.text = System.Convert.ToString((int) (rb.velocity.magnitude * MpSToMpH * 7 / 8)) + " mph";
        songText.text = radioScript.playingSource.clip.name;

        float t = Time.time - startTime;

        string min = ((int)t / 60).ToString("00");
        string sec = (t % 60).ToString("00");

        timerText.text = min + ":" + sec;
    }

    public void ResetStartTime()
    {
        startTime = Time.time;
    }
}
