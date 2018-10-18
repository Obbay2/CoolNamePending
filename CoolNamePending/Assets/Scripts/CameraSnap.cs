using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSnap : MonoBehaviour {

    public Transform movingVehicle;
    public Transform VRCamera;
    public Transform nonVRCamera;

    private Vector3 initialVRPositionOffset;
    private Vector3 initialNonVRPositionOffset;
    private Vector3 initialNonVRRotationOffset;

	// Use this for initialization
	void Start () {
        initialVRPositionOffset = movingVehicle.position - VRCamera.position;
        initialNonVRPositionOffset = movingVehicle.position - nonVRCamera.position;
        initialNonVRRotationOffset = movingVehicle.eulerAngles + nonVRCamera.eulerAngles; // We don't care about VR rotation because the user controls this
    }
	
	// Update is called once per frame
	void Update () {
        VRCamera.position = movingVehicle.position - initialVRPositionOffset;
        nonVRCamera.position = movingVehicle.position - initialNonVRPositionOffset;
        nonVRCamera.eulerAngles = movingVehicle.eulerAngles - initialNonVRRotationOffset;
	}
}
