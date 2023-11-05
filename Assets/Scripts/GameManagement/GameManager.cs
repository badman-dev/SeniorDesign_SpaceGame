using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    string currentSceneName = "";

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
