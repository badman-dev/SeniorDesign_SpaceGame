using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralPick : MonoBehaviour
{
    public Sprite[] sprites;
    void Start()
    {
        //choose sprite from list of selected sprites
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
