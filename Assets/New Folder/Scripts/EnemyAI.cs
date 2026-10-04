using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float stoppingDistance = 2f;

    [Header("Attack")]
    public float attackDamage = 10f;
    public float attackRate = 1f;

    [Header("Animation")]
    public Animator animator;

    [Header("References")]
    public Transform player;
    public PlayerHealth playerHealth;

    private float nextAttackTime = 0f;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (player == null || playerHealth == null)
        {
            SetMoving(false);
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            SetMoving(false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            MoveTowardPlayer();
            SetMoving(true);
        }
        else
        {
            SetMoving(false);
            AttackPlayer();
        }

        LookAtPlayer();
    }

    private void MoveTowardPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        playerHealth.TakeDamage(attackDamage);
        nextAttackTime = Time.time + attackRate;
    }

    private void LookAtPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection == Vector3.zero)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void SetMoving(bool isMoving)
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }
    }
}