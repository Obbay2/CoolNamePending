using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System;
using System.Collections.Generic;

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

        public bool TakingInput = true;

        private bool hasTouchedAccelerator;
        private float initialThrottle;

        public bool IsChangingLevel = false;

        public float inputLagMs;

        private Queue<float> controllerHQueue = new Queue<float>();
        private Queue<float> vQueue = new Queue<float>();
        private Queue<float> hQueue = new Queue<float>();
        private Queue<float> accQueue = new Queue<float>();
        private Queue<float> footbrakeQueue = new Queue<float>();
        List<Queue<float>> inputQueues = new List<Queue<float>>();

        void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            hasTouchedAccelerator = false;
            initialThrottle = Input.GetAxis("Accelerator");
            print(hasTouchedAccelerator + " " + initialThrottle);
            inputQueues.Add(controllerHQueue);
            inputQueues.Add(vQueue);
            inputQueues.Add(hQueue);
            inputQueues.Add(accQueue);
            inputQueues.Add(footbrakeQueue);
            inputLagMs = 0;
        }

        public void SetInputLag(float ms)
        {
            inputLagMs = ms;
            foreach (Queue<float> q in inputQueues)
            {
                q.Clear();
                for (int i = 0; i < ms / 20; i++)
                {
                    q.Enqueue(0);
                }
            }
        }


        void FixedUpdate()
        {
            if (TakingInput && !IsChangingLevel)
            {
                // pass the input to the car!
                float h;
                if (Controller)
                {
                    float newV = Input.GetAxis("Right Trigger") - Input.GetAxis("Left Trigger");
                    float newControllerH = Input.GetAxis("Left Joystick");
                    vQueue.Enqueue(newV);
                    controllerHQueue.Enqueue(newControllerH);

                    float v = vQueue.Dequeue();
                    h = hQueue.Dequeue();
                    m_Car.Move(h, v, v, 0);
                }
                else if (SteeringWheel)
                {
                    hQueue.Enqueue(Input.GetAxis("SteeringWheel")); // -1 to 1
                    footbrakeQueue.Enqueue((Input.GetAxis("Footbrake") + 1) / 2);

                    float new_acc = Input.GetAxis("Accelerator"); // -1 to 1 => 0 to 1                    
                                                              //print("Unscaled accelerator value: " + footbrake

                    if (new_acc == initialThrottle && !hasTouchedAccelerator)
                    {
                        new_acc = 0;
                    }
                    else
                    {
                        hasTouchedAccelerator = true;
                        new_acc = (new_acc + 1) / 2;
                    }
                    accQueue.Enqueue(new_acc);
                    h = hQueue.Dequeue();
                    m_Car.Move(h, accQueue.Dequeue(), 0, footbrakeQueue.Dequeue());

                }
                else
                {
                    controllerHQueue.Enqueue(CrossPlatformInputManager.GetAxis("Horizontal"));
                    vQueue.Enqueue(CrossPlatformInputManager.GetAxis("Vertical"));

                    float v = vQueue.Dequeue();
                    h = controllerHQueue.Dequeue();
                    m_Car.Move(h, v, v, 0);
                }

                SteeringWheelMesh.eulerAngles = new Vector3(SteeringWheelMesh.eulerAngles.x, SteeringWheelMesh.eulerAngles.y, h * SteerMultiplier);
            }
            else
            {
                m_Car.Move(0, 0, 0, 1);
            }
            
        }
    }
}
