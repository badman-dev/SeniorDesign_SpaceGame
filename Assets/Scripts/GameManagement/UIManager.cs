using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get {return _instance; } }

    public GameObject fadeScreen;
    public float fadeTime = 1f;
    public GameObject deathPanel;
    public GameObject levelEndPanel;
    public GameObject pauseMenuPanel;
    public Text stats;
    public Button btnContinue;
    public float waitBetweenChars = .01f;
    public float waitBetweenLines = .75f;

    public TextMeshProUGUI asteroidGoalText;
    public TextMeshProUGUI asteroidBonusText1;
    public TextMeshProUGUI asteroidBonusText2;

    [Header("Input")]
    public InputActionReference pauseAction;
    public InputActionReference confirmAction;

    [Header("Audio Settings")]
    public bool playAudio = true;
    public AudioSource audSource;
    public AudioClip[] textBlips;
    public AudioClip buttonSound;

    private bool skipToEndOfText = false;
    private bool displayTextIsFinished = true;
    private bool lvlEnded = false;
    [HideInInspector]
    public bool isGamePausedWithMenu = false;
    private bool listenForContinue = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start() {
        UpdateObjectiveUI(0, 0, 0);

        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() =>
            {
                if (audSource && buttonSound)
                    audSource.PlayOneShot(buttonSound);
            });
        }

        pauseAction.action.Enable();
        confirmAction.action.Enable();
    }

    public void UpdateObjectiveUI(int goalAstCount, int bonusAstCountA, int bonusAstCountB) {
        asteroidGoalText.text = goalAstCount.ToString() + "/" + LevelManager.Instance.currentLvlTotalGoal;
        asteroidBonusText1.text = bonusAstCountA.ToString() + "/" + LevelManager.Instance.currentLvlTotalBonusA;
        asteroidBonusText2.text = bonusAstCountB.ToString() + "/" + LevelManager.Instance.currentLvlTotalBonusB;
    }

    private void Update()
    {
        //listen for mouse click in case player tries to skip to the end of the text immediately
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            skipToEndOfText = true;
        }

        if (pauseAction.action.WasPressedThisFrame())
        {
            if (LevelManager.Instance.isGamePaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }

        if (listenForContinue && confirmAction.action.WasPressedThisFrame())
        {
            listenForContinue = false;
            btnContinue.onClick.Invoke();
        }
    }

    public void endLevel()
    {
        StartCoroutine(endLevelPanelRoutine());
    }

    public void restartRevel()
    {
        LevelManager.Instance.restartLevel();
    }

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void nextScene()
    {
        LevelManager.Instance.nextScene();
    }

    public void resume()
    {
        pauseMenuPanel.SetActive(false);
        isGamePausedWithMenu = false;
        LevelManager.Instance.resumeGame();
    }

    public void pause()
    {
        pauseMenuPanel.SetActive(true);
        isGamePausedWithMenu = true;
        LevelManager.Instance.pauseGame();
    }

    private IEnumerator endLevelPanelRoutine()
    {
        if (lvlEnded)
            yield break;

        lvlEnded = true;

        levelEndPanel.SetActive(true);

        displayTextIsFinished = false;
        float collectedA = LevelManager.Instance.currentLvlBonusAstCountA;
        float totalA = LevelManager.Instance.currentLvlTotalBonusA;
        float fraction = (LevelManager.Instance.currentLvlTotalBonusA != 0) ? collectedA / totalA : 1;
        float asteroidsGotPercentTypeA = fraction * 100;
        StartCoroutine(displayTextGradualRoutine(stats, "Type A Asteroids Mined: " + Mathf.Floor(asteroidsGotPercentTypeA) + "%", waitBetweenChars, true));
        while (!displayTextIsFinished)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(waitBetweenLines);

        displayTextIsFinished = false;
        float collectedB = LevelManager.Instance.currentLvlBonusAstCountB;
        float totalB = LevelManager.Instance.currentLvlTotalBonusB;
        fraction = (LevelManager.Instance.currentLvlTotalBonusB != 0) ? collectedB / totalB : 1;
        float asteroidsGotPercentTypeB = fraction * 100;
        StartCoroutine(displayTextGradualRoutine(stats, "\nType B Asteroids Mined: " + Mathf.Floor(asteroidsGotPercentTypeB) + "%", waitBetweenChars, false));
        while (!displayTextIsFinished)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(waitBetweenLines);

        displayTextIsFinished = false;
        string time = TimeSpan.FromSeconds(LevelManager.Instance.currentLvlTime).Minutes + ":" + TimeSpan.FromSeconds(LevelManager.Instance.currentLvlTime).Seconds;
        StartCoroutine(displayTextGradualRoutine(stats, "\nTime: " + time, waitBetweenChars, false));
        while (!displayTextIsFinished)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(waitBetweenLines);

        //displayTextIsFinished = false;
        //StartCoroutine(displayTextGradualRoutine(stats, "\nPar Time: " + 00 + ":" + 00, waitBetweenChars, false));
        //while (!displayTextIsFinished)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitForSecondsRealtime(waitBetweenLines);


        //make continue btn appear
        btnContinue.transform.localScale =
            new Vector3(
                btnContinue.transform.localScale.x,
                .01f,
                btnContinue.transform.localScale.z
            );
        btnContinue.gameObject.SetActive(true);
        btnContinue.transform.DOScaleY(1, .3f);

        yield return null;
    }

    //display text one character at a time asynchronously
    private IEnumerator displayTextGradualRoutine(Text textBody, string content, float waitTimeSeconds = .1f, bool replaceExistingText = true)
    {

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
                audSource.PlayOneShot(textBlips[Mathf.FloorToInt(UnityEngine.Random.Range(0, textBlips.Length - .01f))]);

            yield return new WaitForSeconds(waitTimeSeconds);
        }
        displayTextIsFinished = true;

        yield return null;
    }
}
