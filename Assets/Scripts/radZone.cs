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

    private int highestTier = 0;
    
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        playerController player = collision.gameObject.GetComponent<playerController>();
        PlayerAudioController playerAudio = collision.gameObject.GetComponent<PlayerAudioController>();
        Collider2D playerCollider = player.playerCollider;

        if (highDmgTrigger.IsTouching(playerCollider))
        {
            ManageHighestTier(playerAudio, 3);
        }
        else if (medDmgTrigger.IsTouching(playerCollider))
        {
            ManageHighestTier(playerAudio, 2);
        }
        else if (lowDmgTrigger.IsTouching(playerCollider))
        {
            ManageHighestTier(playerAudio, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        playerController player = collision.gameObject.GetComponent<playerController>();
        PlayerAudioController playerAudio = collision.gameObject.GetComponent<PlayerAudioController>();
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

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        playerController player = collision.gameObject.GetComponent<playerController>();
        PlayerAudioController playerAudio = collision.gameObject.GetComponent<PlayerAudioController>();
        Collider2D playerCollider = player.playerCollider;

        if (highDmgTrigger.IsTouching(playerCollider))
        {
            ManageHighestTier(playerAudio, 3);
        }
        else if (medDmgTrigger.IsTouching(playerCollider))
        {
            ManageHighestTier(playerAudio, 2);
        }
        else if (lowDmgTrigger.IsTouching(playerCollider))
        {
            ManageHighestTier(playerAudio, 1);
        }
        else {
            ManageHighestTier(playerAudio, 0);
        }
    }

    private void ManageHighestTier(PlayerAudioController playerAudio, int tier)
    {
        //Keep track of the highest tier in THIS radiation zone. Replace it's entry in the PlayerAudioController so that the PlayerAudioController will compare the highest tiers from each radiation zone the player is currently in

        if (highestTier != 0) { playerAudio.RemoveRadiationIndex(highestTier); }
        highestTier = tier;
        playerAudio.AddRadiationIndex(tier);
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
