using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LoreDumpManager : MonoBehaviour
{
    public Text contentWindow;
    [TextArea]
    public string introText;
    public float charDisplayWaitTime = .05f;
    public float startDelay = 1f;

    public bool playAudio = true;
    public AudioSource audSource;
    public AudioClip[] textBlips;

    private bool skipToEndOfText = false;
    private bool displayTextIsFinished = false;
    private bool displayTextIsRunning = false;
    private float startDelayTimer = 0;

    private void FixedUpdate()
    {
        if (startDelayTimer < startDelay)
            startDelayTimer += Time.deltaTime;
        else if (!displayTextIsFinished)
        {
            displayTextGradual(contentWindow, introText, charDisplayWaitTime);
        }
    }

    public void displayTextGradual(Text textBody, string content, float waitTimeSeconds = .1f, bool replaceExistingText = true)
    {
        StartCoroutine(displayTextGradualRoutine(textBody, content, waitTimeSeconds, replaceExistingText));
    }

    //display text one character at a time asynchronously
    private IEnumerator displayTextGradualRoutine(Text textBody, string content, float waitTimeSeconds = .1f, bool replaceExistingText = true)
    {
        if (displayTextIsRunning)
            yield break;

        displayTextIsRunning = true;

        char[] contentSplit = content.ToCharArray();

        if (replaceExistingText)
            textBody.text = "";

        for (int i = 0; i < contentSplit.Length; i++)
        {
            //check if player clicked to skip text printout
            if (skipToEndOfText)
            {
                skipToEndOfText = false;
                textBody.text = content;
                break;
            }

            textBody.text += contentSplit[i];

            if (playAudio && audSource != null)
                audSource.PlayOneShot(textBlips[Mathf.FloorToInt(Random.Range(0, textBlips.Length - .01f))]);

            yield return new WaitForSeconds(waitTimeSeconds);
        }

        displayTextIsRunning = false;
        displayTextIsFinished = true;
        yield return null;
    }

}
