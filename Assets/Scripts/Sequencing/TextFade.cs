using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    public bool startFadedOut = true;

    TextMeshPro text;
    bool isFadingIn = false;
    bool isFadingOut = false;
    void Start()
    {
        text = gameObject.GetComponent<TextMeshPro>();

        if (text == null)
            Debug.LogError("No TextMeshPro found on " + transform.gameObject.name + "!");
        
        text.faceColor = new Color32(text.faceColor.r, text.faceColor.g, text.faceColor.b, 0);
    }

    public void FadeIn(float fadeTime)
    {
        if (isFadingIn)
            return;
        if (isFadingOut)
            isFadingOut = false;
        isFadingIn = true;

        StartCoroutine(FadeInRoutine(fadeTime));
    }

    private IEnumerator FadeInRoutine(float fadeTime)
    {
        float currentTime = 0;

        while (isFadingIn && currentTime < fadeTime) {
            currentTime += Time.deltaTime;
            byte newAlpha = (byte)Mathf.Lerp(0, 255, currentTime / fadeTime);
            text.faceColor = new Color32(text.faceColor.r, text.faceColor.g, text.faceColor.b, newAlpha);
            yield return null;
        }
    }

    public void FadeOut(float fadeTime)
    {
        if (isFadingOut)
            return;
        if (isFadingIn)
            isFadingIn = false;
        isFadingOut = true;
        
        StartCoroutine(FadeOutRoutine(fadeTime));
    }

    private IEnumerator FadeOutRoutine(float fadeTime)
    {
        float currentTime = 0;

        while (isFadingOut && currentTime < fadeTime) {
            currentTime += Time.deltaTime;
            byte newAlpha = (byte)Mathf.Lerp(255, 0, currentTime / fadeTime);
            text.faceColor = new Color32(text.faceColor.r, text.faceColor.g, text.faceColor.b, newAlpha);
            yield return null;
        }
    }
}
