using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;

    [Header("Score Settings")]
    public int scorePerHit = 10;
    public int scoreOnDestroy = 60;

    [Header("UI")]
    public Image healthFillImage;

    [Header("Hit Feedback")]
    public Renderer targetRenderer;
    public Color hitColor = Color.white;
    public float flashTime = 0.08f;
    public AudioSource audioSource;
    public AudioClip hitSound;

    private float currentHealth;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        UpdateHealthUI();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scorePerHit);
        }

        UpdateHealthUI();
        PlayHitFeedback();

        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health left: " + currentHealth);

        if (currentHealth <= 0f)
        {
            DestroyTarget();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private void PlayHitFeedback()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (targetRenderer != null)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }

            flashCoroutine = StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        targetRenderer.material.color = hitColor;

        yield return new WaitForSeconds(flashTime);

        if (targetRenderer != null)
        {
            targetRenderer.material.color = originalColor;
        }
    }

    private void DestroyTarget()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreOnDestroy);
        }

        Debug.Log(gameObject.name + " destroyed.");
        Destroy(gameObject);
    }
}