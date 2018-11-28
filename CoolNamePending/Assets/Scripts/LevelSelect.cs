using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Effects;
using PostProcess;
using Valve.VR;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    public GameObject PlayerVehicle;
    public GameObject HeadLights;
    public GameObject Terrain;
    public GameObject RoadNetwork;

    public GameObject MessageUI;
    public Text MessageText;
    public Text NextMessageText;

    public Transform Level1Start;
    public Transform Level2Start;

    public static int numberOfLevels = 4;

    [SerializeField] private GameObject[] levelObjects = new GameObject[numberOfLevels]; // Level 0: Game Start UI, Level 1: Day time, Level 2: Night time, Level 3: Ending

    [SerializeField] private Material[] levelSkybox = new Material[numberOfLevels]; // Level 0: Game Start UI, Level 1: Day time, Level 2: Night time, Level 3: Ending

    [SerializeField] public GameObject[] levelCameras = new GameObject[numberOfLevels]; // Level 0: Game Start UI, Level 1: Day time, Level 2: Night time, Level 3: Ending

    [SerializeField] public PostProcessingProfile[] postProcessingProfiles = new PostProcessingProfile[3];

    public int Level = 0;

    public delegate void LevelChangedHandler(int level);
    public event LevelChangedHandler OnLevelChanged;

    public int FadeOutTime = 1;
    public int FadeInTime = 5;

    private CarFailureStates carStates;
    private CarUserControl carUserControl;
    private CarController carController;

    private bool HMDActive = false;

    // Use this for initialization
    void Start()
    {
        HMDActive = OpenVR.IsHmdPresent();
        carStates = PlayerVehicle.GetComponent<CarFailureStates>();
        carUserControl = PlayerVehicle.GetComponent<CarUserControl>();
        carController = PlayerVehicle.GetComponent<CarController>();
        StartCoroutine(SetLevel(Level, 0, false, false));
        carStates.OnLevelChangeTrigger += PlayerVehicleLevelChangeHandler;

        RenderSettings.ambientSkyColor = new Color32(54, 58, 66, 0);
        RenderSettings.ambientEquatorColor = new Color32(29, 32, 34, 0);
        RenderSettings.ambientGroundColor = new Color32(12, 11, 9, 0);
        RenderSettings.fog = true;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(SetLevel(0, 0, false, false));
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
           StartCoroutine(SetLevel(1, 0, false, true));
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            StartCoroutine(SetLevel(2, 3, false, true));
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            StartCoroutine(SetLevel(3, 0, false, false));
        }
    }

    public IEnumerator SetLevel(int level, int difficulty, bool delay, bool showMessages)
    {
        carUserControl.TakingInput = false;
        if (delay && HMDActive)
        {
            SteamVR_Fade.View(Color.black, FadeOutTime);
            Invoke("FadeIn", FadeOutTime);
            yield return new WaitForSeconds(FadeOutTime);
        }

        EnableObjects(level, levelCameras);
        EnableObjects(level, levelObjects);

        SetSkybox(level);
        Level = level;
        CancelInvoke("BlinkRandomizer");
        CancelInvoke("Blink");
        switch (level)
        {
            case 0:
                PlayerVehicle.SetActive(false);
                Terrain.SetActive(false);
                RoadNetwork.SetActive(false);
                RenderSettings.fogDensity = 0;
                break;
            case 1:
                PlayerVehicle.SetActive(true);
                HeadLights.SetActive(false);
                Terrain.SetActive(true);
                RoadNetwork.SetActive(true);
                SetVehiclePosition(Level1Start);
                RenderSettings.fogDensity = 0.0005f;
                RenderSettings.ambientSkyColor = new Color(0.8510008f, 0.9106251f, 1.035294f);
                RenderSettings.ambientEquatorColor = new Color(0.454902f, 0.5019608f, 0.5333334f);
                RenderSettings.ambientGroundColor = new Color(0.1882353f, 0.172549f, 0.1411765f);
                if (showMessages)
                {
                    print("Showing Level Messages");
                    print(level);
                    yield return StartCoroutine(ShowLevelMessages(level));
                }
                break;
            case 2:
                PlayerVehicle.SetActive(true);
                HeadLights.SetActive(true);
                Terrain.SetActive(true);
                RoadNetwork.SetActive(true);
                SetVehiclePosition(Level2Start);
                TriggerLevelTwoWithDifficulty(difficulty);
                levelCameras[2].GetComponent<PostProcessingBehaviour>().profile = postProcessingProfiles[difficulty - 1];
                RenderSettings.fogDensity = 0.002f;
                RenderSettings.ambientSkyColor = new Color(0.05318755f, 0.05691407f, 0.06470588f);
                RenderSettings.ambientEquatorColor = new Color(0.02843137f, 0.03137255f, 0.03333334f);
                RenderSettings.ambientGroundColor = new Color(0.01176471f, 0.01078431f, 0.00882353f);
                if (showMessages)
                {
                    yield return StartCoroutine(ShowLevelMessages(level));
                }
                break;
            case 3:
                PlayerVehicle.SetActive(false);
                Terrain.SetActive(false);
                RoadNetwork.SetActive(false);
                RenderSettings.fogDensity = 0;
                break;
        }

        carUserControl.TakingInput = true;
        if (OnLevelChanged != null)
        {
            OnLevelChanged(level);
        }
    }
    
    private void TriggerLevelTwoWithDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 1: // easy
                break;
            case 2: // medium
                InvokeRepeating("BlinkRandomizer", 5, 14);
                break;
            case 3: // hard
                InvokeRepeating("BlinkRandomizer", 5, 7);
                break;
        }
    }

    private void EnableObjects(int level, GameObject[] objects)
    {
        if (objects[level] != null)
        {
            objects[level].SetActive(true); // We do this first to make sure we don't have weird camera flicker
        }
        
        for (int i = 0; i < numberOfLevels; i++)
        {
            if (i != level && objects[i] != null)
            {
                objects[i].SetActive(false);
            }
        }
    }

    private void SetVehiclePosition(Transform t)
    {
        PlayerVehicle.transform.eulerAngles = t.eulerAngles;
        PlayerVehicle.transform.position = t.transform.position;
        PlayerVehicle.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerVehicle.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void SetSkybox(int level)
    {
        if(levelSkybox[level] != null)
        {
            RenderSettings.skybox = levelSkybox[level];
        }
    }

    private void FadeIn()
    {
        SteamVR_Fade.View(Color.clear, FadeInTime);
    }

    private void PlayerVehicleLevelChangeHandler(string triggerName)
    {
        if (triggerName == "crash")
        {
            switch(Level)
            {
                case 1:
                    StartCoroutine(SetLevel(1, 0, true, false));
                    break;
                case 2:
                    StartCoroutine(SetLevel(3, 0, true, false));
                    break;
            }
        }
        else if (triggerName == "Level2Trigger")
        {
            carUserControl.TakingInput = false;
            carController.Move(0, 0, -1, 1);
            StartCoroutine(SetLevel(2, 3, true, true));
        }
        else if (triggerName == "Level3Trigger")
        {
            StartCoroutine(SetLevel(3, 0, true, false));
        }
    }

    private IEnumerator ShowLevelMessages(int level)
    {
        print(level);
        MessageUI.SetActive(true);
        switch(level)
        {
            case 1:
                MessageText.text = "You're on your way to a party in town.";
                yield return StartCoroutine(TextUtilities.ShowTextSmoothly(0.5f, 0.5f, 1, MessageText, NextMessageText));
                MessageText.text = "You've decided to take a route you've never driven before because of traffic.";
                yield return StartCoroutine(TextUtilities.ShowTextSmoothly(0.5f, 0.5f, 1, MessageText, NextMessageText));
                break;
            case 2:
                break;
        }
        MessageUI.SetActive(false);
    }

    void BlinkRandomizer()
    {
        Invoke("Blink", Random.Range(1, 3));
    }

    void Blink()
    {
        levelCameras[2].GetComponent<BlinkEffect>().Blink();
    } 
}
