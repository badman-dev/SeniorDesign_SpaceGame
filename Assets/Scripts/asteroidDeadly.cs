using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidDeadly : MonoBehaviour
{
    public float maxCollisionDmg = 10;
    [Tooltip("collision damage stops scaling with velocity if the player is moving faster than this value")]
    public float terminalVelocity = 5;
    public float minimumVelocity = 2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerController player;
        collision.gameObject.TryGetComponent<playerController>(out player);

        //collision damage scales with velocity up to a max velocity value
        if (player != null)
        {
            if (collision.relativeVelocity.magnitude >= terminalVelocity)
                player.applyDamage(maxCollisionDmg);
            else if (collision.relativeVelocity.magnitude >= minimumVelocity)
                player.applyDamage(Mathf.Lerp(0, maxCollisionDmg, (player.rb.velocity.magnitude / terminalVelocity)));
        }
    }
}
