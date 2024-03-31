using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get {return _instance; } }

    [HideInInspector]
    public int totalGoalAstCount, totalBonusAstCountA, totalBonusAstCountB; //0, 1, 2 //Total amount of these asteroids collected in play session
    [HideInInspector]
    public int currentLvlGoalAstCount, currentLvlBonusAstCountA, currentLvlBonusAstCountB; //amount of these asteroids collected this level
    [HideInInspector]
    public int currentLvlTotalGoal, currentLvlTotalBonusA, currentLvlTotalBonusB; //Total amount of these asteroids
    private bool restartingLevel = false;
    [HideInInspector]
    public float currentLvlTime = 0;

    [HideInInspector]
    public bool isGamePaused = false;
    private bool isTrackingTime = true;

    public InputActionAsset inputActions;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += onLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onLevelFinishedLoading;
    }

    private void onLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        currentLvlGoalAstCount = 0;
        currentLvlBonusAstCountA = 0;
        currentLvlBonusAstCountB = 0;

        currentLvlTotalGoal = 0;
        currentLvlTotalBonusA = 0;
        currentLvlTotalBonusB = 0;

        currentLvlTime = 0;

        ResourcePickupPrim[] allPickups = FindObjectsOfType<ResourcePickupPrim>();

        for (int i = 0; i < allPickups.Length; i++)
        {
            switch (allPickups[i].pickupType)
            {
                case ResourcePickupPrim.PickupType.GoalObjective:
                    currentLvlTotalGoal++;
                    break;
                case ResourcePickupPrim.PickupType.BonusObjectiveA:
                    currentLvlTotalBonusA++;
                    break;
                case ResourcePickupPrim.PickupType.BonusObjectiveB:
                    currentLvlTotalBonusB++;
                    break;
            }
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            resumeGame();
        else
            pauseGame();
    }

    private void Update()
    {
        if (isTrackingTime)
            currentLvlTime += Time.deltaTime;
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        foreach (InputAction item in inputActions.actionMaps[0].actions)
        {
            item.Disable();
        }

        isTrackingTime = false;
        isGamePaused = true;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        foreach (InputAction item in inputActions.actionMaps[0].actions)
        {
            item.Enable();
        }

        isTrackingTime = true;
        isGamePaused = false;
    }

    public void AddPickup(int type)
    {
        switch (type)
        {
            case 0:
                totalGoalAstCount++;
                currentLvlGoalAstCount++;
                Debug.Log("Picked up goal asteroid");
                break;
            case 1:
                totalBonusAstCountA++;
                currentLvlBonusAstCountA++;
                Debug.Log("Picked up bonus asteroid type 1");
                break;
            case 2:
                totalBonusAstCountB++;
                currentLvlBonusAstCountB++;
                Debug.Log("Picked up bonus asteroid type 2");
                break;
        }

        UIManager.Instance.UpdateObjectiveUI(currentLvlGoalAstCount, currentLvlBonusAstCountA, currentLvlBonusAstCountB);
    }

    public void StartPlayerDeath()
    {
        if (!restartingLevel)
        {
            StartCoroutine(PlayerDeathRoutine());
        }
    }

    private IEnumerator PlayerDeathRoutine()
    {
        Image fadeImage = UIManager.Instance.fadeScreen.GetComponent<Image>();
        Color newColor = fadeImage.color;
        newColor.a = 1;
        fadeImage.color = newColor;
        float fadeTime = UIManager.Instance.fadeTime;

        float currentTime = 0;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            newColor.a = currentTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        UIManager.Instance.deathPanel.SetActive(true);

        restartingLevel = false;
    }

    public void nextScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
