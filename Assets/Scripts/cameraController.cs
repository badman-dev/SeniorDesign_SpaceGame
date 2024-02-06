using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    public playerController player;
    public float cameraSpeed = 0.5f;
    public float maximumDistance = 1;

    private Vector3 pushDir;

    private void Update()
    {
        //convert player velocity to vector3 and make sure it's relative to player position and velocity
        Vector3 targetPos =
            new Vector3(
            player.rb.velocity.x,
            player.rb.velocity.y,
            transform.position.z);
        Vector3.ClampMagnitude(targetPos, maximumDistance);

        targetPos = (targetPos) + player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, /*player.rb.velocity.magnitude **/ cameraSpeed);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, pushDir, Color.red);
        Debug.DrawLine(player.transform.position, player.transform.position + player.transform.up, Color.blue);
    }
}
