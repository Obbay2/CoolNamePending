using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour {

    public GameObject vehicle;
    public Text velocityText;
    public Text songText;

    private RadioScript radioScript;
    private Rigidbody rb;

    private double MpSToMpH = 2.23693629;

	// Use this for initialization
	void Start () {
        rb = vehicle.GetComponent<Rigidbody>();
        radioScript = vehicle.GetComponent<RadioScript>();
	}
	
	// Update is called once per frame
	void Update () {
        velocityText.text = System.Convert.ToString((int) (rb.velocity.magnitude * MpSToMpH)) + " mph";
        songText.text = radioScript.playingSource.clip.name;
	}
}
