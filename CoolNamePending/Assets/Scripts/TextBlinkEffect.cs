using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBlinkEffect : MonoBehaviour {

    public GameObject blinkText;
    public float flashTime = 0.25f;

    private bool isOn = true;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SetText", flashTime, flashTime);
    }

    void SetText()
    {
        if (isOn)
        {
            blinkText.SetActive(false);
            isOn = false;
        }
        else
        {
            blinkText.SetActive(true);
            isOn = true;
        }
    }
}
