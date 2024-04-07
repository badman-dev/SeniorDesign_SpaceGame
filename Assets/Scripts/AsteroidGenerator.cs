using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    [Header("Generate Asteroid Sprite From List")]
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        //make sprite of asteroid from set list
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        //make polygon collider from picked sprite.
        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
