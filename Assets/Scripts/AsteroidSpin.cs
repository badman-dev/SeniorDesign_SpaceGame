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
    private float speed = 0;

    void Start()
    {
        switch (speedType)
        {
            case SpeedType.RandomSpeedFromRange:
                speed = Random.Range(minRandomSpeed, maxRandomSpeed);
                break;
            case SpeedType.FixedSpeed:
                speed = fixedRotateSpeed;
                break;
        }
    }

    void Update()
    {
        transform.Rotate (0 ,0 , speed * Time.deltaTime);
    }
}
