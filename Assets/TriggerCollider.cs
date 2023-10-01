using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollider : MonoBehaviour
{
    public Collider2D target;
    public UnityEvent onEnter;
    public UnityEvent onExit;
    Collider2D col;

    void Start()
    {
        col = gameObject.GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("No collider found on " + transform.gameObject.name + "!");
        }
        else if (!col.isTrigger)
        {
            Debug.LogError("Collider not trigger on " + transform.gameObject.name + "!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == target)
            onEnter.Invoke();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == target)
            onExit.Invoke();
    }
}
