using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get {return _instance; } }

    private int goalAstCount, bonusAstCountA, bonusAstCountB; //0, 1, 2
    private bool restartingLevel = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
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
            restartingLevel = true;
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
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
