using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        // return to main menu if F1 or a button on steering wheel is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("MainScene");
        }
	}
    
    void SwitchToMain()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }
}
