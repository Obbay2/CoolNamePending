using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class MessageManager : MonoBehaviour {

    public GameObject orchestrator;
    public GameObject playerVehicle;
    private LevelSelect levelSelect;

    public GameObject messageUI;
    public Text messageText;
    public Text nextMessageText;

    private CarUserControl carUserControl;

    // Use this for initialization
    void Start () {
        levelSelect = orchestrator.GetComponent<LevelSelect>();
        carUserControl = playerVehicle.GetComponent<CarUserControl>();
        levelSelect.OnLevelChangeShowMessage += LevelChanged;
    }

    void LevelChanged(int level, bool show)
    {
        StopAllCoroutines();
        if (show)
        {
            StartCoroutine(ShowMessages(level));
        }
    }

    IEnumerator ShowMessages(int level)
    {
        carUserControl.TakingInput = false;
        messageUI.SetActive(true);
        switch (level)
        {
            case 1:
                messageText.text = "You're on your way to a party in town.";
                yield return StartCoroutine(TextUtilities.ShowTextSmoothly(0.5f, 0.5f, 1, messageText, nextMessageText));
                messageText.text = "You've decided to take a route you've never driven before because of traffic.";
                yield return StartCoroutine(TextUtilities.ShowTextSmoothly(0.5f, 0.5f, 1, messageText, nextMessageText));
                break;
            case 2:
                break;
        }
        messageUI.SetActive(false);
        carUserControl.TakingInput = true;
    }
	
	
}
