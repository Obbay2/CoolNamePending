using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using SharpDX.DirectInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        public bool Controller;
        public bool SteeringWheel;
        public Transform SteeringWheelMesh;
        private CarController m_Car; // the car controller we want to use
        private static int SteerMultiplier = 450;

        private void Start()
        {
            var directInput = new DirectInput();
            var guid = Guid.Empty;

            foreach (var inst in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Driving, DeviceEnumerationFlags.AllDevices))
            {
                print(inst.InstanceName);
                Console.WriteLine(inst.InstanceName);
            }
        }


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            if (Controller)
            {
                float v = Input.GetAxis("Right Trigger") - Input.GetAxis("Left Trigger");
                float h = Input.GetAxis("Left Joystick");
                m_Car.Move(h, v, v, 0);
            }
            else if (SteeringWheel)
            {
                float footbrake = Input.GetAxis("Footbrake"); // -1 to 1 => 0 to 1
                footbrake = (footbrake + 1) / 2;
                float h = Input.GetAxis("SteeringWheel"); // -1 to 1
                float v = Input.GetAxis("Accelerator"); // -1 to 1 => 0 to 1
                v = (v + 1) / 2;
                SteeringWheelMesh.eulerAngles = new Vector3(SteeringWheelMesh.eulerAngles.x, SteeringWheelMesh.eulerAngles.y, h * SteerMultiplier);
                //print(v + " " + h + " " + footbrake);
                m_Car.Move(h, v, footbrake, 0);
            }
            else
            {
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");
                m_Car.Move(h, v, v, 0);
            }
            
            
        }
    }
}
