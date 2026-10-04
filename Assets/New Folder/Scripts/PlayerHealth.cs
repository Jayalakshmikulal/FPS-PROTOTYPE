using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;

    [Header("UI References")]
    public Image healthFillImage;
    public Image damageFlashImage;

    [Header("Feedback")]
    public float damageFlashAlpha = 0.35f;
    public float damageFlashFadeSpeed = 4f;
    public AudioSource audioSource;
    public AudioClip hurtSound;

    private float currentHealth;
    private bool isDead = false;
    private Coroutine damageFlashCoroutine;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (damageFlashImage != null)
        {
            Color flashColor = damageFlashImage.color;
            flashColor.a = 0f;
            damageFlashImage.color = flashColor;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Player took " + damageAmount + " damage. Health left: " + currentHealth);

        UpdateHealthUI();
        PlayDamageFeedback();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private void PlayDamageFeedback()
    {
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

        if (damageFlashImage != null)
        {
            if (damageFlashCoroutine != null)
            {
                StopCoroutine(damageFlashCoroutine);
            }

            damageFlashCoroutine = StartCoroutine(DamageFlashRoutine());
        }
    }

    private IEnumerator DamageFlashRoutine()
    {
        Color flashColor = damageFlashImage.color;
        flashColor.a = damageFlashAlpha;
        damageFlashImage.color = flashColor;

        while (damageFlashImage.color.a > 0f)
        {
            flashColor = damageFlashImage.color;
            flashColor.a -= damageFlashFadeSpeed * Time.deltaTime;
            damageFlashImage.color = flashColor;

            yield return null;
        }

        flashColor = damageFlashImage.color;
        flashColor.a = 0f;
        damageFlashImage.color = flashColor;
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0f;
        UpdateHealthUI();

        Debug.Log("Player died.");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}