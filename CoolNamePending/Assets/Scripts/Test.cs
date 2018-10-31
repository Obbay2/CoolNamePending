using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX;
using SharpDX.DirectInput;
using System;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public Text velocityText;

    // Use this for initialization
    void Start () {
        //Debug.Log("Hi");
        var directInput = new DirectInput();
        var guid = Guid.Empty;

        foreach (var inst in directInput.GetDevices(SharpDX.DirectInput.DeviceType.Driving, DeviceEnumerationFlags.AllDevices))
        {
            print(inst.InstanceName);
            Console.WriteLine(inst.InstanceName);
            velocityText.text = inst.InstanceName;
            guid = inst.InstanceGuid;
        }

        if (guid != Guid.Empty)
        {
            var joystick = new Joystick(directInput, guid);
            joystick.Acquire();

            //joystick.Properties.

            var allEffects = joystick.GetEffects();

            foreach (var effectInfo in allEffects)
            {
                velocityText.text += effectInfo.Name + "\n";

            }

            velocityText.text += allEffects[0].StaticParameters;
            velocityText.text += allEffects[0].DynamicParameters;

            //velocityText.text += allEffects[0].Type;

            ////Console.WriteLine("Effect available {0}", effectInfo.Name);
            EffectParameters b = new EffectParameters();
            velocityText.text += joystick.CreatedEffects;

            ////Effect blah = new Effect(joystick, guid, b);
            ////blah.Start();
        }

    }
	
}
