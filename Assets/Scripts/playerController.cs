using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public InputActionReference thrusterAction;
    public InputActionReference brakesAction;
    public InputActionReference rotationAction;

    [Header("Thrust Settings")]
    public int thrusterStrength = 5;
    public int brakeStrength = 5;
    public float maximumVelocity = 10;
    [Header("Rotational Thrust Settings")]
    public bool applyBrakeToRotation = false;
    [Range(0, 1)]
    public float brakeRotationStrength = .1f;
    public float thrusterRotationStrength = 5;
    public float maximumTorque = 20;

    private float currentThrusterAxisValue;
    private float currentBrakeValue;
    private float currentThrusterRotateValue;

    // Start is called before the first frame update
    void Start()
    {
        //get reference to rigidbody
        rb = this.GetComponent<Rigidbody2D>();

        //Activate actions (without this the inputs will not register)
        thrusterAction.action.Enable();
        brakesAction.action.Enable();
        rotationAction.action.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        //save current input values
        currentThrusterAxisValue = thrusterAction.action.ReadValue<float>();
        currentBrakeValue = brakesAction.action.ReadValue<float>();
        currentThrusterRotateValue = rotationAction.action.ReadValue<float>();

        //add forward/backward thrust
        //TODO: play with the physics on this more, maybe figure out a way to make the horizontal momentum slow when the player rotates?
        if (currentThrusterAxisValue != 0)
        {
            rb.AddRelativeForce(new Vector2(currentThrusterAxisValue * thrusterStrength * Time.deltaTime, 0));

            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maximumVelocity);
        }

        //add torque based on rotation direction input
        if (currentThrusterRotateValue != 0)
        {
            rb.AddTorque(currentThrusterRotateValue * thrusterRotationStrength * Time.deltaTime);
        }

        //clamp torque in general, not just when thrusting
        if (rb.angularVelocity > maximumTorque)
        {
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, maximumTorque * -1, maximumTorque);
        }
        
        //apply counter-thrust if brake is pressed
        if (currentBrakeValue != 0)
        {
            rb.AddForce(rb.velocity.normalized * -1 * brakeStrength * currentBrakeValue * Time.deltaTime);

            if (applyBrakeToRotation)
                rb.AddTorque(rb.angularVelocity * -1 * brakeRotationStrength * currentBrakeValue * Time.deltaTime);
        }
    }
}