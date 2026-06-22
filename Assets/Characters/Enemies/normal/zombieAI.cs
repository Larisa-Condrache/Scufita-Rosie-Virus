using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 8f;
    public float attackRange = 1.2f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private ZombieHealth health;

    private float lastAttackTime;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<ZombieHealth>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || isDead)
            return;

        if (health != null && health.IsDead())
        {
            Die();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StopMoving();
            Attack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    void MoveTowardsPlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        animator.SetBool("isWalking", true);

        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetBool("isWalking", false);
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        animator.SetTrigger("attack");

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
            playerHealth.TakeDamage(damage);
    }

    void Die()
    {
        isDead = true;
        StopMoving();

        animator.SetBool("isDead", true);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 2f);
    }
}