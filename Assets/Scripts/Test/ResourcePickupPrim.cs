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

    public float timeToBreak = 3f;
    public GameObject finalDustCloud;

    public PickupType pickupType;

    public UnityEvent onPickup;

    void Start()
    {
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "drill")
        {
            
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "drill")
        {
            
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
       if (col.tag == "drill")
        {
            timeToBreak -= 1 * Time.deltaTime;

           if (timeToBreak <= 0)
            {
                FinishMining();
            }
        }

    }

    // spawn final mining particles, add score to pickup counter, invoke pickup, destroy mineral deposit
    public void FinishMining()
    {
        GameObject poof = Instantiate(finalDustCloud, transform.position + new Vector3(0,0,2), new Quaternion(0,0,0,0)) as GameObject;
        LevelManager.Instance.AddPickup((int)pickupType);
        onPickup.Invoke();
        Destroy(gameObject);
    }

}
