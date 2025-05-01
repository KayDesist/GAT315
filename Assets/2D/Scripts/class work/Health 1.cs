using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelay = 0;

    [Header("Damage Feedback")]
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField][Range(0, 1)] private float hurtSoundVolume = 0.7f;

    [Header("Events")]
    [SerializeField] private UnityEvent onDeath;

    private bool isDead = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine damageFlashCoroutine;
    private AudioSource audioSource;

    public float GetCurrentHealth() // New method for UI access
    {
        return health;
    }

    private void Awake()
    {
        health = maxHealth;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) originalColor = spriteRenderer.color;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ApplyDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        PlayHurtSound();
        TriggerDamageFeedback();

        if (health <= 0) Die();
    }

    private void PlayHurtSound()
    {
        if (hurtSound != null) audioSource.PlayOneShot(hurtSound, hurtSoundVolume);
    }

    private void TriggerDamageFeedback()
    {
        if (spriteRenderer == null) return;

        if (damageFlashCoroutine != null) StopCoroutine(damageFlashCoroutine);
        damageFlashCoroutine = StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        damageFlashCoroutine = null;
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        health = Mathf.Min(health + amount, maxHealth);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        health = 0;
        onDeath?.Invoke();

        if (CompareTag("Player")) GameManager.Instance?.OnPlayerDeath();
        if (destroyOnDeath) Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<DamageDealer>(out var dealer))
        {
            ApplyDamage(dealer.damageAmount);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<DamageDealer>(out var dealer))
        {
            ApplyDamage(dealer.damageAmount);
        }
    }
}

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10;
}