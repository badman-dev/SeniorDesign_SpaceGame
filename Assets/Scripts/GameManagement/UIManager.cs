using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get {return _instance; } }

    public TextMeshProUGUI asteroidGoalText;
    public TextMeshProUGUI asteroidBonusText1;
    public TextMeshProUGUI asteroidBonusText2;

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
    }

    public void UpdateObjectiveUI(int goalAstCount, int bonusAstCountA, int bonusAstCountB) {
        asteroidGoalText.text = goalAstCount.ToString();
        asteroidBonusText1.text = bonusAstCountA.ToString();
        asteroidBonusText2.text = bonusAstCountB.ToString();
    }
}
