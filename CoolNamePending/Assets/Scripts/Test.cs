using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX.DirectInput;
using System;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    public static System.IntPtr GetWindowHandle()
    {
        return GetActiveWindow();
    }

    public Text velocityText;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("Hi");
        //var directInput = new DirectInput();
        //var guid = Guid.Empty;

        //foreach (var inst in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Driving, DeviceEnumerationFlags.AllDevices))
        //{
        //    print(inst.InstanceName);
        //    guid = inst.InstanceGuid;
        //}

        //if (guid != Guid.Empty)
        //{
        //    var joystick = new Joystick(directInput, guid);
        //    joystick.SetCooperativeLevel(GetWindowHandle(), CooperativeLevel.Background | CooperativeLevel.Exclusive);
        //    joystick.Properties.AutoCenter = false;
        //    joystick.Acquire();

        //    var allEffects = joystick.GetEffects();

        //    foreach (var effectInfo in allEffects)
        //    {
        //        print(effectInfo.Name);
        //    }

        //    //velocityText.text += allEffects[0].StaticParameters;
        //    //velocityText.text += allEffects[0].DynamicParameters;

        //    //velocityText.text += allEffects[0].Type;

        //    ////Console.WriteLine("Effect available {0}", effectInfo.Name);
        //    EffectParameters b = new EffectParameters();
        //    //velocityText.text += joystick.CreatedEffects[0];
        //    print(joystick.CreatedEffects.Count);
        //    print(joystick.Capabilities.AxeCount);
        //    print(allEffects[0].DynamicParameters);

        //    b.SetAxes(new int[joystick.Capabilities.AxeCount], new int[joystick.Capabilities.AxeCount]);
        //    b.Gain = 10000;
        //    b.Duration = 2;
        //    b.Flags = EffectFlags.ObjectOffsets | EffectFlags.Cartesian;

        //    Effect blah = new Effect(joystick, allEffects[0].Guid, b);
        //    //blah.Start();
        //    //joystick.Dispose();
        //}
    }

}