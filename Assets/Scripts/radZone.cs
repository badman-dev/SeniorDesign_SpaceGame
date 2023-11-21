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

    private AudioSource radiationLowAudio;
    private AudioSource radiationMediumAudio;
    private AudioSource radiationHighAudio;

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

        radiationLowAudio = lowDmgObject.GetComponent<AudioSource>();
        radiationMediumAudio = medDmgObject.GetComponent<AudioSource>();
        radiationHighAudio = highDmgObject.GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        playerController player = null;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<playerController>();
        }

        Collider2D playerCollider = player.playerCollider;

        //Apply damage based on zone player is in. Damage tick rate determined by player controller.
        if (highDmgTrigger.IsTouching(playerCollider))
        {
            player.applyRadDamage(highDmgValue, true);
            if (!radiationHighAudio.isPlaying)
            {
                radiationHighAudio.Play();
                radiationMediumAudio.Pause();
                radiationLowAudio.Pause();
            }
        }
        else if (medDmgTrigger.IsTouching(playerCollider))
        {
            player.applyRadDamage(medDmgValue, true);
            if (!radiationMediumAudio.isPlaying)
            {
                radiationHighAudio.Pause();
                radiationMediumAudio.Play();
                radiationLowAudio.Pause();
            }
        }
        else if (lowDmgTrigger.IsTouching(playerCollider))
        {
            player.applyRadDamage(lowDmgValue, true);
            if (!radiationLowAudio.isPlaying)
            {
                radiationHighAudio.Pause();
                radiationMediumAudio.Pause();
                radiationLowAudio.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        playerController player = null;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<playerController>();
        }

        Collider2D playerCollider = player.playerCollider;

        if (!highDmgTrigger.IsTouching(playerCollider) && !medDmgTrigger.IsTouching(playerCollider) && !lowDmgTrigger.IsTouching(playerCollider))
        {
            radiationHighAudio.Pause();
            radiationMediumAudio.Pause();
            radiationLowAudio.Pause();
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
