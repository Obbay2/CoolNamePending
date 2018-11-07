using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Effects;
using PostProcess;

public class LevelSelect : MonoBehaviour {

    public GameObject PlayerVehicle;
    public GameObject Terrain;
    public GameObject RoadNetwork;

    public Transform Level0Start;
    public Transform Level1Start;
    public Transform Level2Start;

    public static int numberOfLevels = 4;

    [SerializeField] private GameObject[] levelObjects = new GameObject[numberOfLevels]; // Level 0: Game Start UI, Level 1: Day time, Level 2: Night time, Level 3: Ending

    [SerializeField] private Material[] levelSkybox = new Material[numberOfLevels]; // Level 0: Game Start UI, Level 1: Day time, Level 2: Night time, Level 3: Ending

    [SerializeField] public GameObject[] levelCameras = new GameObject[numberOfLevels]; // Level 0: Game Start UI, Level 1: Day time, Level 2: Night time, Level 3: Ending

    public PostProcessingProfile profile;

    public int Level = 1;

    // Use this for initialization
    void Start()
    {
        SetLevel(1);
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetLevel(0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SetLevel(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SetLevel(2);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            SetLevel(3);
        }
    }

    public void SetLevel(int level)
    {
        switch (level)
        {
            case 0:
                PlayerVehicle.SetActive(false);
                Terrain.SetActive(false);
                RoadNetwork.SetActive(false);
                break;
            case 1:
                PlayerVehicle.SetActive(true);
                Terrain.SetActive(true);
                RoadNetwork.SetActive(true);
                SetVehiclePosition(Level1Start);
                break;
            case 2:
                PlayerVehicle.SetActive(true);
                Terrain.SetActive(true);
                RoadNetwork.SetActive(true);
                SetVehiclePosition(Level2Start);
                CancelInvoke("BlinkRandomizer");
                CancelInvoke("Blink");
                InvokeRepeating("BlinkRandomizer", 5, 7);
                break;
            case 3:
                PlayerVehicle.SetActive(false);
                Terrain.SetActive(false);
                RoadNetwork.SetActive(false);
                break;
        }

        EnableObjects(level, levelCameras);
        EnableObjects(level, levelObjects);
        SetSkybox(level);
        Level = level;
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
    }

    private void SetSkybox(int level)
    {
        if(levelSkybox[level] != null)
        {
            RenderSettings.skybox = levelSkybox[level];
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
