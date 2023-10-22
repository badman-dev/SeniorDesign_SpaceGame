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

    [Header("Radar Visualization")]
    public GameObject lowDmgObject;
    public GameObject medDmgObject;
    public GameObject highDmgObject;

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

        lowDmgObject.transform.localScale = new Vector2(lowDmgRadius * 2, lowDmgRadius * 2);
        medDmgObject.transform.localScale = new Vector2(medDmgRadius * 2, medDmgRadius * 2);
        highDmgObject.transform.localScale = new Vector2(highDmgRadius * 2, highDmgRadius * 2);
    }
}
