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

    public delegate void LevelChangingHandler();
    public event LevelChangingHandler OnLevelChanging;

    public delegate void ShowMessagesHandler(int level, bool show);
    public event ShowMessagesHandler OnLevelChangeShowMessage;

    public int FadeOutTime = 1;
    public int FadeInTime = 5;

    private CarFailureStates carStates;
    private CarUserControl carUserControl;

    private bool HMDActive = false;

    // Use this for initialization
    void Start()
    {
        HMDActive = OpenVR.IsHmdPresent();
        carStates = PlayerVehicle.GetComponent<CarFailureStates>();
        carUserControl = PlayerVehicle.GetComponent<CarUserControl>();
        StartCoroutine(SetLevel(Level, 0, false, false));
        carStates.OnLevelChangeTrigger += PlayerVehicleLevelChangeHandler;
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
        carUserControl.IsChangingLevel = true; // This script instance can't see the current script due to namespace issues otherwise we would have it subscribe to the following event
        if (OnLevelChanging != null)
        {
            OnLevelChanging();
        }

        if (delay && HMDActive)
        {
            SteamVR_Fade.View(Color.black, FadeOutTime);
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
                break;
            case 3:
                PlayerVehicle.SetActive(false);
                Terrain.SetActive(false);
                RoadNetwork.SetActive(false);
                RenderSettings.fogDensity = 0;
                break;
        }

        // The level "changes" before the player can see it, thus letting us reset other systems subscribed to this event
        if (OnLevelChanged != null)
        {
            OnLevelChanged(level);
        }

        // Show messages before we fade view back in (although the use won't see fade in, there is a delay if we put this after fade in)
        if (OnLevelChangeShowMessage != null)
        {
            OnLevelChangeShowMessage(level, showMessages);
        }

        // Extra one second wait before fade in makes the transition smoother
        if (delay && HMDActive)
        {
            yield return new WaitForSeconds(1.0f);
            SteamVR_Fade.View(Color.clear, FadeInTime);
            yield return new WaitForSeconds(FadeInTime);
        }

        

        carUserControl.IsChangingLevel = false; // Work around hack described above
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
            StartCoroutine(SetLevel(2, 3, true, true));
        }
        else if (triggerName == "Level3Trigger")
        {
            StartCoroutine(SetLevel(3, 0, true, false));
        }
    }

    public void ExternalSetLevel(int level, int difficulty)
    {
        StartCoroutine(SetLevel(level, difficulty, true, true));
    }

    private void TriggerLevelTwoWithDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 1: // easy
                carUserControl.SetInputLag(100);
                break;
            case 2: // medium
                InvokeRepeating("BlinkRandomizer", 5, 14);
                carUserControl.SetInputLag(200);
                break;
            case 3: // hard
                InvokeRepeating("BlinkRandomizer", 5, 7);
                carUserControl.SetInputLag(300);
                break;
        }
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
