using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radZone : MonoBehaviour
{
    [Header("Trigger references")]
    public CircleCollider2D lowDmgTrigger;
    public CircleCollider2D medDmgTrigger;
    public CircleCollider2D highDmgTrigger;

    [Header("Zone Radius Values")]
    public bool setColliderRadiusAutomatically = true;
    public float lowDmgRadius = 10;
    public float medDmgRadius = 5;
    public float highDmgRadius = 3;

    [Header("Zone Damage Values")]
    public float lowDmgValue = 2;
    public float medDmgValue = 4;
    public float highDmgValue = 6;

    // Start is called before the first frame update
    void Start()
    {
        if (setColliderRadiusAutomatically)
        {
            lowDmgTrigger.radius = lowDmgRadius;
            medDmgTrigger.radius = medDmgRadius;
            highDmgTrigger.radius = highDmgRadius;
        }
        else
        {
            lowDmgRadius = lowDmgTrigger.radius;
            medDmgRadius = medDmgTrigger.radius;
            highDmgRadius = highDmgTrigger.radius;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        playerController player;
        collision.TryGetComponent<playerController>(out player);
        if (player == null)
            return;

        Collider2D playerCollider = player.GetComponent<Collider2D>();

        if (highDmgTrigger.IsTouching(playerCollider))
        {
            Debug.Log("Player within high rad damage zone");
        }
        else if (medDmgTrigger.IsTouching(playerCollider))
        {
            Debug.Log("Player within med rad damage zone");
        }
        else if (lowDmgTrigger.IsTouching(playerCollider))
        {
            Debug.Log("Player within low rad damage zone");
        }
    }

    private void OnDrawGizmos()
    {
        if (setColliderRadiusAutomatically)
        {
            lowDmgTrigger.radius = lowDmgRadius;
            medDmgTrigger.radius = medDmgRadius;
            highDmgTrigger.radius = highDmgRadius;
        }
    }
}
