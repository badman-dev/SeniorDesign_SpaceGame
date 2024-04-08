using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class LoreDumpManager : MonoBehaviour
{
    [Header("Text stuff")]
    public Text contentWindow;
    [TextArea]
    public string introText;
    public float charDisplayWaitTime = .05f;
    public float startDelay = 1f;

    [Header("Continue Button settings")]
    public InputActionReference confirmAction;
    public Button continueBtn;
    public string sceneToLoad = "1_Asteroid";
    public float appearTimeSeconds = .3f;

    [Header("Audio Settings")]
    public bool playAudio = true;
    public AudioSource audSource;
    public AudioClip[] textBlips;
    public AudioClip buttonSound;

    private bool skipToEndOfText = false;
    private bool displayTextIsFinished = false;
    private bool displayTextIsRunning = false;
    private float startDelayTimer = 0;

    private void Start()
    {
        continueBtn.onClick.AddListener(() => {
            audSource.PlayOneShot(buttonSound);
            LevelManager.Instance.ChangeScene(sceneToLoad);
        });

        confirmAction.action.Enable();
    }

    private void FixedUpdate()
    {
        if (startDelayTimer < startDelay)
            startDelayTimer += Time.deltaTime;
        else if (!displayTextIsFinished)
        {
            displayTextGradual(contentWindow, introText, charDisplayWaitTime);
        }
        else if (!continueBtn.gameObject.activeInHierarchy)
        {
            continueBtn.gameObject.SetActive(true);
            float fullYScale = continueBtn.transform.localScale.y;
            continueBtn.transform.localScale = new Vector3(continueBtn.transform.localScale.x, .0f, continueBtn.transform.localScale.z);
            continueBtn.transform.DOScaleY(fullYScale, appearTimeSeconds);
        }
    }

    private void Update()
    {
        //listen for mouse click in case player tries to skip to the end of the text immediately
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            skipToEndOfText = true;
        }

        if (displayTextIsFinished && confirmAction.action.WasPressedThisFrame())
        {
            continueBtn.onClick.Invoke();
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
                if (replaceExistingText)
                    textBody.text = content;
                else
                {
                    while (i < contentSplit.Length)
                    {
                        textBody.text += contentSplit[i];
                        i++;
                    }
                }
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
