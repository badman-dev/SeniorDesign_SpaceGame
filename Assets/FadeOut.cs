using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private bool fadeCheck = false;
    [SerializeField] private float timer = 0f;


    void Update()
    {
        if (fadeCheck == false && timer >= 2f)
        {
            {
                if (myUIGroup.alpha >= 0)
                {
                    myUIGroup.alpha -= Time.deltaTime * .5f;
                }
            }
        }
        if (myUIGroup.alpha == 0)
        {
            fadeCheck = true;   
        }
        timer += Time.deltaTime;
    }

}
