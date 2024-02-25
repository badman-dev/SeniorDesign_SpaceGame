using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralPick : MonoBehaviour
{
    public Sprite[] sprites;
    void Start()
    {
        //choose mineral sprite from list of sprites. Maybe change to a hue select in future
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
