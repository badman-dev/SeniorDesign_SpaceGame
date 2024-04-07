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
    [HideInInspector]
    public GameObject dmgSpriteController;
    [HideInInspector]
    public SpriteRenderer dmgSprite;
    [HideInInspector]
    public Color dmgAlpha;

    [HideInInspector]
    public SpriteRenderer playerSprite;
    [HideInInspector]
    public GameObject damageSystem;
    [HideInInspector]
    public GameObject deathSystemWhite;
    [HideInInspector]
    public GameObject deathSystemWhite2;
    [HideInInspector]
    public GameObject deathSystemOrange;
    [HideInInspector]
    public GameObject deathSystemOrange2;

    public InputActionReference thrusterAction;
    public InputActionReference brakesAction;
    public InputActionReference rotationAction;
    public InputActionReference sideDashAction;
    public InputActionReference deployDrillAction;
    public InputActionReference retractDrillAction;

    [Header("Drill Prefab")]
    public GameObject drillPrefab;

    [Header("Thrust Settings")]
    public int thrusterStrength = 5;
    public float maximumVelocity = 10;
    [Header("Rotational Thrust Settings")]
    [Range(0, 1)]
    public float thrusterRotationStrength = 5;
    public float maximumTorque = 20;
    public float rotationStrafeStrength = 3;
    public float rotationInputDeadzone = .2f;
    [Header("Dash Settings")]
    public float dashCooldownSeconds = 1;
    public float dashDuration = .25f;
    public float dashDistance = 1;
    [Header("Brake Settings")]
    public int brakeStrength = 5;
    public bool applyBrakeToRotation = false;
    public float brakeRotationStrength = .1f;
    public float brakeFullStopThreshold = .1f;


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

    //animator
    Animator animator;

    private float radDmgTimer;
    private float dashTimer = 0;
    [HideInInspector]
    public bool isDrillDeployed = false;

    private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        radDmgTimer = radDmgTickRateSeconds; //makes sure the player gets immediately damaged first time they enter a rad zone

        //get reference to rigidbody
        rb = this.GetComponent<Rigidbody2D>();
        //get reference to collider
        playerCollider = this.GetComponent<Collider2D>();
        //get reference to animator
        animator = this.GetComponent<Animator>();
        animator.SetBool("IsHarvesting", false);
        //get reference to player sprite
        playerSprite = this.GetComponent<SpriteRenderer>();

        //Gets the damage state sprite
        dmgSpriteController = GameObject.Find("DamageState");
        dmgSprite = dmgSpriteController.GetComponent<SpriteRenderer>();
        dmgAlpha = dmgSprite.color;
        dmgAlpha.a = 0;

        //Test
        //gets reference to particle systems
        damageSystem = GameObject.Find("FireSystem");
        deathSystemWhite = GameObject.Find("DeathSystemWhite1");
        deathSystemWhite2 = GameObject.Find("DeathSystemWhite2");
        deathSystemOrange = GameObject.Find("DeathSystemOrange1");
        deathSystemOrange2 = GameObject.Find("DeathSystemOrange2");
        //EndTest

        //Activate actions (without this the inputs will not register)
        thrusterAction.action.Enable();
        brakesAction.action.Enable();
        rotationAction.action.Enable();
        sideDashAction.action.Enable();
        deployDrillAction.action.Enable();
        retractDrillAction.action.Enable();

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

        //animator controller
        animator.SetBool("IsMovingForward", false);
        animator.SetBool("IsMovingBack", false);
        animator.SetBool("IsMovingLeft", false);
        animator.SetBool("IsMovingRight", false);
        

        //add forward/backward thrust
        if (currentThrusterAxisValue != 0)
        {
            rb.AddRelativeForce(new Vector2(0, currentThrusterAxisValue * thrusterStrength * Time.deltaTime));

            if (currentThrusterAxisValue > 0)
            {
                animator.SetBool("IsMovingForward", true);
            }
            else if (currentThrusterAxisValue < 0)
            {
                animator.SetBool("IsMovingBack", true);
            }

        }

        //add torque based on rotation direction input
        if (currentThrusterRotateValue != 0 && Mathf.Abs(currentThrusterRotateValue) > rotationInputDeadzone)
        {
            rb.AddTorque(currentThrusterRotateValue * thrusterRotationStrength * Time.deltaTime);

            //add some horizontal force when turning to keep the flying from being totally uncontrollable (hopefully)
            rb.AddRelativeForce(Vector2.right * Mathf.Sign(currentThrusterRotateValue * -1) * rotationStrafeStrength * currentThrusterAxisValue * (rb.velocity.magnitude / maximumVelocity) * Time.deltaTime);


            if (currentThrusterRotateValue > 0)
            {
                animator.SetBool("IsMovingLeft", true);
            }
            else if (currentThrusterRotateValue < 0)
            {
                animator.SetBool("IsMovingRight", true);
            }
        }

        //handle side dash input

        if (currentSideDashInputValue != 0 && dashTimer >= dashCooldownSeconds)
        {
            //remove relative horizontal component from player velocity
            Vector2 relativeForwardVelocity = new Vector2(0, transform.InverseTransformDirection(rb.velocity).y);
            rb.velocity = transform.TransformDirection(relativeForwardVelocity);

            if (currentSideDashInputValue > 0)
            {
                animator.SetTrigger("DodgeRight");

            }
            else if (currentSideDashInputValue < 0)
            {
                animator.SetTrigger("DodgeLeft");
            }

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
        if (currentBrakeValue != 0 && rb.velocity.magnitude >= brakeFullStopThreshold)
        {
            rb.AddForce(rb.velocity.normalized * -1 * brakeStrength * currentBrakeValue * Time.deltaTime);

            if (applyBrakeToRotation)
                rb.AddTorque(rb.angularVelocity * -1 * brakeRotationStrength * currentBrakeValue * Time.deltaTime);
        }
        else if (currentBrakeValue != 0 && rb.velocity.magnitude < brakeFullStopThreshold)
        {
            //if going slow enough, just come to a full stop
            rb.velocity = Vector3.zero;
        }

        //keep incrementing radiation timer until enough time has elapsed to allow the player to be damaged by radiation again
        if (radDmgTimer < radDmgTickRateSeconds)
        {
            radDmgTimer += Time.deltaTime;
        }


        //drill spawner. If drill is already active, do not spawn
        if (deployDrillAction.action.WasPressedThisFrame())
        {
            if (!isDrillDeployed)
            {
                attachDrill();
                Debug.Log("drill was attached");

                animator.SetBool("IsHarvesting", true);
                isDrillDeployed = true;
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("drillPrefab"));
                animator.SetBool("IsHarvesting", false);
                isDrillDeployed = false;

            }
        }
        ////drill despawner
        //if (retractDrillAction.action.WasPressedThisFrame() && isDrillDeployed)
        //{
        //    Destroy(GameObject.FindGameObjectWithTag("drillPrefab"));
        //    isDrillDeployed = false;
        //}


        lerpSpeed = 3f * Time.deltaTime;
        AdjustHealthBar();
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
        //Test
        DamageAlpha();
        //EndTest
        CheckDamage();
        CheckDead();
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

    public void giveHealth(float healthAmount)
    {
        currentPlayerHealth += healthAmount;
        healthText.text = currentPlayerHealth.ToString();
    }

    public void setHealth(float amount)
    {
        currentPlayerHealth = amount;
        healthText.text = currentPlayerHealth.ToString();
        
        CheckDead();
    }

    public float getCurrentHealth()
    {
        return currentPlayerHealth;
    }

    public void AdjustHealthBar()
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
        }
        else
        {
            healthBar.CrossFadeAlpha(1, .1f, false);
        }
    }

    private void CheckDead()
    {
        if (currentPlayerHealth <= 0)
        {
            LevelManager.Instance.StartPlayerDeath();
            Destroy(playerSprite);
            Destroy(dmgSprite);
            deathSystemWhite.GetComponent<ParticleSystem>().Play();
            deathSystemWhite2.GetComponent<ParticleSystem>().Play();
            deathSystemOrange.GetComponent<ParticleSystem>().Play();
            deathSystemOrange2.GetComponent<ParticleSystem>().Play();
        }
    }

    public void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (getCurrentHealth() / startingPlayerHealth));
        healthBar.color = healthColor;
    }


    //-----------------------
    //Drill-Related Functions
    //-----------------------


    //adds drill prefab as child of player gameobject
    private void attachDrill()
    {
        GameObject PlayerRef = GameObject.Find("Player");

        GameObject go = Instantiate(drillPrefab, transform.position, transform.rotation) as GameObject;
        go.transform.parent = GameObject.Find("Player").transform;
        GameObject.Find("link1").GetComponent<HingeJoint2D>().connectedBody = rb;
    }

    //Test
    public void DamageAlpha()
    {
        dmgAlpha.a = 1 - (currentPlayerHealth / startingPlayerHealth);

        dmgSprite.color = dmgAlpha;

        Debug.Log("playerController: Alpha: " + dmgAlpha.a.ToString());
    }

    public void CheckDamage()
    {
        if (currentPlayerHealth <= 50)
        {
            Debug.Log(damageSystem.name);
            damageSystem.GetComponent<ParticleSystem>().Play();
        }
    }
}
