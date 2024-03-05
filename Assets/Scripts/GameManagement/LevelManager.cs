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

    private int goalAstCount, bonusAstCountA, bonusAstCountB; //0, 1, 2
    private bool restartingLevel = false;

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

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            resumeGame();
        else
            pauseGame();
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        foreach (InputAction item in inputActions.actionMaps[0].actions)
        {
            item.Disable();
        }
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        foreach (InputAction item in inputActions.actionMaps[0].actions)
        {
            item.Enable();
        }
    }

    public void AddPickup(int type)
    {
        switch (type)
        {
            case 0:
                goalAstCount++;
                Debug.Log("Picked up goal asteroid");
                break;
            case 1:
                bonusAstCountA++;
                Debug.Log("Picked up bonus asteroid type 1");
                break;
            case 2:
                bonusAstCountB++;
                Debug.Log("Picked up bonus asteroid type 2");
                break;
        }

        UIManager.Instance.UpdateObjectiveUI(goalAstCount, bonusAstCountA, bonusAstCountB);
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

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
