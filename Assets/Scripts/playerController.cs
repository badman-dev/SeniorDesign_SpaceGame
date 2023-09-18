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

    public int thrusterStrength = 5;
    public int brakeStrength = 5;
    public float maximumVelocity = 10;
    public bool thrustRelative = false;

    private Vector2 currentThrusterAxisValue;
    private float currentBrakeValue;

    // Start is called before the first frame update
    void Start()
    {
        //get reference to rigidbody
        rb = this.GetComponent<Rigidbody2D>();

        //Activate actions (without this the input will not register)
        thrusterAction.action.Enable();
        brakesAction.action.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        //save current input values
        currentThrusterAxisValue = thrusterAction.action.ReadValue<Vector2>();
        currentBrakeValue = brakesAction.action.ReadValue<float>();

        //add force based on directional input
        if (currentThrusterAxisValue != Vector2.zero)
        {
            if (thrustRelative)
            {
                rb.AddRelativeForce(currentThrusterAxisValue * thrusterStrength);
            }
            else
            {
                rb.AddForce(currentThrusterAxisValue * thrusterStrength);
            }

            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maximumVelocity);
        }
        //Debug.Log("Current Velocity: " + rb.velocity);

        if (currentBrakeValue != 0)
        {
            //TODO: holding brake should activate thrusters in opposite direction to current velocity so ship holds in place
            //possibly this should also apply to rotation? or separate rotation brake?
        }
    }
}
