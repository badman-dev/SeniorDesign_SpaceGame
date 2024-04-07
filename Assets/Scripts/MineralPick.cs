using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralPick : MonoBehaviour
{
    [Header("List of Mineral Sprites")]
    public Sprite[] sprites;
    void Start()
    {
        //choose mineral sprite from list of sprites. Maybe change to a hue select in future
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }

}
