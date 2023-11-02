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

    public GameObject fadeScreen;
    public float fadeTime = 1f;
    public GameObject deathPanel;
    public TextMeshProUGUI inventoryText;
    private int goalAstCountA, bonusAstCountA, bonusAstCountB; //0, 1, 2
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
                goalAstCountA++;
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
        UpdateObjectiveUI();
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
        Image fadeImage = fadeScreen.GetComponent<Image>();
        Color newColor = fadeImage.color;
        newColor.a = 1;
        fadeImage.color = newColor;

        float currentTime = 0;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            newColor.a = currentTime / fadeTime;
            fadeImage.color = newColor;
            yield return null;
        }
        deathPanel.SetActive(true);
    }

    private void UpdateObjectiveUI()
    {
        inventoryText.text = "Goal Asteroids: " + goalAstCountA + "\nBonus Asteroids A: " + bonusAstCountA + "\nBonus Asteroids B: " + bonusAstCountB;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
