using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
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
    public float rotationStrafeStrength = 3;

    [Header("Player Stats")]
    public TMPro.TextMeshProUGUI healthText;
    public float playerHealth = 100f;

    [HideInInspector]
    public float currentThrusterAxisValue;
    [HideInInspector]
    public float currentBrakeValue;
    [HideInInspector]
    public float currentThrusterRotateValue;

    // Start is called before the first frame update
    void Start()
    {
        //get reference to rigidbody
        rb = this.GetComponent<Rigidbody2D>();

        //Activate actions (without this the inputs will not register)
        thrusterAction.action.Enable();
        brakesAction.action.Enable();
        rotationAction.action.Enable();

        //display player health (change this to actual ui later)
        healthText.text = playerHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //save current input values
        currentThrusterAxisValue = thrusterAction.action.ReadValue<float>();
        currentBrakeValue = brakesAction.action.ReadValue<float>();
        currentThrusterRotateValue = rotationAction.action.ReadValue<float>();

        //add forward/backward thrust
        if (currentThrusterAxisValue != 0)
        {
            rb.AddRelativeForce(new Vector2(0, currentThrusterAxisValue * thrusterStrength * Time.deltaTime));
        }

        //add torque based on rotation direction input
        if (currentThrusterRotateValue != 0)
        {
            rb.AddTorque(currentThrusterRotateValue * thrusterRotationStrength * Time.deltaTime);
            
            //add some horizontal force when turning to keep the flying from being totally uncontrollable (hopefully)
            rb.AddRelativeForce(Vector2.right * Mathf.Sign(currentThrusterRotateValue * -1) * rotationStrafeStrength * currentThrusterAxisValue * (rb.velocity.magnitude / maximumVelocity) * Time.deltaTime);
        }

        //clamp torque in general, not just when thrusting
        if (Mathf.Abs(rb.angularVelocity) > maximumTorque)
        {
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, maximumTorque * -1, maximumTorque);
        }

        //clamp velocity in general, not just when thrusting
        if (rb.velocity.magnitude > maximumVelocity)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maximumVelocity);
        }
        
        //apply counter-thrust if brake is pressed
        if (currentBrakeValue != 0)
        {
            rb.AddForce(rb.velocity.normalized * -1 * brakeStrength * currentBrakeValue * Time.deltaTime);

            if (applyBrakeToRotation)
                rb.AddTorque(rb.angularVelocity * -1 * brakeRotationStrength * currentBrakeValue * Time.deltaTime);
        }
    }

    //----------------------------------
    //Player Stat Modification Functions
    //----------------------------------

    public void applyDamage(float dmgAmount)
    {
        playerHealth -= dmgAmount;
        healthText.text = playerHealth.ToString();
    }

    public void giveHealth(float hlthAmount)
    {
        playerHealth += hlthAmount;
        healthText.text = playerHealth.ToString();
    }

    public void setHealth(float amount)
    {
        playerHealth = amount;
        healthText.text = playerHealth.ToString();
    }
}
