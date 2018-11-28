using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUtilities : MonoBehaviour {

    public static IEnumerator ShowTextSmoothly(float fadeInTimeSec, float fadeOutTimeSec, float shownTimeSec, Text text, Text nextText)
    {
        yield return FadeInText(text, fadeInTimeSec);
        yield return new WaitForSeconds(shownTimeSec);
        nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, 1);
        do
        {
            yield return null;
        } while (!Input.anyKeyDown);
        nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, 0);
        yield return FadeOutText(text, fadeOutTimeSec);
    }

    public static IEnumerator FadeInText(Text text, float seconds)
    {
        for (float i = 0; i < 255; i+=5)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, i / 255);
            yield return new WaitForSecondsRealtime(seconds / 51);
        }
    }

    public static IEnumerator FadeOutText(Text text, float seconds)
    {
        for (float i = 255; i >= 0; i-=5)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, i / 255);
            yield return new WaitForSecondsRealtime(seconds / 51);
        }
    }
}
