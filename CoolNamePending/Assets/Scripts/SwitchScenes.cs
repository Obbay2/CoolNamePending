using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour {

    private void Start()
    {
        StartCoroutine(SwitchToMain());
    }

    // Update is called once per frame
    void Update () {
        // return to main menu if F1 or a button on steering wheel is pressed
        /*if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("MainScene");
        }*/
	}

    IEnumerator SwitchToMain()
    {
        yield return null;
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("MainScene");
        asyncOp.allowSceneActivation = false;
        while (!asyncOp.allowSceneActivation)
        {
            if (asyncOp.progress >= 0.9f)
            {
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.F1))
                    //Activate the Scene
                    asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
