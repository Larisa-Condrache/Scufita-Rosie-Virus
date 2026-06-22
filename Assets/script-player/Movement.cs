using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public float runSpeed = 5f;
    public float stealthSpeed = 3f;

    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    public bool isDead = false;

    public float knifeAttackDuration = 0.45f;
    public float pistolAttackDuration = 0.35f;
    public float rifleAttackDuration = 0.4f;

    [Header("Attack Damage")]
    public int knifeDamage = 25;
    public int pistolDamage = 20;
    public int rifleDamage = 35;

    [Header("Attack Range")]
    public Transform attackPoint;
    public float knifeRange = 0.8f;
    public float pistolRange = 6f;
    public float rifleRange = 10f;
    public LayerMask enemyLayer;

    [Header("Attack Timing")]
    public float knifeHitDelay = 0.2f;
    public float pistolHitDelay = 0.15f;
    public float rifleHitDelay = 0.15f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float currentSpeed;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = runSpeed;

        UpdateWeaponAnimator();
    }

    void Update()
    {
        UpdateWeaponAnimator();

        if (isDead)
        {
            moveInput = 0f;
            ResetMovementAnimations();
            animator.SetBool("isDead", true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.J) &&
            WeaponManager.Instance.currentWeapon != 0 &&
            !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (isAttacking)
        {
            moveInput = 0f;
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
                        Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        bool isWalking = isMoving && Input.GetKey(KeyCode.LeftControl);
        bool isRunning = isMoving && !Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.localScale = new Vector3(1, 1, 1);
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.localScale = new Vector3(-1, 1, 1);

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        animator.SetBool("isJumping", !isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        currentSpeed = isWalking ? stealthSpeed : runSpeed;
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float targetSpeed = moveInput * currentSpeed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                                 (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y *
                                 (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void UpdateWeaponAnimator()
    {
        if (WeaponManager.Instance == null)
            return;

        animator.SetInteger("weaponType", WeaponManager.Instance.currentWeapon);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        moveInput = 0f;

        ResetMovementAnimations();

        int weapon = WeaponManager.Instance.currentWeapon;

        animator.SetInteger("weaponType", weapon);
        animator.SetBool("isAttacking", true);

        float duration = 0.4f;
        float hitDelay = 0.15f;

        if (weapon == 1)
        {
            duration = knifeAttackDuration;
            hitDelay = knifeHitDelay;
        }
        else if (weapon == 2)
        {
            duration = pistolAttackDuration;
            hitDelay = pistolHitDelay;
        }
        else if (weapon == 3)
        {
            duration = rifleAttackDuration;
            hitDelay = rifleHitDelay;
        }

        yield return new WaitForSeconds(hitDelay);

        DealDamage(weapon);

        float remainingTime = duration - hitDelay;

        if (remainingTime > 0)
            yield return new WaitForSeconds(remainingTime);

        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }

    void DealDamage(int weapon)
    {
        if (attackPoint == null)
            return;

        float range = 0f;
        int damage = 0;

        if (weapon == 1)
        {
            range = knifeRange;
            damage = knifeDamage;
        }
        else if (weapon == 2)
        {
            range = pistolRange;
            damage = pistolDamage;
        }
        else if (weapon == 3)
        {
            range = rifleRange;
            damage = rifleDamage;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            range,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            ZombieHealth zombie = enemy.GetComponent<ZombieHealth>();

            if (zombie != null)
                zombie.TakeDamage(damage);
        }
    }

    void ResetMovementAnimations()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, knifeRange);
    }
}