using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInText : MonoBehaviour {

    public CanvasGroup[] uiElement;

    public void Start()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, 1));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup[] cg, float end, float lerpTime = 4.0f)
    {

        float startTime = Time.time;
        float timeSinceStartTime = Time.time - startTime;
        float percentage = timeSinceStartTime / lerpTime;
        int i = 0;
        float start = cg[i].alpha;
        while (i < cg.Length)
        {
            timeSinceStartTime = Time.time - startTime;
            percentage = timeSinceStartTime / lerpTime;

            float curr = Mathf.Lerp(start, end, percentage);

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

            yield return new WaitForEndOfFrame();
        }

        print("done");
    }
}
