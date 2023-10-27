using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get {return _instance; } }

    private int mainAstCount1, sideAstCount1, sideAstCount2; //0, 1, 2

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
                mainAstCount1++;
                Debug.Log("Picked up goal asteroid");
                break;
            case 1:
                sideAstCount1++;
                Debug.Log("Picked up bonus asteroid type 1");
                break;
            case 2:
                sideAstCount2++;
                Debug.Log("Picked up bonus asteroid type 2");
                break;
        }
        UpdateObjectiveUI();
    }

    private void UpdateObjectiveUI()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
