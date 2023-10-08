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

    void Start()
    {
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

    void Update()
    {
        transform.Rotate (0 ,0 , speed * Time.deltaTime);
    }
}
