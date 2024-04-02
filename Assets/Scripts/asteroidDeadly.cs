using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class asteroidDeadly : MonoBehaviour
{
    public float maxCollisionDmg = 10;
    [Tooltip("collision damage stops scaling with velocity if the player is moving faster than this value")]
    public float terminalVelocity = 5;
    public float minimumVelocity = 2;

    public AudioClip[] damageSounds;
    public AudioSource audioSource;

    public UnityEvent onCollision;

    public ParticleSystem particleSystem;

    private void Start()
    {
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollision.Invoke();

        

        playerController player;
        collision.gameObject.TryGetComponent<playerController>(out player);

        //collision damage scales with velocity up to a max velocity value
        if (player != null)
        {
            if (collision.relativeVelocity.magnitude >= terminalVelocity)
            {
                player.applyDamage(maxCollisionDmg);
                playRandomHitSound();
            }
            else if (collision.relativeVelocity.magnitude >= minimumVelocity)
            {
                player.applyDamage(Mathf.Lerp(0, maxCollisionDmg, (player.rb.velocity.magnitude / terminalVelocity)));
                playRandomHitSound();

                var em = particleSystem.emission;
                em.enabled = true;
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.position = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0.0f);
                particleSystem.Emit(emitParams, 1);
                particleSystem.Play();
            }
        }
    }

    private void playRandomHitSound()
    {
        if (damageSounds.Length != 0)
            audioSource.PlayOneShot(damageSounds[Random.Range(0, damageSounds.Length - 1)]);
    }
}
