using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpin : MonoBehaviour
{
    public enum SpeedType 
    {
        RandomSpeedFromRange,
        FixedSpeed
    }
    public SpeedType speedType = SpeedType.RandomSpeedFromRange;
    public float minRandomSpeed = 5;
    public float maxRandomSpeed = 20;
    public float fixedRotateSpeed = 10;
    public bool isCounterClockwise = true;
    private float speed = 0;
    private bool spinning = false;
    public PolygonCollider2D asteroid;

    void Start()
    {
        CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        float transformScale = transform.localScale.x;
        if (transformScale > 1)
        {
            col.radius = 25;
        }
        else
        {
            col.radius = 25 / transformScale;
        }

        switch (speedType)
        {
            case SpeedType.RandomSpeedFromRange:
                int directionMultiplier = (int)Random.Range(0, 2);
                if (directionMultiplier == 0) { directionMultiplier = -1; }
                speed = Random.Range(minRandomSpeed, maxRandomSpeed) * directionMultiplier;
                break;
            case SpeedType.FixedSpeed:
                speed = fixedRotateSpeed * (isCounterClockwise ? 1 : -1);
                break;
        }
    }

    void StartSpinning()
    {
        spinning = true;
        StartCoroutine(SpinRoutine());
    }

    void StopSpinning()
    {
        spinning = false;
    }

    IEnumerator SpinRoutine()
    {
        while (spinning)
        {
            transform.Rotate (0 ,0 , speed * Time.deltaTime);
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StartSpinning();
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bomb")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StopSpinning();
        }
    }
}
