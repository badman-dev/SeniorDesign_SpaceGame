using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get {return _instance; } }

    public TextMeshProUGUI inventoryText;
    private int goalAstCountA, bonusAstCountA, bonusAstCountB; //0, 1, 2

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

    private void UpdateObjectiveUI()
    {
        inventoryText.text = "Goal Asteroids: " + goalAstCountA + "\nBonus Asteroids A: " + bonusAstCountA + "\nBonus Asteroids B: " + bonusAstCountB;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
