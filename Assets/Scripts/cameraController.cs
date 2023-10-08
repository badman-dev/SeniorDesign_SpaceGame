using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    public playerController player;
    public float cameraSpeed = 0.5f;
    public float distanceMultiplier = 1;

    private Vector3 pushDir;

    private void Update()
    {
        //convert player velocity to vector3 and make sure it's relative to player position and thruster value
        Vector3 targetPos = new Vector3(player.rb.velocity.normalized.x * distanceMultiplier, player.rb.velocity.normalized.y * distanceMultiplier, 0);
        targetPos = (targetPos) + player.transform.position;

        pushCameraTowardsPosition(targetPos, cameraSpeed * player.rb.velocity.magnitude);
    }

    public void pushCameraTowardsPosition(Vector3 targetPos, float pushStrength)
    {
        pushDir = targetPos - transform.position;
        pushDir.Normalize();
        pushDir = new Vector3(pushDir.x, pushDir.y, 0);

        transform.position += pushDir * pushStrength;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, pushDir, Color.red);
        Debug.DrawLine(player.transform.position, player.transform.position + player.transform.up, Color.blue);
    }
}
