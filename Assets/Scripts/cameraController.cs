using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class cameraController : MonoBehaviour
{

    public playerController player;
    public float cameraSpeed = 0.5f;
    public float camSpeedMultiplierOnDash = .01f;
    public float maximumDistance = 1;
    public float cooldownOnDashInSeconds = .1f;
    public float slowmodeCooldownSeconds = 1;

    private float disableMovementTimer = 999;
    private float currentDeltaMultiplier = 1;
    private float slowmodeTimer = 999;

    private void Update()
    {
        if (player.sideDashAction.action.WasPressedThisFrame())
        {
            disableMovementTimer = 0;
            slowmodeTimer = 0;
        }

        //do not move if dash was just pressed
        if (disableMovementTimer < cooldownOnDashInSeconds)
        {
            disableMovementTimer += Time.deltaTime;
        }
        else
        {
            if (slowmodeTimer < slowmodeCooldownSeconds)
            {
                slowmodeTimer += Time.deltaTime;
                currentDeltaMultiplier = Mathf.Lerp(camSpeedMultiplierOnDash, 1, (slowmodeTimer / slowmodeCooldownSeconds));
            }

            //convert player velocity to vector3 and make sure it's relative to player position and velocity
            Vector3 targetPos =
                new Vector3(
                player.rb.velocity.x,
                player.rb.velocity.y,
                transform.position.z);
            Vector3.ClampMagnitude(targetPos, maximumDistance);

            targetPos = (targetPos) + player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, cameraSpeed * currentDeltaMultiplier);
        }
    }
}
