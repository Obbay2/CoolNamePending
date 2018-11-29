using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;

public class FadeInText : MonoBehaviour {

    public TextMeshProUGUI[] uiElement;
    private bool HMDActive;
    public float FadeInTime = 3.0f;

    public void Start()
    {
        HMDActive = OpenVR.IsHmdPresent();
        StartCoroutine(FadeCanvasGroup(uiElement));
    }

    public IEnumerator FadeCanvasGroup(TextMeshProUGUI[] cg)
    {

        if (HMDActive)
        {
            yield return new WaitForSeconds(1.0f);
            SteamVR_Fade.View(Color.clear, FadeInTime);
            yield return new WaitForSeconds(FadeInTime);
        }

        for (int i = 0; i < cg.Length; i++)
        {
            /*timeSinceStartTime = Time.time - startTime;
            percentage = timeSinceStartTime / lerpTime;

            float curr = Mathf.Lerp(start, end, percentage);
            System.Console.WriteLine(curr);

            cg[i].alpha = curr;

            if (percentage >= 1)
            {
                i++;
                if (i < cg.Length)
                {
                    start = cg[i].alpha;
                }
                startTime = Time.time;
                percentage = 0;
            }

            yield return new WaitForEndOfFrame();*/
            for (float j = 0; j < 255; j += 1)
            {
                cg[i].color = new Color(cg[i].color.r, cg[i].color.g, cg[i].color.b, j / 255);
                yield return new WaitForSecondsRealtime(1 / 51);
            }
        }


    }

}
