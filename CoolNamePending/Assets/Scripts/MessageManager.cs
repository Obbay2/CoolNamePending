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

    public delegate void MessageCompleteHandler();
    public event MessageCompleteHandler MessageComplete;

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
        //levelSelect.startTime = 0.0f;
        levelSelect.startCounting = false;
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
                messageText.text = "You were at a party and had a few too many drinks.";
                yield return StartCoroutine(TextUtilities.ShowTextSmoothly(0.5f, 0.5f, 1, messageText, nextMessageText));
                messageText.text = "Your mom called and she said to get home quick something bad has happened.";
                yield return StartCoroutine(TextUtilities.ShowTextSmoothly(0.5f, 0.5f, 1, messageText, nextMessageText));
                messageText.text = "She didn't say why...";
                yield return StartCoroutine(TextUtilities.ShowTextSmoothly(0.5f, 0.5f, 1, messageText, nextMessageText));
                break;
        }
        messageUI.SetActive(false);
        carUserControl.TakingInput = true;
        levelSelect.startCounting = true;

        if(MessageComplete != null)
        {
            MessageComplete();
        }
    }
	
	
}
