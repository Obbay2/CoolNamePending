using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeInText : MonoBehaviour {

    public TextMeshProUGUI[] uiElement;

    public void Start()
    {
        StartCoroutine(FadeCanvasGroup(uiElement));
    }

    public IEnumerator FadeCanvasGroup(TextMeshProUGUI[] cg)
    {
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
