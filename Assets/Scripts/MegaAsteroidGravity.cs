using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaAsteroidGravity : MonoBehaviour
{
    public playerController player;

    public float maxGravStrength = 7.5f;
    public float minDistance = 15;

    private void FixedUpdate()
    {
        //calculate gravity vector
        Vector2 asteroidPos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPos2D = new Vector2(player.transform.position.x, player.transform.position.y);

        Vector2 gravDir = (asteroidPos2D - playerPos2D);
        gravDir = Vector2.ClampMagnitude(gravDir, 1);
        Debug.Log("Grav direction: " + gravDir);

        float playerDist = Vector3.Distance(asteroidPos2D, playerPos2D);
        float gravStrength = 1;
        if (playerDist > minDistance)
        {
            gravStrength = Mathf.Lerp(0, maxGravStrength, playerDist / minDistance);
        }
        else
        {
            gravStrength = maxGravStrength;
        }

        gravDir *= gravStrength;
        //Debug.Log("Grav direction scaled: " + gravDir);

        //apply force
        player.rb.AddForce(gravDir);
    }


}
