using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Collider2D playerCollider;
    public InputActionReference thrusterAction;
    public InputActionReference brakesAction;
    public InputActionReference rotationAction;
    public InputActionReference sideDashAction;

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
    [Header("Dash Settings")]
    public float dashCooldownSeconds = 1;
    public float dashDuration = .25f;
    public float dashDistance = 1;

    [Header("Player Stats")]
    public TMPro.TextMeshProUGUI healthText;
    public float startingPlayerHealth = 100f;
    private float currentPlayerHealth;
    public float radDmgTickRateSeconds = 1;
    public Image healthBar;
    private float timeSinceLastHit = 0f;
    private float lerpSpeed;

    [HideInInspector]
    public float currentThrusterAxisValue;
    [HideInInspector]
    public float currentBrakeValue;
    [HideInInspector]
    public float currentThrusterRotateValue;
    [HideInInspector]
    public float currentSideDashInputValue;

    private float radDmgTimer;
    private float dashTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        radDmgTimer = radDmgTickRateSeconds; //makes sure the player gets immediately damaged first time they enter a rad zone

        //get reference to rigidbody
        rb = this.GetComponent<Rigidbody2D>();
        //get reference to collider
        playerCollider = this.GetComponent<Collider2D>();

        //Activate actions (without this the inputs will not register)
        thrusterAction.action.Enable();
        brakesAction.action.Enable();
        rotationAction.action.Enable();
        sideDashAction.action.Enable();

        //display player health (change this to actual ui later)
        healthText.text = startingPlayerHealth.ToString();
        currentPlayerHealth = startingPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastHit += Time.deltaTime;
        //save current input values
        currentThrusterAxisValue = thrusterAction.action.ReadValue<float>();
        currentBrakeValue = brakesAction.action.ReadValue<float>();
        currentThrusterRotateValue = rotationAction.action.ReadValue<float>();
        currentSideDashInputValue = sideDashAction.action.ReadValue<float>();

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

        //handle side dash input

        if (currentSideDashInputValue != 0 && dashTimer >= dashCooldownSeconds)
        {
            //remove relative horizontal component from player velocity
            Vector2 relativeForwardVelocity = new Vector2(0, transform.InverseTransformDirection(rb.velocity).y);
            rb.velocity = transform.TransformDirection(relativeForwardVelocity);

            rb.DOMove(transform.position + (transform.right * currentSideDashInputValue * dashDistance), dashDuration);
            dashTimer = 0;
        }
        if (dashTimer >= dashCooldownSeconds)
        {
            dashTimer = dashCooldownSeconds;
        }
        else
        {
            dashTimer += Time.deltaTime;
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
        //TODO: holding the brake button shifts player back and forth very slightly instead of coming to a stop
        //(maybe because brake Strength isn't zero so it overshoots a little every time unless you're very quick?
        if (currentBrakeValue != 0)
        {
            rb.AddForce(rb.velocity.normalized * -1 * brakeStrength * currentBrakeValue * Time.deltaTime);

            if (applyBrakeToRotation)
                rb.AddTorque(rb.angularVelocity * -1 * brakeRotationStrength * currentBrakeValue * Time.deltaTime);
        }

        //keep incrementing radiation timer until enough time has elapsed to allow the player to be damaged by radiation again
        if (radDmgTimer < radDmgTickRateSeconds)
        {
            radDmgTimer += Time.deltaTime;
        }

        lerpSpeed = 3f * Time.deltaTime;
        AdjustHealth();
        ColorChanger();
    }

    //----------------------------------
    //Player Stat Modification Functions
    //----------------------------------

    public void applyDamage(float dmgAmount, bool logDamage = false)
    {
        currentPlayerHealth -= dmgAmount;
        healthText.text = currentPlayerHealth.ToString();

        if (logDamage)
            Debug.Log("playerController: damage taken: " + dmgAmount);
        timeSinceLastHit = 0f;
    }

    //Tick rate for rad damage determined by player so that rad damage doesn't stack up in overlapping zones
    public void applyRadDamage(float dmgAmount, bool logDamage = false)
    {
        if (radDmgTimer >= radDmgTickRateSeconds)
        {
            applyDamage(dmgAmount, logDamage);
            radDmgTimer = 0;
        }
        timeSinceLastHit = 0f;
    }

    public void giveHealth(float hlthAmount)
    {
        currentPlayerHealth += hlthAmount;
        healthText.text = currentPlayerHealth.ToString();
    }

    public void setHealth(float amount)
    {
        currentPlayerHealth = amount;
        healthText.text = currentPlayerHealth.ToString();
    }

    public float getCurrentHealth()
    {
        return currentPlayerHealth;
    }

    public void AdjustHealth()
    {
        float currentHealth = getCurrentHealth();
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (currentHealth / startingPlayerHealth) / 2, lerpSpeed);
        HealthFade();
    }

    public void HealthFade()
    {
        if (timeSinceLastHit > 3)
        {
            healthBar.CrossFadeAlpha(0, .5f, false);
        } else
        {
            healthBar.CrossFadeAlpha(1, .1f, false);
        }
    }
    public void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (getCurrentHealth() / startingPlayerHealth));
        healthBar.color = healthColor;
    }
}
