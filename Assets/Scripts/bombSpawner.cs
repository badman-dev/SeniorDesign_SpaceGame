using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombSpawner : MonoBehaviour
{
    public Rigidbody2D bombPrefab;
    public float bombSpeed = .1f;
    public float timeBetweenBombs = 5f;
    public GameObject ship;
    void Update()
    {
        
    }

    public void FireBomb()
    {
        Rigidbody2D newBomb = Instantiate(bombPrefab, transform.position, transform.rotation);
        Vector2 direction = transform.position - ship.transform.position;
        newBomb.velocity = direction;
        Destroy(newBomb.gameObject, 3);
    }
}
