using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcePickupPrim : MonoBehaviour
{
    public enum PickupType 
    {
        GoalObjective,
        BonusObjectiveA,
        BonusObjectiveB
    }

    public PickupType pickupType;

    public UnityEvent onPickup;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            LevelManager.Instance.AddPickup((int)pickupType);
            transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            transform.parent.GetComponent<Collider2D>().enabled = false;
            onPickup.Invoke();
        }
    }
}
