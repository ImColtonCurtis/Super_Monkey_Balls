using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GlobalFunctions : MonoBehaviour
{
    public static IEnumerator FadeImage(float animLength, Image myImage, bool fadingIn)
    {
        float timer = 0;
        int startAlpha = 0, endAlpha = 1;
        if (!fadingIn)
        {
            startAlpha = 1;
            endAlpha = 0;
        }

        while (timer <= animLength)
        {
            myImage.color = Color.Lerp(new Color(myImage.color.r, myImage.color.g, myImage.color.b, startAlpha),
                new Color(myImage.color.r, myImage.color.g, myImage.color.b, endAlpha),
                timer / animLength);
            timer++;
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator FadeTMP(float animLength, TextMeshProUGUI myTMP, bool fadingIn)
    {
        float timer = 0;
        int startAlpha = 0, endAlpha = 1;
        if (!fadingIn)
        {
            startAlpha = 1;
            endAlpha = 0;
        }

        while (timer <= animLength)
        {
            myTMP.color = Color.Lerp(new Color(myTMP.color.r, myTMP.color.g, myTMP.color.b, startAlpha),
                new Color(myTMP.color.r, myTMP.color.g, myTMP.color.b, endAlpha),
                timer / animLength);
            timer++;
            yield return new WaitForFixedUpdate();
        }
    }
}
