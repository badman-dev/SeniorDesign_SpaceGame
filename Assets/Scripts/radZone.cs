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
        playerController player = new playerController();
        
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<playerController>();
        }

        Collider2D playerCollider = player.playerCollider;

        //Apply damage based on zone player is in. Damage tick rate determined by player controller.
        if (highDmgTrigger.IsTouching(playerCollider))
        {
            player.applyRadDamage(highDmgValue, true);
        }
        else if (medDmgTrigger.IsTouching(playerCollider))
        {
            player.applyRadDamage(medDmgValue, true);
        }
        else if (lowDmgTrigger.IsTouching(playerCollider))
        {
            player.applyRadDamage(lowDmgValue, true);
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
